using kifuwarabe_wcsc27.interfaces;
using System.Collections.Generic;
using kifuwarabe_wcsc27.abstracts;

#if DEBUG
using kifuwarabe_wcsc27.machine;
#endif

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 対局終了時の記録用だぜ☆
    /// 
    /// (2017-04-27 08:37) USI用にも拡張するぜ☆（*＾～＾*）
    /// </summary>
    public class Kifu
    {
        public Kifu()
        {
            this.SsList = new List<Move>();
            this.SyokiKyokumenFen = "";
        }

        /// <summary>
        /// 先頭に追加するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ss"></param>
        public void AddFirst(Move ss)
        {
            this.SsList.Insert(0, ss);
        }
        /// <summary>
        /// moves 以降の符号を指定しろだぜ☆（＾～＾）
        /// </summary>
        /// <param name="moves"></param>
        public void AddMoves(bool isSfen, string moves, Kyokumen.Sindanyo kys)
        {
            string[] fugoItiran = moves.Split(' ');

            foreach(string fugo in fugoItiran)
            {
                int caret = 0;
                if(!Med_Parser.TryFenMove(isSfen, fugo, ref caret,kys,out Move move))
                {
                    throw new System.Exception("指し手のパースエラー fugo=["+ fugo + "]");
                }
                SsList.Add(move);
            }
        }

        public void Setumei(bool isSfen, Mojiretu syuturyoku)
        {
            // 初期局面を作成
            Kyokumen ky2 = new Kyokumen();
            int caret = 0;
            ky2.ParsePositionvalue(isSfen,  SyokiKyokumenFen,ref caret, false, false, out string moves, syuturyoku);

            // 初期局面を出力
            syuturyoku.AppendLine("初期局面");
            Util_Information.Setumei_NingenGameYo(ky2,syuturyoku);

            int temeMade = 1;
            foreach (Move ss in this.SsList)
            {
                syuturyoku.Append("(");
                syuturyoku.Append(temeMade.ToString());
                syuturyoku.Append(")");
                ConvMove.AppendFenTo(isSfen, ss, syuturyoku);
                syuturyoku.Append(" ");
                temeMade++;
            }
            syuturyoku.AppendLine();
        }

        public void TusinYo(bool isSfen, Mojiretu syuturyoku)
        {
            // 初期局面を作成
            Kyokumen ky2 = new Kyokumen();
            int caret = 0;
            ky2.ParsePositionvalue(isSfen, SyokiKyokumenFen, ref caret, false, false, out string moves, syuturyoku);

            // 初期局面を出力
            syuturyoku.Append("< kifu, 初期局面, ");
            ky2.TusinYo_Line(isSfen, syuturyoku);

            syuturyoku.Append("< kifu, move = ");
            int temeMade = 1;
            foreach (Move ss in this.SsList)
            {
                ConvMove.AppendFenTo(isSfen, ss, syuturyoku);
                syuturyoku.Append(" ");
                temeMade++;
            }
            syuturyoku.AppendLine();
        }

        public List<Move> SsList { get; set; }
        public string SyokiKyokumenFen { get; set; }

        /// <summary>
        /// 終局図まで進めるぜ☆（＾～＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <param name="syuturyoku"></param>
        public void GoToFinish(bool isSfen, Kyokumen ky, Mojiretu syuturyoku)
        {
            ky.Clear(syuturyoku);

            int caret = 0;
            ky.ParsePositionvalue(isSfen,  SyokiKyokumenFen, ref caret,
                true//適用
                , false, out string moves, syuturyoku
                );

            // 棋譜を元に、局面データを再現するぜ☆
            foreach (Move ss in SsList)
            {
                Nanteme nanteme = new Nanteme();
                ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);

#if DEBUG
                Util_Commands.Ky(isSfen, "ky", ky, syuturyoku);
                //Util_Machine.Flush(syuturyoku);
                Util_Machine.Flush_USI(syuturyoku);
#endif
            }
        }
        /// <summary>
        /// 指定の手目まで進めるぜ☆（＾～＾）
        /// </summary>
        /// <param name="temeMade"></param>
        /// <param name="ky"></param>
        /// <param name="syuturyoku"></param>
        public void GoToTememade(bool isSfen, int temeMade, Kyokumen ky, Mojiretu syuturyoku)
        {
            // 棋譜を元に、局面データを再現するぜ☆
            ky.Clear(syuturyoku);
            int caret = 0;
            ky.ParsePositionvalue(isSfen, SyokiKyokumenFen, ref caret,
                true//適用
                , false, out string moves, syuturyoku
                );
            // 指定の手目まで進めるぜ☆（＾～＾）
            foreach (Move ss in SsList)
            {
                if (temeMade < 1)
                {
                    break;
                }
                Nanteme nanteme = new Nanteme();
                ky.DoMove(isSfen, ss, MoveType.N00_Karappo, ref nanteme, ky.Teban, syuturyoku);
                temeMade--;
            }
        }

        public void AppendMovesTo(bool isSfen, Mojiretu syuturyoku)
        {
            foreach (Move ss in SsList)
            {
                ConvMove.AppendFenTo(isSfen, ss, syuturyoku);
                syuturyoku.Append(" ");
            }
        }
    }
}

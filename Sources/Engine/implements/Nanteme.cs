using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// 局面状況。１方向のリンクリストになっているぜ☆（＾▽＾）
    /// 
    /// 役目☆
    /// ・取られた駒を覚えている☆
    /// ・千日手判定用の局面ハッシュを覚えている☆
    /// ・指し手別の勝率の成績表を作るために、指し手を覚えている☆
    /// </summary>
    public class Nanteme
    {
        public Nanteme()
        {
            this.ToraretaKs = Komasyurui.Yososu;//取られた駒の該当はなしだぜ☆（＾▽＾）
        }

        public void Clear()
        {
            this.ToraretaKs = Komasyurui.Yososu;//取られた駒の該当はなしだぜ☆（＾▽＾）
            this.SennititeHash = 0;
            this.Ittego = null; // リンクを切るぜ☆（＾▽＾）
            this.Ittemae = null; // リンクを切るぜ☆（＾▽＾）
        }

        /// <summary>
        /// 前の手目から引き継ぎたいものをここでコピーするぜ☆（＾▽＾）
        /// </summary>
        /// <param name="nanteme"></param>
        public void CopyPropertyFrom(Nanteme nanteme)
        {
        }

        /// <summary>
        /// 千日手判定だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsSennitite()
        {
            return Const_Game.SENNITITE_COUNT <= this.GetSennititeCount();
        }

        /// <summary>
        /// 千日手の判定だぜ☆（＾～＾）反復した回数を返すぜ☆
        /// 千日手になったら数えるのを止めるぜ☆
        /// </summary>
        /// <returns></returns>
        public int GetSennititeCount()
        {
            ulong expected = this.SennititeHash;
            int count = 1;// 同じ局面が出た回数☆

            for (Nanteme nanteme = this.Ittemae; null !=nanteme; nanteme = nanteme.Ittemae)
            {
                if(nanteme.SennititeHash == expected)
                {
                    count++;
                    if (Const_Game.SENNITITE_COUNT <= count)// 千日手だぜ☆（＾～＾）数えるのを止めるぜ☆
                    {
                        break;
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 千日手の判定のために局面を覚えておくぜ☆（＾▽＾）
        /// </summary>
        public ulong SennititeHash { get; set; }
        /// <summary>
        /// 指し手の勝率の成績表を作るために、指し手を覚えておくぜ☆（＾▽＾）
        /// </summary>
        public Move Move { get; set; }
        /// <summary>
        /// 読み筋に指し手タイプを出すことで、デバッグに使うために覚えておくぜ☆（＾▽＾）
        /// </summary>
        public MoveType MoveType { get; set; }

        /// <summary>
        /// 取られた駒の種類だぜ☆（＾▽＾）
        /// </summary>
        public Komasyurui ToraretaKs { get; set; }

        /// <summary>
        /// 一手後だぜ☆（＾▽＾）
        /// </summary>
        public Nanteme Ittego { get; set; }
        /// <summary>
        /// 一手前だぜ☆（＾▽＾）
        /// </summary>
        public Nanteme Ittemae { get; set; }

        /// <summary>
        /// 何手目かを数えるぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public int ScanNantemadeBango()
        {
            int nantemeBango = 0;

            // 先頭には投了（初期局面）が入っているぜ☆（＾▽＾）
            // 先頭の一手前はヌルだぜ☆

            // 先頭の投了（初期局面）まで戻るぜ☆
            Nanteme nanteme = this;
            for (; null != nanteme.Ittemae; nanteme = nanteme.Ittemae)
            {
                nantemeBango++;
            }
            // 初期局面は 0 手目と表示される計算だぜ☆（＾▽＾）

            return nantemeBango;
        }

        /// <summary>
        /// 読み筋を返すぜ☆（＾～＾）
        /// </summary>
        /// <param name="sentoNantemade">初期局面からのリンクリストなので、どの「図はn手まで」戻すか☆</param>
        /// <returns></returns>
        public void ScanYomisuji(bool isSfen, int sentoNantemade, Mojiretu syuturyoku)
        {
            // 先頭（投了、初期局面、図は0手まで）まで戻るぜ☆
            Nanteme nanteme = this;
            for (; null != nanteme.Ittemae; nanteme = nanteme.Ittemae)
            {
            }

            // 先頭から今までの読み筋をつなげるぜ☆（＾▽＾）
            int zuhaNantemade = 0; // 図はn手まで
            for (;
                null != nanteme;// 一番最後まで回すぜ☆（＾▽＾）
                nanteme = nanteme.Ittego)
            {
                if (sentoNantemade <= zuhaNantemade)
                {
#if !UNITY
                    syuturyoku.Append("(");
                    syuturyoku.Append(zuhaNantemade.ToString());// 「図はn手まで」の数字
                    syuturyoku.Append(")");
#endif
                    ConvMove.AppendFenTo(isSfen, nanteme.Move, syuturyoku);
                    syuturyoku.Append(" ");

#if !UNITY
                    // おまけ
                    AbstractConvMoveType.Setumei(nanteme.MoveType, syuturyoku);
                    syuturyoku.Append(" ");
#endif
                }
                zuhaNantemade++;
            }
        }
    }
}

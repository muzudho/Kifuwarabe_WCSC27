using System;
using System.Collections.Generic;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using Grayscale.Kifuwarakei.Entities.Logging;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public class SeisekiMove
    {
        public SeisekiMove(Move move, Move ousyu, int version, int kati, int hikiwake, int make, SeisekiKyokumen owner)
        {
            this.Owner = owner;
            this.Move = move;
            this.Ousyu = ousyu;
            this.Version = version;
            this.Kati = kati;
            this.Hikiwake = hikiwake;
            this.Make = make;
        }

        public SeisekiKyokumen Owner { get; private set; }
        /// <summary>
        /// 指し手
        /// </summary>
        public Move Move { get; }
        /// <summary>
        /// 応手
        /// </summary>
        public Move Ousyu { get; }
        /// <summary>
        /// 成績を登録したソフトのバージョン
        /// </summary>
        public int Version { get { return this.m_version_; } set { this.m_version_ = value; this.Owner.Owner.Edited = true; } }
        private int m_version_;
        /// <summary>
        /// 勝った回数
        /// </summary>
        public int Kati { get { return this.m_kati_; } set { this.m_kati_ = value; this.Owner.Owner.Edited = true; } }
        private int m_kati_;
        /// <summary>
        /// 引き分けた回数
        /// </summary>
        public int Hikiwake { get { return this.m_hikiwake_; } set { this.m_hikiwake_ = value; this.Owner.Owner.Edited = true; } }
        private int m_hikiwake_;
        /// <summary>
        /// 負けた回数
        /// </summary>
        public int Make { get { return this.m_make_; } set { this.m_make_ = value; this.Owner.Owner.Edited = true; } }
        private int m_make_;

        public static bool TryParse(Kyokumen ky, string commandline, ref int caret, out SeisekiMove out_result, SeisekiKyokumen owner, StringBuilder syuturyoku)
        {
            bool successfule = true;
            // 指し手☆
            if (!Med_Parser.TryFenMove(Option_Application.Optionlist.USI, commandline, ref caret, ky.Sindan, out Move ss))
            {
                successfule = false;
            }

            // 応手☆
            if (!Med_Parser.TryFenMove(Option_Application.Optionlist.USI, commandline, ref caret, ky.Sindan, out Move ss2))
            {
                successfule = false;
            }

            // バージョン（これは無いこともある☆ 評価値のパーサーを使いまわし）
            if (!Conv_Hyokati.TryParse(commandline, ref caret, out int version, syuturyoku))
            {
                version = 0;
                //successfule = false;
            }

            // 勝った回数（評価値のパーサーを使いまわし）
            if (!Conv_Hyokati.TryParse(commandline, ref caret, out int kati, syuturyoku))
            {
                successfule = false;
            }

            // 引き分けた回数
            if (!Conv_Hyokati.TryParse(commandline, ref caret, out int hikiwake, syuturyoku))
            {
                successfule = false;
            }

            // 負けた回数
            if (!Conv_Hyokati.TryParse(commandline, ref caret, out int make, syuturyoku))
            {
                successfule = false;
            }

            out_result = new SeisekiMove(ss, ss2, version, kati, hikiwake, make, owner);
            return successfule;
        }

        /// <summary>
        /// 勝率☆
        /// </summary>
        /// <returns></returns>
        public float GetSyoritu()
        {
            return (float)this.Kati / (float)(this.Kati + this.Hikiwake + this.Make);
        }

        public void ToContents_NotUnity(bool isSfen, StringBuilder syuturyoku)
        {
            ConvMove.AppendFenTo(isSfen, Move, syuturyoku);
            syuturyoku.Append(" ");

            if (Ousyu == Move.Toryo)
            {
                syuturyoku.Append("none");// FIXME: toryo と none の区別に未対応
            }
            else
            {
                ConvMove.AppendFenTo(isSfen, Ousyu, syuturyoku);
            }

            syuturyoku.Append(" ");
            syuturyoku.Append(Version.ToString());
            syuturyoku.Append(" ");
            syuturyoku.Append(Kati.ToString());
            syuturyoku.Append(" ");
            syuturyoku.Append(Hikiwake.ToString());
            syuturyoku.Append(" ");
            syuturyoku.AppendLine(Make.ToString());
        }
    }

    /// <summary>
    /// 定跡の１局面だぜ☆（＾▽＾）
    /// </summary>
    public class SeisekiKyokumen
    {
        public SeisekiKyokumen(string fen, Option<Phase> optionalPhase, Seiseki owner)
        {
            this.Owner = owner;
            this.Fen = fen;
            this.TbTaikyokusya = OptionalPhase.ToTaikyokusya(optionalPhase);
            this.SsItems = new Dictionary<Move, SeisekiMove>();
        }

        public Seiseki Owner { get; private set; }
        /// <summary>
        /// 記録されている合法手一覧☆（＾▽＾）
        /// </summary>
        public Dictionary<Move, SeisekiMove> SsItems { get; private set; }

        /// <summary>
        /// 改造Fen
        /// 例： fen kr1/1h1/1H1/1R1 K2z 1
        /// </summary>
        public string Fen { get; private set; }
        public Taikyokusya TbTaikyokusya { get; private set; }

        public SeisekiMove AddSasite(Kyokumen ky, string sasiteRecordStr, StringBuilder syuturyoku)
        {
            int caret = 0;
            if (SeisekiMove.TryParse(ky, sasiteRecordStr, ref caret, out SeisekiMove josekiSs, this, syuturyoku))
            {

            }

            if (!this.SsItems.ContainsKey(josekiSs.Move))
            {
                this.SsItems.Add(josekiSs.Move, josekiSs);
                this.Owner.Edited = true;
            }
            else
            {
                josekiSs = this.SsItems[josekiSs.Move];
            }

            return josekiSs;
        }
        public SeisekiMove AddSasite(Move bestSasite, int version, int kati, int hikiwake, int make)
        {
            SeisekiMove seisekiSs = null;

            if (!this.SsItems.ContainsKey(bestSasite))
            {
                // 無ければ問答無用で追加☆（＾▽＾）
                seisekiSs = new SeisekiMove(bestSasite, Move.Toryo, version, kati, hikiwake, make, this);

                if (null != seisekiSs)
                {
                    this.SsItems.Add(bestSasite, seisekiSs);
                    this.Owner.Edited = true;
                }
            }
            else
            {
                // 既存なら、上書き（加算）☆
                seisekiSs = this.SsItems[bestSasite];
                seisekiSs.Version = version;

                seisekiSs.Kati += kati;
                seisekiSs.Hikiwake += hikiwake;
                seisekiSs.Make += make;
            }

            return seisekiSs;
        }

        /// <summary>
        /// 定跡ファイル
        /// </summary>
        /// <returns></returns>
        public void ToContents_NotUnity(bool isSfen, StringBuilder syuturyoku)
        {
            // 局面
            syuturyoku.AppendLine(this.Fen);

            // 指し手
            foreach (KeyValuePair<Move, SeisekiMove> entry2 in this.SsItems)
            {
                entry2.Value.ToContents_NotUnity(isSfen, syuturyoku);
            }
        }
    }

    /// <summary>
    /// 定跡ファイルだぜ☆（＾▽＾）
    /// 
    /// 出典
    /// やねうら王　「将棋ソフト用の標準定跡ファイルフォーマットの提案」
    /// http://yaneuraou.yaneu.com/2016/02/05/%E5%B0%86%E6%A3%8B%E3%82%BD%E3%83%95%E3%83%88%E7%94%A8%E3%81%AE%E6%A8%99%E6%BA%96%E5%AE%9A%E8%B7%A1%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%83%95%E3%82%A9%E3%83%BC%E3%83%9E%E3%83%83%E3%83%88%E3%81%AE/
    /// </summary>
    public class Seiseki
    {
        public Seiseki()
        {
            this.KyItems = new Dictionary<ulong, SeisekiKyokumen>();
        }

        /// <summary>
        /// 分割ファイルを全部マージしたと考えたときの目安の最大容量だぜ☆（＾～＾）
        /// ちょっとオーバーしたりするぜ☆（＾▽＾）
        /// 文字数換算だぜ☆（＾▽＾）
        /// </summary>
        public const int Capacity = 64 * 1000 * 1000;// 64 Mega ascii characters

        /// <summary>
        /// ハッシュを使うので、データが消えるかも……☆（＾～＾）
        /// </summary>
        public Dictionary<ulong, SeisekiKyokumen> KyItems { get; }

        public void Clear()
        {
            this.KyItems.Clear();
            this.Edited = true;
        }
        public bool Edited { get; set; }

        /// <summary>
        /// データを追加するぜ☆（＾▽＾） 指しながら定跡を追加していくときだぜ☆
        /// </summary>
        /// <param name="ky_before"></param>
        public SeisekiKyokumen AddMove(string kyFen_before, ulong kyHash_before, Option<Phase> optionalPhaseBeforeMove, Move bestSasite, int version, int kati, int hikiwake, int make)
        {
            SeisekiKyokumen josekiKy = this.Parse_AddKyLine(kyFen_before, kyHash_before, optionalPhaseBeforeMove);

            josekiKy.AddSasite(bestSasite, version, kati, hikiwake, make);
            return josekiKy;
        }
        /// <summary>
        /// データを追加するぜ☆（＾▽＾）
        /// </summary>
        /// <param name="fen_before">指す前の局面の改造fen</param>
        /// <param name="kyHash_before">指す前の局面のハッシュ</param>
        /// <param name="optionalPhaseBeforeMove">指す前の局面の手番</param>
        /// <returns></returns>
        public SeisekiKyokumen Parse_AddKyLine(string fen_before, ulong kyHash_before, Option<Phase> optionalPhaseBeforeMove)
        {
            SeisekiKyokumen josekiKy;
            if (this.KyItems.ContainsKey(kyHash_before))
            {
                // 既存☆
                josekiKy = this.KyItems[kyHash_before];
            }
            else
            {
                josekiKy = new SeisekiKyokumen(fen_before, optionalPhaseBeforeMove, this);
                this.KyItems.Add(kyHash_before, josekiKy);
                this.Edited = true;
            }

            return josekiKy;
        }

        public void Parse(bool isSfen, string[] lines, StringBuilder syuturyoku)
        {
            this.Clear();
            Kyokumen ky2 = new Kyokumen();
            int caret;
            SeisekiKyokumen josekiKy = null;

            int gyoBango = 1;
            foreach (string commandline in lines)
            {
                caret = 0;
                if (caret == commandline.IndexOf("fen ", caret))// fen で始まれば局面データ☆（＾▽＾）
                {
                    // キャレットは進めずに続行だぜ☆（＾▽＾）
                    if (!ky2.ParsePositionvalue(isSfen, commandline, ref caret, false, false, out string moves, syuturyoku))
                    {
                        string msg = $"パースに失敗だぜ☆（＾～＾）！ #寒鰤 定跡ファイル解析失敗 {gyoBango}]行目";
                        syuturyoku.AppendLine(msg);
                        Logger.Flush(syuturyoku.ToString());
                        syuturyoku.Clear();
                        throw new Exception(msg);
                    }

                    {
                        ky2.Tekiyo(false, syuturyoku); // とりあえず全部作り直し☆（＾～＾）ルールは変わらないものとするぜ☆（＾～＾）
                        //ky2.KyokumenHash = ky2.CreateKyokumenHash();//必要最低限、ハッシュだけ適用しておくぜ☆（＾▽＾）
                    }

                    josekiKy = this.Parse_AddKyLine(commandline, ky2.KyokumenHash.Value, OptionalPhase.From( ky2.Teban));
                }
                else if (commandline.Trim().Length < 1)
                {
                    // 空行は無視☆
                    // 半角空白とか、全角空白とか、タブとか　入れてるやつは考慮しないぜ☆（＾～＾）！
                }
                else
                {
                    // それ以外は手筋☆（＾▽＾）
                    if (null == josekiKy)
                    {
                        throw new Exception("定跡ファイル解析失敗 定跡局面の指定なし☆");
                    }

                    josekiKy.AddSasite(ky2, commandline, syuturyoku);
                }

                gyoBango++;
            }
        }

        /// <summary>
        /// 定跡局面の中で、勝率が一番高い指し手を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <returns>なければ投了☆</returns>
        public Move GetSasite_Winest(Kyokumen ky, out float out_bestSyoritu)
        {
            Move bestSasite = Move.Toryo;
            out_bestSyoritu = float.MinValue;
            int minMake = int.MaxValue;

            //Util_Machine.Assert_KyokumenSeigosei_SabunKosin("ゲット指し手 #鯨",true);
            ulong hash = ky.KyokumenHash.Value;
            if (this.KyItems.ContainsKey(hash))
            {
                SeisekiKyokumen josekyKy = this.KyItems[hash];
                foreach (KeyValuePair<Move, SeisekiMove> entry in josekyKy.SsItems)
                {
                    if (out_bestSyoritu < entry.Value.GetSyoritu())// 勝率が高い指し手を選ぶぜ☆（＾▽＾）
                    {
                        bestSasite = entry.Key;
                        out_bestSyoritu = entry.Value.GetSyoritu();
                        minMake = entry.Value.Make;
                    }
                    else if (out_bestSyoritu == entry.Value.GetSyoritu() &&//勝率が同じ場合は、
                        entry.Value.Make < minMake//負けが少ない指し手を選ぶぜ☆（＾▽＾）
                        )
                    {
                        bestSasite = entry.Key;
                        out_bestSyoritu = entry.Value.GetSyoritu();
                        minMake = entry.Value.Make;
                    }
                }
            }

            return bestSasite;
        }

        /// <summary>
        /// 定跡局面の中で、勝率が一番高い指し手を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <returns>指し手が登録されていれば真☆</returns>
        public bool GetSasite_Syoritu(Kyokumen ky, Move ss, out float out_syoritu)
        {
            out_syoritu = float.MinValue;

            ulong hash = ky.KyokumenHash.Value;
            if (this.KyItems.ContainsKey(hash))
            {
                SeisekiKyokumen josekyKy = this.KyItems[hash];
                if (josekyKy.SsItems.ContainsKey(ss))
                {
                    SeisekiMove seSs = josekyKy.SsItems[ss];
                    out_syoritu = seSs.GetSyoritu();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 定跡ファイルの容量を小さくしたいときに、定跡を削っていくぜ☆（＾～＾）
        /// </summary>
        /// <param name="removeBytes">減らしたいバイトサイズ☆（＾▽＾）</param>
        public long DownSizeing(long removeBytes)
        {
            long removed = 0;

            if (removeBytes < 1)
            {
                return removed;
            }

            // 削る優先順
            // （１）１つの局面の中で、２つ以上の指し手があり、勝率が一番悪い手、
            // （２）あとは泣く泣く適当に削る☆
            //
            // 最後に、指し手を持たない局面を削っておくぜ☆

            //────────────────────────────────────────
            // （１）１つの局面の中で、２つ以上の指し手があり、勝率が一番悪い手
            //────────────────────────────────────────
            foreach (SeisekiKyokumen josekiKy in this.KyItems.Values)
            {
                if (2 <= josekiKy.SsItems.Count)
                {
                    // 全ての手を走査し、一番勝率が悪いもの☆（＾▽＾）
                    float badest = float.MaxValue;
                    float goodest = float.MinValue;

                    foreach (SeisekiMove josekiSs in josekiKy.SsItems.Values)
                    {
                        if (josekiSs.GetSyoritu() < badest)
                        {
                            badest = josekiSs.GetSyoritu();
                        }
                        else if (goodest < josekiSs.GetSyoritu())
                        {
                            goodest = josekiSs.GetSyoritu();
                        }
                    }

                    if (badest == goodest || goodest < badest)
                    {
                        break;
                    }

                    // 評価が悪いキーを列挙☆（＾▽＾）
                    List<Move> removee = new List<Move>();
                    foreach (KeyValuePair<Move, SeisekiMove> entry in josekiKy.SsItems)
                    {
                        if (badest == entry.Value.GetSyoritu())
                        {
                            removee.Add(entry.Key);
                        }
                    }

                    // 列挙したキーに従って削除だぜ☆（＾▽＾）
                    foreach (Move key in removee)
                    {
                        int size = josekiKy.SsItems[key].ToString().Length;
                        josekiKy.SsItems.Remove(key);
                        removeBytes -= size;
                        removed += size;
                        if (removeBytes < 1)
                        {
                            goto gt_FinishRemove;
                        }
                    }
                }
            }

            //────────────────────────────────────────
            // （４）あとは泣く泣く適当に削る☆
            //────────────────────────────────────────
            {
                // 全部のキーを列挙☆（＾▽＾）
                List<ulong> removee = new List<ulong>();
                foreach (KeyValuePair<ulong, SeisekiKyokumen> entry in this.KyItems)
                {
                    removee.Add(entry.Key);
                }

                // 列挙したキーに従って削除だぜ☆（＾▽＾）
                foreach (ulong key in removee)
                {
                    int size = this.KyItems[key].ToString().Length;
                    this.KyItems.Remove(key);
                    removeBytes -= size;
                    removed += size;
                    if (removeBytes < 1)
                    {
                        goto gt_FinishRemove;
                    }
                }
            }

        gt_FinishRemove:
            //────────────────────────────────────────
            // （最後に）指し手を持たない局面を削る☆
            //────────────────────────────────────────
            {
                // 指し手を持たない局面のキーを列挙☆（＾▽＾）
                List<ulong> removee = new List<ulong>();
                foreach (KeyValuePair<ulong, SeisekiKyokumen> entry in this.KyItems)
                {
                    if (entry.Value.SsItems.Count < 1)
                    {
                        removee.Add(entry.Key);
                    }
                }

                // 列挙したキーに従って削除だぜ☆（＾▽＾）
                foreach (ulong key in removee)
                {
                    this.KyItems.Remove(key);
                }
            }

            if (0 < removed)
            {
                this.Edited = true;
            }
            return removed;
        }

        /// <summary>
        /// ファイルの容量が大きくなったので、分割するぜ☆（＾～＾）
        /// 低速にはなるが、たくさん記憶するためのものだぜ☆
        /// </summary>
        /// <returns>分けた残りの定跡</returns>
        public void Bunkatu(out Seiseki[] out_bunkatu, out string[] out_bunkatupartNames)
        {
            out_bunkatupartNames = new string[] { "(P1)", "(P2)" };
            Seiseki seP2 = new Seiseki();

            // 削除するキー
            List<ulong> removeKeys = new List<ulong>();
            foreach (KeyValuePair<ulong, SeisekiKyokumen> seKy in this.KyItems)
            {
                if (seKy.Value.TbTaikyokusya == Taikyokusya.T2)
                {
                    removeKeys.Add(seKy.Key);

                    foreach (KeyValuePair<Move, SeisekiMove> seSs in seKy.Value.SsItems)
                    {
                        seP2.AddMove(
                            seKy.Value.Fen,
                            seKy.Key,
                           OptionalPhase.From( seKy.Value.TbTaikyokusya),
                            seSs.Key,
                            seSs.Value.Version,
                            seSs.Value.Kati,
                            seSs.Value.Hikiwake,
                            seSs.Value.Make
                        );
                    }
                }
            }

            foreach (ulong key in removeKeys)
            {
                this.KyItems.Remove(key);
            }

            out_bunkatu = new Seiseki[] { this,//[0]はthisにしろだぜ☆（＾▽＾）
                seP2 };
        }

        /// <summary>
        /// 分けたファイルを吸収するぜ☆ｗｗｗ（＾▽＾）
        /// 重複したデータは、どちらを残すか自動的に判断するぜ☆（＾▽＾）
        /// </summary>
        public void Merge(Seiseki seiseki)
        {
            foreach (KeyValuePair<ulong, SeisekiKyokumen> seKy in seiseki.KyItems)
            {
                foreach (KeyValuePair<Move, SeisekiMove> seSs in seKy.Value.SsItems)
                {
                    this.AddMove(
                        seKy.Value.Fen,
                        seKy.Key,
                       OptionalPhase.From( seKy.Value.TbTaikyokusya),
                        seSs.Key,
                        seSs.Value.Version,
                        seSs.Value.Kati,
                        seSs.Value.Hikiwake,
                        seSs.Value.Make
                        );
                }
            }
        }

        /// <summary>
        /// 定跡ファイル
        /// </summary>
        /// <returns></returns>
        public string ToContents_NotUnity(bool isSfen)
        {
            StringBuilder mojiretu = new StringBuilder();

            foreach (KeyValuePair<ulong, SeisekiKyokumen> entry1 in this.KyItems)
            {
                entry1.Value.ToContents_NotUnity(isSfen, mojiretu);
            }

            return mojiretu.ToString();
        }
    }

    public abstract class Conv_Seiseki
    {
        public static void ResultToCount(Taikyokusya tb, TaikyokuKekka gameResult, out int kati, out int hikiwake, out int make)
        {
            switch (gameResult)
            {
                case TaikyokuKekka.Taikyokusya1NoKati:
                    switch (tb)
                    {
                        case Taikyokusya.T1: kati = 1; hikiwake = 0; make = 0; return;
                        case Taikyokusya.T2: kati = 0; hikiwake = 0; make = 1; return;
                        default: throw new Exception("未対応の手番");
                    }
                case TaikyokuKekka.Taikyokusya2NoKati:
                    switch (tb)
                    {
                        case Taikyokusya.T1: kati = 0; hikiwake = 0; make = 1; return;
                        case Taikyokusya.T2: kati = 1; hikiwake = 0; make = 0; return;
                        default: throw new Exception("未対応の手番");
                    }
                case TaikyokuKekka.Hikiwake://thru
                case TaikyokuKekka.Sennitite:
                    kati = 0; hikiwake = 1; make = 0;
                    break;
                case TaikyokuKekka.Karappo://thru
                default:
                    throw new Exception("未対応の対局結果");
            }
        }
    }
}

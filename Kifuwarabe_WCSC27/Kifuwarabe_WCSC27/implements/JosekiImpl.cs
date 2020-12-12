using kifuwarabe_wcsc27.facade;
using kifuwarabe_wcsc27.interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using kifuwarabe_wcsc27.machine;
using kifuwarabe_wcsc27.abstracts;

namespace kifuwarabe_wcsc27.implements
{
    public class JosekiSasite
    {
        public JosekiSasite(Move move, Move ousyu, Hyokati hyokati, int fukasa, int version, JosekiKyokumen owner)
        {
            this.Owner = owner;
            this.Move = move;
            this.Ousyu = ousyu;
            this.Hyokati = hyokati;
            this.Fukasa = fukasa;
            this.Version = version;
        }

        public JosekiKyokumen Owner { get; private set; }
        /// <summary>
        /// 指し手
        /// </summary>
        public Move Move { get; private set; }
        /// <summary>
        /// 応手
        /// </summary>
        public Move Ousyu { get; private set; }
        /// <summary>
        /// 評価値
        /// </summary>
        public Hyokati Hyokati { get { return this.m_hyokati_; } set { this.m_hyokati_ = value; this.Owner.Owner.Edited = true; } }
        Hyokati m_hyokati_;
        /// <summary>
        /// 深さ
        /// </summary>
        public int Fukasa { get; private set; }
        /// <summary>
        /// 定跡を登録したソフトのバージョン
        /// </summary>
        public int Version { get; private set; }

#if UNITY && !KAIHATU

#else
        /// <summary>
        /// 定跡ファイル
        /// </summary>
        /// <returns></returns>
        public void ToContentsLine_NotUnity(bool isSfen, Mojiretu syuturyoku)
        {
            ConvMove.AppendFenTo(isSfen, this.Move, syuturyoku);
            syuturyoku.Append(" ");

            if (this.Ousyu == Move.Toryo)
            {
                syuturyoku.Append("none");// FIXME: toryo と none の区別に未対応
            }
            else
            {
                ConvMove.AppendFenTo(isSfen, this.Ousyu, syuturyoku);
            }
            syuturyoku.Append(" ");
            syuturyoku.Append(((int)this.Hyokati).ToString());// enum型の変数名で出力されないように、int型に変換してから文字列にするぜ☆（＾▽＾）
            syuturyoku.Append(" ");
            syuturyoku.Append(this.Fukasa.ToString());
            syuturyoku.Append(" ");
            syuturyoku.AppendLine(this.Version.ToString());
        }
#endif
    }

    /// <summary>
    /// 定跡の１局面だぜ☆（＾▽＾）
    /// </summary>
    public class JosekiKyokumen
    {
        public JosekiKyokumen(string fen, Taikyokusya tb, Joseki owner)
        {
            this.Owner = owner;
            this.Fen = fen;
            this.TbTaikyokusya = tb;
            this.SsItems = new Dictionary<Move, JosekiSasite>();
        }

        public Joseki Owner { get; private set; }

        /// <summary>
        /// 記録されている合法手一覧☆（＾▽＾）
        /// </summary>
        public Dictionary<Move, JosekiSasite> SsItems { get; private set; }

        /// <summary>
        /// 指し手データを返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ss">指し手</param>
        /// <returns>無ければヌル☆</returns>
        public JosekiSasite GetSasite(Move ss)
        {
            if (this.SsItems.ContainsKey(ss))
            {
                return this.SsItems[ss];
            }
            return null;
        }

        /// <summary>
        /// 改造Fen
        /// 例： kr1/1h1/1H1/1R1 K2z 1
        /// </summary>
        public string Fen { get; private set; }
        public Taikyokusya TbTaikyokusya { get; private set; }

        public JosekiSasite AddSasite(Move bestSasite, Hyokati hyokati, int fukasa, int version)
        {
            JosekiSasite josekiSs;

            if (!this.SsItems.ContainsKey(bestSasite))
            {
                // 無ければ問答無用で追加☆（＾▽＾）
                josekiSs = new JosekiSasite(bestSasite, Move.Toryo, hyokati, fukasa, version, this);
                this.SsItems.Add(bestSasite, josekiSs);
                this.Owner.Edited = true;
            }
            else
            {
                // 既存なら
                josekiSs = this.SsItems[bestSasite];

                if (josekiSs.Version < version) // 新しいソフトの評価を優先☆
                {
                    josekiSs = new JosekiSasite(bestSasite, Move.Toryo, hyokati, fukasa, version, this);
                    this.SsItems[bestSasite] = josekiSs;
                    this.Owner.Edited = true;
                }
                // バージョンが同じなら
                else if (josekiSs.Fukasa < fukasa)// 深い探索の方を優先☆（＾▽＾）
                {
                    josekiSs = new JosekiSasite(bestSasite, Move.Toryo, hyokati, fukasa, version, this);
                    this.SsItems[bestSasite] = josekiSs;
                    this.Owner.Edited = true;
                }
                // 深さが同じなら
                // 無視☆
            }

            return josekiSs;
        }

#if UNITY && !KAIHATU

#else
        /// <summary>
        /// 定跡ファイル
        /// </summary>
        /// <returns></returns>
        public void ToContentsLine(bool isSfen, Mojiretu syuturyoku)
        {
            // 局面
            syuturyoku.AppendLine(this.Fen);

            // 指し手
            foreach (KeyValuePair<Move, JosekiSasite> entry2 in this.SsItems)
            {
                entry2.Value.ToContentsLine_NotUnity(isSfen, syuturyoku);
            }
        }
#endif
    }

    /// <summary>
    /// 定跡ファイルだぜ☆（＾▽＾）
    /// 
    /// 出典
    /// やねうら王　「将棋ソフト用の標準定跡ファイルフォーマットの提案」
    /// http://yaneuraou.yaneu.com/2016/02/05/%E5%B0%86%E6%A3%8B%E3%82%BD%E3%83%95%E3%83%88%E7%94%A8%E3%81%AE%E6%A8%99%E6%BA%96%E5%AE%9A%E8%B7%A1%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB%E3%83%95%E3%82%A9%E3%83%BC%E3%83%9E%E3%83%83%E3%83%88%E3%81%AE/
    /// </summary>
    public class Joseki
    {
        static Joseki()
        {
            Joseki.DanStrings = new string[4]; // 1段目～4段目,1筋～3筋
        }

        public Joseki()
        {
            this.KyItems = new Dictionary<ulong, JosekiKyokumen>();
        }

        /// <summary>
        /// 分割ファイルを全部マージしたと考えたときの目安の最大容量だぜ☆（＾～＾）
        /// ちょっとオーバーしたりするぜ☆（＾▽＾）
        /// 文字数換算だぜ☆（＾▽＾）
        /// </summary>
        public const int Capacity = 64 * 1000 * 1000;// 64 Mega ascii characters
        private static string[] DanStrings { get; set; }

        /// <summary>
        /// ハッシュを使うので、データが消えるかも……☆（＾～＾）
        /// </summary>
        public Dictionary<ulong,JosekiKyokumen> KyItems { get; }

        public void Clear()
        {
            this.KyItems.Clear();
            this.Edited = true;
        }
        /// <summary>
        /// 編集済みフラグ（未保存フラグ）だぜ☆（＾～＾）
        /// </summary>
        public bool Edited { get; set; }

        /// <summary>
        /// データを追加するぜ☆（＾▽＾） 指しながら定跡を追加していくときだぜ☆
        /// </summary>
        /// <param name="ky_before"></param>
        public JosekiKyokumen AddSasite(string kyFen_before, ulong kyHash_before, Taikyokusya kyTb_before, Move bestSasite, Hyokati hyokati, int fukasa, int version, Mojiretu syuturyoku)
        {
            JosekiKyokumen josekiKy = this.ParseKyokumenLine(kyFen_before, kyHash_before, kyTb_before, syuturyoku);

            //#if DEBUG
            //            //────────────────────────────────────────
            //            // データを追加する前に
            //            //────────────────────────────────────────
            //            //
            //            // 指し手の整合性をチェックして、不正なデータを弾くことは必要だぜ☆（＾▽＾）
            //            // 
            //            {
            //                Kyokumen ky2 = new KyokumenImpl();
            //                if(!ky2.ParseFen(kyFen_before, false))
            //                {
            //                    string msg = "パースに失敗だぜ☆（＾～＾）！";
            //                    Face_Application.MessageLine(msg);
            //                    Face_Application.Write();
            //                    throw new Exception(msg);
            //                }

            //                SasiteError reason;
            //                if (!ky2.CanDoSasite(bestSasite, out reason))
            //                {
            //                    throw new Exception("指せない指し手を定跡に登録しようとしたぜ☆（＾～＾）！："+ConvMove.Setumei(reason));
            //                }
            //            }

            //            // これから登録する指し手を、ログに書き出しておきたいぜ☆（＾▽＾）マージのとき、うるさい☆（＾～＾）
            //            {
            //                // 定跡登録
            //                //     fen .../.../.../ - 1 
            //                //     B1A1 .....
            //                Util_Machine.AppendLine("定跡登録");
            //                Util_Machine.AppendLine("    " + kyFen_before);
            //                Util_Machine.AppendLine("    " + ConvMove.ToFen(bestSasite));
            //            }
            //#endif

            josekiKy.AddSasite(bestSasite, hyokati, fukasa, version);

//#if DEBUG
//            // 定跡を追加した直後にダンプして中身を目視確認だぜ☆（＾～＾）
//            Util_Machine.AppendLine(
//                    "定跡を追加した直後にダンプして中身を目視確認だぜ☆（＾～＾）\n" +
//                    "┌──────────┐\n" +
//                    this.ToString() +
//                    "└──────────┘\n" +
//                    ""
//                );
//            Util_Machine.Flush();
//#endif

            return josekiKy;
        }
        /// <summary>
        /// 局面データだけを追加するぜ☆（＾▽＾）
        /// ファイルを行単位にパースしているときに使う☆（＾▽＾）
        /// </summary>
        /// <param name="kyFen_before">指す前の局面の改造fen</param>
        /// <param name="kyHash_before">指す前の局面のハッシュ</param>
        /// <param name="kyTb_before">指す前の局面の手番</param>
        /// <returns></returns>
        public JosekiKyokumen ParseKyokumenLine(string kyFen_before, ulong kyHash_before, Taikyokusya kyTb_before, Mojiretu syuturyoku)
        {
            JosekiKyokumen josekiKy;
            if (this.KyItems.ContainsKey(kyHash_before))
            {
                // 既存☆
                josekiKy = this.KyItems[kyHash_before];
            }
            else
            {
                // 新規☆
                /*
#if DEBUG
                {
                    Kyokumen ky2 = new KyokumenImpl();
                    int caret = 0;
                    ky2.ParseFen(kyFen_before, ref caret, false, syuturyoku);
                    ulong newHash = ky2.CreateKyokumenHash();
                    if (newHash != kyHash_before)
                    {
                        Mojiretu reigai = new MojiretuImpl();
                        reigai.Append("局面ハッシュが異なるぜ☆（＾～＾）！ kyFen_before=[");
                        reigai.Append(kyFen_before);
                        reigai.Append("] newHash=[");
                        reigai.Append(newHash.ToString());
                        reigai.Append("] kyHash_before=[");
                        reigai.Append(kyHash_before.ToString());
                        reigai.Append("]");
                        syuturyoku.AppendLine(reigai.ToContents());
                        Util_Machine.Flush(syuturyoku);
                        throw new Exception(reigai.ToContents());
                    }
                }
#endif
                */

                josekiKy = new JosekiKyokumen(kyFen_before, kyTb_before, this);
                this.KyItems.Add(kyHash_before, josekiKy);
                this.Edited = true;
            }

            return josekiKy;
        }

        /// <summary>
        /// 定跡ファイルの解析☆（＾～＾）
        /// </summary>
        /// <param name="lines"></param>
        public void Parse(bool isSfen, string[] lines, Mojiretu syuturyoku)
        {
            this.Clear();
            Kyokumen ky_forJoseki = new Kyokumen();//使いまわすぜ☆（＾▽＾）
            JosekiKyokumen josekiKy = null;
            JosekiSasite josekiSs;
            Match m;
            string commandline;
            for (int iGyoBango = 0; iGyoBango<lines.Length; iGyoBango++)
            {
                commandline = lines[iGyoBango];

                if (commandline.Length < 1)
                {
                    // 空行は無視☆
                    // 半角空白とか、全角空白とか、タブとか　入れてるやつは考慮しないぜ☆（＾～＾）！
                }
                else if ('f'==commandline[0])// fen で始まれば局面データ☆（＾▽＾）// caret == commandline.IndexOf("fen ", caret)
                {
                    // キャレットは進めずに続行だぜ☆（＾▽＾）
                    m = Itiran_FenParser.GetJosekiKyPattern(Option_Application.Optionlist.USI).Match(commandline);//, caret
                    if (!m.Success)
                    {
                        Mojiretu reigai1 = new MojiretuImpl();
                        reigai1.AppendLine("パースに失敗だぜ☆（＾～＾）！ #寿 定跡ファイル解析失敗");
                        reigai1.AppendLine("commandline=["+ commandline + "]");
#if DEBUG
                        reigai1.Append(" [");
                        reigai1.Append(iGyoBango.ToString());
                        reigai1.Append("]行目");
#endif
                        syuturyoku.AppendLine(reigai1.ToContents());
                        Util_Machine.Flush(syuturyoku);
                        throw new Exception(reigai1.ToContents());
                    }

                    // .Value は、該当しないときは空文字列か☆
                    // .Value は、該当しないときは空文字列か☆
                    if (Itiran_FenParser.STARTPOS_LABEL == m.Groups[1].Value)
                    {
                        DanStrings = Itiran_FenParser.GetStartpos(Option_Application.Optionlist.USI).Split('/');
                    }
                    else
                    {
                        DanStrings = m.Groups[1].Value.Split('/');   // N段目
                    }


                    ky_forJoseki.SetNaiyo(
                        isSfen,
                        true,//適用
                        false,
                        Joseki.DanStrings,  //1～N 段目
                        m.Groups[2].Value,
                        m.Groups[3].Value,  //手番
                        syuturyoku
                    );

                    /*
#if DEBUG
                {
                    Kyokumen ky3 = new KyokumenImpl();
                    int caret = 0;
                    ky3.ParseFen(commandline, ref caret, false, syuturyoku);
                    ulong newHash = ky3.CreateKyokumenHash();
                    if (newHash != ky2.KyokumenHash)
                    {
                        Mojiretu reigai1 = new MojiretuImpl();
                        reigai1.Append("局面ハッシュが異なるぜ☆（＾～＾）！ commandline=[");
                        reigai1.Append(commandline);
                        reigai1.Append("] ky3.AppendFenTo=[");
                        ky3.AppendFenTo(reigai1);
                        reigai1.Append("] dan1[");
                        reigai1.Append(JosekiImpl.DanStrings[0]);
                        reigai1.Append("] dan2[");
                        reigai1.Append(JosekiImpl.DanStrings[1]);
                        reigai1.Append("] dan3[");
                        reigai1.Append(JosekiImpl.DanStrings[2]);
                        reigai1.Append("] dan4[");
                        reigai1.Append(JosekiImpl.DanStrings[3]);
                        reigai1.Append("] newHash=[");
                        reigai1.Append(newHash.ToString());
                        reigai1.Append("] ky2.KyokumenHash=[");
                        reigai1.Append(ky2.KyokumenHash.ToString());
                        reigai1.Append("]");
                        syuturyoku.AppendLine(reigai1.ToContents());
                        Util_Machine.Flush(syuturyoku);
                        throw new Exception(reigai1.ToContents());
                    }
                }
#endif
                    */

                    josekiKy = this.ParseKyokumenLine(commandline,ky_forJoseki.KyokumenHash.Value, ky_forJoseki.Teban, syuturyoku);
                }
                else
                {
                    // それ以外は手筋☆（＾▽＾）
                    if (null== josekiKy)
                    {
                        throw new Exception("定跡ファイル解析失敗 定跡局面の指定なし☆");
                    }

                    // 指し手、指し手、数字、数字、数字　と並んでいるぜ☆（＾▽＾）
                    m = Itiran_FenParser.GetJosekiSsPattern(Option_Application.Optionlist.USI).Match(commandline);
                    if (!m.Success)
                    {
                        //*
                        // FIXME:
                        string msg = "パースに失敗だぜ☆（＾～＾）！ #鮪 commandline=[" + commandline + "]";
                        syuturyoku.AppendLine(msg);
                        Util_Machine.Flush(syuturyoku);
                        throw new Exception(msg);
                        // */
                    }

                    // 高速化のために、ローカル変数を減らして、詰め込んだコードにしているぜ☆（＞＿＜）            

                    // 第１引数 B1C1 や toryo のような指し手の解析。
                    josekiSs = new JosekiSasite(
                        // １列目：指し手☆ (1:グループ,2:指し手全体,3～7:指し手各部,8:投了)
                        m.Groups[8].Success ? Move.Toryo : // "toryo" が入っている場合☆
                            Med_Parser.TryFen_Sasite2(Option_Application.Optionlist.USI,
                                ky_forJoseki.Sindan,
                                m.Groups[3].Value,
                                m.Groups[4].Value,
                                m.Groups[5].Value,
                                m.Groups[6].Value,
                                m.Groups[7].Value
                            ),

                        // ２列目：応手☆ none とか。(9:グループ,10:none,11:指し手全体,12～16:指し手各部,17:投了)
                        m.Groups[10].Success || m.Groups[17].Success ? Move.Toryo :// [10]"none" または[17]"toryo" が入っている場合☆ FIXME: none と toryo を区別してないぜ☆（＾～＾）
                        Med_Parser.TryFen_Sasite2(Option_Application.Optionlist.USI,
                            ky_forJoseki.Sindan,
                            m.Groups[12].Value,
                            m.Groups[13].Value,
                            m.Groups[14].Value,
                            m.Groups[15].Value,
                            m.Groups[16].Value
                        ),

                        (Hyokati)int.Parse(m.Groups[18].Value),//hyokati (18) 解析はokなはず☆
                        int.Parse(m.Groups[19].Value),//fukasa (19) 解析はokなはず☆
                        int.Parse(m.Groups[20].Value),//version (20) 解析はokなはず☆（旧版ではバージョンは無いこともある☆）
                        josekiKy
                        );


                    // 定跡ファイルの局面には、重複指し手データがないようにしてくれだぜ☆（＾～＾）チェックは省くぜ☆
                    /*
                    if (josekiKy.SsItems.ContainsKey(josekiSs.Move))
                    {
                            // FIXME:
                            String2 str = new String2Impl();
                            str.Append("局面データに重複の指し手があるぜ☆（＾～＾）！ 局面=[");
                            str.Append(josekiKy.Fen);
                            str.Append("] 指し手=[");
                            ConvMove.Setumei(josekiSs.Move,str);
                            str.Append("]");
                            Util_Machine.AppendLine(str.ToContents());
                            Util_Machine.Flush();
                            throw new Exception(str.ToContents());
                    }
                    else
                    {
                    // */
                    // 新規

                    //#if DEBUG
                    //                // 指し手の整合性をチェックしておきたいぜ☆（＾▽＾）
                    //                {
                    //                    Kyokumen ky2 = new KyokumenImpl();
                    //                    if (!ky2.ParseFen(ky.ToFen(), false))
                    //                    {
                    //                        string msg = "新規：　パースに失敗だぜ☆（＾～＾）！";
                    //                        Face_Application.MessageLine(msg);
                    //                        Face_Application.Write();
                    //                        throw new Exception(msg);
                    //                    }

                    //                    Move bestSasite;
                    //                    int caret_test = 0;
                    //                    ConvMove.TryParse(commandline, ref caret_test, ky2, out bestSasite);

                    //                    SasiteError reason;
                    //                    if (!ky2.CanDoSasite( bestSasite, out reason))
                    //                    {
                    //                        throw new Exception("新規：　指せない指し手を定跡に登録しようとしたぜ☆（＾～＾）！：" + ConvMove.Setumei(reason));
                    //                    }
                    //                }
                    //#endif

                    josekiKy.SsItems.Add(josekiSs.Move, josekiSs);
                    this.Edited = true;
                    //}
                }
            }
        }

        /// <summary>
        /// 定跡局面の中で、評価値が一番高い指し手を返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ky"></param>
        /// <returns>なければ投了☆</returns>
        public Move GetSasite(bool isSfen, Kyokumen ky, out Hyokati out_bestHyokati, Mojiretu syuturyoku
#if DEBUG
            , out string fen_forTest
#endif
            )
        {
            Move bestSasite = Move.Toryo;
            out_bestHyokati = Hyokati.TumeTesu_GohosyuNasi;
            int bestFukasa = 0;
#if DEBUG
            fen_forTest = "";
#endif

            //Util_Machine.Assert_KyokumenSeigosei_SabunKosin("ゲット指し手 #鯨",true);
            ulong hash = ky.KyokumenHash.Value;
            if (this.KyItems.ContainsKey(hash))
            {
                JosekiKyokumen josekyKy = this.KyItems[hash];
                // 整合性の確認用だぜ☆（＾～＾）
#if DEBUG
                fen_forTest = josekyKy.Fen;
#endif
                foreach (KeyValuePair<Move, JosekiSasite> entry in josekyKy.SsItems)
                {
                    if (out_bestHyokati < entry.Value.Hyokati)// 評価値が高い指し手を選ぶぜ☆（＾▽＾）
                    {
                        bestSasite = entry.Key;
                        out_bestHyokati = entry.Value.Hyokati;
                        bestFukasa = entry.Value.Fukasa;
                    }
                    else if (out_bestHyokati == entry.Value.Hyokati &&//評価値が同じ場合は、
                        bestFukasa < entry.Value.Fukasa//深く読んでいる指し手を選ぶぜ☆（＾▽＾）
                        )
                    {
                        bestSasite = entry.Key;
                        out_bestHyokati = entry.Value.Hyokati;
                        bestFukasa = entry.Value.Fukasa;
                    }
                }
            }

#if DEBUG
            // 指し手の整合性をチェックしておきたいぜ☆（＾▽＾）
            {
                Kyokumen ky_forAssert = new Kyokumen();
                int caret = 0;
                Mojiretu sindan1 = new MojiretuImpl();
                ky.AppendFenTo(Option_Application.Optionlist.USI, sindan1);
                //if (!ky2.ParseFen(sindan1.ToContents(), ref caret, false, syuturyoku))
                if (!ky_forAssert.ParsePositionvalue(isSfen, sindan1.ToContents(),ref caret, true, false, out string moves, syuturyoku))// ビットボードを更新したいので、適用する
                {
                    string msg = "取得：　パースに失敗だぜ☆（＾～＾）！ #鰯";
                    syuturyoku.AppendLine(msg);
                    Util_Machine.Flush(syuturyoku);
                    throw new Exception(msg);
                }

                if (!ky_forAssert.CanDoSasite(bestSasite, out SasiteMatigaiRiyu riyu))
                {
                    Mojiretu sindan2 = new MojiretuImpl();
                    sindan2.Append("取得：　指せない指し手を定跡から取り出そうとしたぜ☆（＾～＾）！：");
                    sindan2.Append("理由:"); ConvMove.SetumeiLine(riyu,sindan2);
                    sindan2.Append("指し手:"); ConvMove.SetumeiLine(isSfen, bestSasite, sindan2);
                    sindan2.Append("定跡局面　（"); ky_forAssert.AppendFenTo(Option_Application.Optionlist.USI, sindan2); sindan2.AppendLine("）");
                    Util_Information.Setumei_Lines_Kyokumen(ky_forAssert, sindan2);

                    //str2.AppendLine("以下、定跡メモリのダンプ");
                    //str2.AppendLine("┌──────────┐");
                    //str2.Append(this.ToString());
                    //str2.AppendLine("└──────────┘");

                    syuturyoku.AppendLine(sindan2.ToContents());
                    Util_Machine.Flush(syuturyoku);
                    throw new Exception(sindan2.ToContents());
                }
            }
#endif

            return bestSasite;
        }

        /// <summary>
        /// 指定の局面に対応するデータを返すぜ☆（＾▽＾）
        /// </summary>
        /// <param name="kyHash">局面のハッシュ☆</param>
        /// <returns>データ☆ なければヌル☆</returns>
        public JosekiKyokumen GetKyokumen(ulong kyHash)
        {
            if (this.KyItems.ContainsKey(kyHash))
            {
                return this.KyItems[kyHash];
            }
            return null;
        }

        /// <summary>
        /// 定跡に登録されている指し手一覧。
        /// </summary>
        /// <param name="ky"></param>
        /// <returns></returns>
        public List<Move> GetSasites(Kyokumen ky)
        {
            List<Move> sasites = new List<Move>();

            ulong hash = ky.KyokumenHash.Value;
            if (this.KyItems.ContainsKey(hash))
            {
                JosekiKyokumen josekyKy = this.KyItems[hash];

                foreach (KeyValuePair<Move, JosekiSasite> entry in josekyKy.SsItems)
                {
                    sasites.Add(entry.Key);
                }
            }
            return sasites;
        }

        /// <summary>
        /// 情報
        /// </summary>
        public void Joho(out int out_kyokumenSu, out int out_sasiteSu)
        {
            out_kyokumenSu = 0;
            out_sasiteSu = 0;
            foreach (KeyValuePair<ulong, JosekiKyokumen> entryKy in this.KyItems)
            {
                out_kyokumenSu++;
                foreach (KeyValuePair<Move, JosekiSasite> entrySs in entryKy.Value.SsItems)
                {
                    out_sasiteSu++;
                }
            }
        }

#if !UNITY
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
            // （１）バージョンが古い順、
            // （２）深さが浅い順、
            // （３）１つの局面の中で、２つ以上の指し手があり、評価値が一番悪い手、
            // （４）あとは泣く泣く適当に削る☆
            //
            // 最後に、指し手を持たない局面を削っておくぜ☆

            //────────────────────────────────────────
            // （１）バージョンが古い順
            //────────────────────────────────────────
            while (true)
            {
                // 全ての手を走査し、一番バージョン番号が古いもの☆（＾▽＾）
                int oldest = int.MaxValue;
                int newest = int.MinValue;
                foreach (JosekiKyokumen josekiKy in this.KyItems.Values)
                {
                    foreach (JosekiSasite josekiSs in josekiKy.SsItems.Values)
                    {
                        if (josekiSs.Version < oldest)
                        {
                            oldest = josekiSs.Version;
                        }
                        else if (newest < josekiSs.Version)
                        {
                            newest = josekiSs.Version;
                        }
                    }
                }

                if (oldest == newest || newest < oldest)
                {
                    break;
                }

                foreach (JosekiKyokumen josekiKy in this.KyItems.Values)
                {
                    // バージョン番号が古いキーを列挙☆（＾▽＾）
                    List<Move> removee = new List<Move>();
                    foreach (KeyValuePair<Move, JosekiSasite> entry in josekiKy.SsItems)
                    {
                        if (oldest == entry.Value.Version)
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
            // （２）深さが浅い順、
            //────────────────────────────────────────
            while (true)
            {
                // 全ての手を走査し、一番深さが浅いもの☆（＾▽＾）
                int shallowest = int.MaxValue;
                int deepest = int.MinValue;
                foreach (JosekiKyokumen josekiKy in this.KyItems.Values)
                {
                    foreach (JosekiSasite josekiSs in josekiKy.SsItems.Values)
                    {
                        if (josekiSs.Fukasa < shallowest)
                        {
                            shallowest = josekiSs.Fukasa;
                        }
                        else if (deepest < josekiSs.Fukasa)
                        {
                            deepest = josekiSs.Fukasa;
                        }
                    }
                }

                if (shallowest == deepest || deepest < shallowest)
                {
                    break;
                }

                // 深さが該当する手は消すぜ☆（＾▽＾）
                foreach (JosekiKyokumen josekiKy in this.KyItems.Values)
                {
                    // 浅いキーを列挙☆（＾▽＾）
                    List<Move> removee = new List<Move>();
                    foreach (KeyValuePair<Move, JosekiSasite> entry in josekiKy.SsItems)
                    {
                        if (shallowest == entry.Value.Fukasa)
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
            // （３）１つの局面の中で、２つ以上の指し手があり、評価値が一番悪い手
            //────────────────────────────────────────
            foreach (JosekiKyokumen josekiKy in this.KyItems.Values)
            {
                if (2 <= josekiKy.SsItems.Count)
                {
                    // 全ての手を走査し、一番評価が悪いもの☆（＾▽＾）
                    Hyokati badest = Hyokati.TumeTesu_SeiNoSu_ReiTeDume;
                    Hyokati goodest = Hyokati.TumeTesu_FuNoSu_ReiTeTumerare;

                    foreach (JosekiSasite josekiSs in josekiKy.SsItems.Values)
                    {
                        if (josekiSs.Hyokati < badest)
                        {
                            badest = josekiSs.Hyokati;
                        }
                        else if (goodest < josekiSs.Hyokati)
                        {
                            goodest = josekiSs.Hyokati;
                        }
                    }

                    if (badest == goodest || goodest < badest)
                    {
                        break;
                    }

                    // 評価が悪いキーを列挙☆（＾▽＾）
                    List<Move> removee = new List<Move>();
                    foreach (KeyValuePair<Move, JosekiSasite> entry in josekiKy.SsItems)
                    {
                        if (badest == entry.Value.Hyokati)
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
                foreach (KeyValuePair<ulong, JosekiKyokumen> entry in this.KyItems)
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
                foreach (KeyValuePair<ulong, JosekiKyokumen> entry in this.KyItems)
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
        public void Bunkatu(out Joseki[] out_bunkatu, out string[] out_bunkatupartNames, Mojiretu syuturyoku)
        {
            out_bunkatupartNames = new string[] { "(P1)", "(P2)" };
            Joseki joP2 = new Joseki();

            // 削除するキー
            List<ulong> removeKeys = new List<ulong>();
            foreach (KeyValuePair<ulong, JosekiKyokumen> joKy in this.KyItems)
            {
                if (joKy.Value.TbTaikyokusya==Taikyokusya.T2)
                {
                    removeKeys.Add(joKy.Key);

                    foreach (KeyValuePair<Move, JosekiSasite> joSs in joKy.Value.SsItems)
                    {
                        joP2.AddSasite(
                            joKy.Value.Fen,
                            joKy.Key,
                            joKy.Value.TbTaikyokusya,
                            joSs.Key,
                            joSs.Value.Hyokati,
                            joSs.Value.Fukasa,
                            joSs.Value.Version,
                            syuturyoku
                        );
                    }
                }
            }

            foreach (ulong key in removeKeys)
            {
                this.KyItems.Remove(key);
            }

            out_bunkatu = new Joseki[] { this,//[0]はthisにしろだぜ☆（＾▽＾）
                joP2 } ;
        }

        /// <summary>
        /// 分けたファイルを吸収するぜ☆ｗｗｗ（＾▽＾）
        /// 重複したデータは、どちらを残すか自動的に判断するぜ☆（＾▽＾）
        /// </summary>
        public void Merge(Joseki joseki, Mojiretu syuturyoku)
        {
            foreach (KeyValuePair<ulong, JosekiKyokumen> joKy in joseki.KyItems)
            {
                foreach (KeyValuePair<Move, JosekiSasite> joSs in joKy.Value.SsItems)
                {
                    this.AddSasite(
                        joKy.Value.Fen,
                        joKy.Key,
                        joKy.Value.TbTaikyokusya,
                        joSs.Key,
                        joSs.Value.Hyokati,
                        joSs.Value.Fukasa,
                        joSs.Value.Version,
                        syuturyoku
                        );
                }
            }
        }

        /// <summary>
        /// 定跡ファイル
        /// </summary>
        /// <returns></returns>
        public string ToContents(bool isSfen )
        {
            Mojiretu mojiretu1 = new MojiretuImpl();

            foreach (KeyValuePair<ulong, JosekiKyokumen> entry1 in this.KyItems)
            {
                entry1.Value.ToContentsLine(isSfen, mojiretu1);
            }

            return mojiretu1.ToContents();
        }
#endif
    }
}

using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using System;
using System.Diagnostics;
using System.Text;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 何かと情報を出力するのに使うぜ☆（＾～＾）
    /// </summary>
    public abstract class Util_Information
    {
        /// <summary>
        /// 全角しか使っていないと想定して、表示盤の横幅サイズに切り抜く。
        /// 右の空いたところには全角空白１個分の半角空白を埋める。
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string FormatBanWidthZenkaku(string header)
        {
            // 盤の横幅は「┌」「──┬」「──┐」の数。
            // 全角で3。
            int banZenkakuWidth = Option_Application.Optionlist.BanYokoHaba * 3 + 1;
            StringBuilder sb = new StringBuilder();
            if (banZenkakuWidth == header.Length)
            {
                sb.Append(header.Substring(0, banZenkakuWidth));
            }
            else if (banZenkakuWidth < header.Length)
            {
                sb.Append(header.Substring(0, banZenkakuWidth - 1));
                sb.Append("…");
            }
            else
            {
                sb.Append(header);
                for (int i = banZenkakuWidth - header.Length; 0 < i; i--)
                {
                    sb.Append("  ");
                }
            }
            return sb.ToString();
        }
        public static void AppendLine_Middle(int banSu, StringBuilder syuturyoku)
        {
            for (int ban = 0; ban < banSu; ban++)
            {
                syuturyoku.Append("├");
                for (int j = 0; j < Option_Application.Optionlist.BanYokoHaba; j++)
                {
                    if (j + 1 < Option_Application.Optionlist.BanYokoHaba)
                    {
                        syuturyoku.Append("──┼");
                    }
                    else
                    {
                        syuturyoku.Append("──┤");
                    }
                }
            }
            syuturyoku.AppendLine();
        }
        public static void AppendLine_Data(Bitboard[] bbHairetu, int ms_hidariHasi, StringBuilder syuturyoku)
        {
            for (int iBb = 0; iBb < bbHairetu.Length; iBb++)
            {
                Bitboard bb = bbHairetu[iBb];

                syuturyoku.Append("│");
                for (int i = 0; i < Option_Application.Optionlist.BanYokoHaba; i++)
                {
                    syuturyoku.Append(bb.IsOn((Masu)(ms_hidariHasi + i)) ? " 〇 " : "　　");
                    syuturyoku.Append("│");
                }
            }
            syuturyoku.AppendLine();
        }
        public static void AppendLine_Bottom(int banSu, StringBuilder syuturyoku)
        {
            for (int ban = 0; ban < banSu; ban++)
            {
                syuturyoku.Append("└");
                for (int j = 0; j < Option_Application.Optionlist.BanYokoHaba; j++)
                {
                    if (j + 1 < Option_Application.Optionlist.BanYokoHaba)
                    {
                        syuturyoku.Append("──┴");
                    }
                    else
                    {
                        syuturyoku.Append("──┘");
                    }
                }
            }
            syuturyoku.AppendLine();
        }

        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// ログに向いた、シンプルな表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_1Bitboard(string header, Bitboard bitboard_tb1, StringBuilder syuturyoku)
        {
            Debug.Assert(null != bitboard_tb1, "");

            Setumei_Bitboards(new string[] { header }, new Bitboard[] { bitboard_tb1 }, syuturyoku);
        }

        /// <summary>
        /// 将棋盤の見出しをコンソールへ出力するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Headers(string[] headers, StringBuilder syuturyoku)
        {
            foreach (string header in headers)
            {
                syuturyoku.Append(FormatBanWidthZenkaku(header));
            }
            syuturyoku.AppendLine();
        }

        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// 見出し有り
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Bitboards(string[] headers, Bitboard[] bbHairetu, StringBuilder syuturyoku)
        {
            Debug.Assert(0 < bbHairetu.Length && null != bbHairetu[0], "");

            // 見出し
            Setumei_Headers(headers, syuturyoku);

            // 盤上
            Util_Information.AppendLine_Top_Kyokumen(bbHairetu.Length, syuturyoku);
            for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
            {
                AppendLine_Data(bbHairetu, dan * Option_Application.Optionlist.BanYokoHaba, syuturyoku);

                if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                {
                    AppendLine_Middle(bbHairetu.Length, syuturyoku);
                }
            }
            AppendLine_Bottom(bbHairetu.Length, syuturyoku);
        }

        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// 見出し無し☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Bitboards(Bitboard[] bbHairetu, StringBuilder syuturyoku)
        {
            Util_Information.AppendLine_Top_Kyokumen(bbHairetu.Length, syuturyoku);
            for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
            {
                AppendLine_Data(bbHairetu, dan * Option_Application.Optionlist.BanYokoHaba, syuturyoku);

                if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                {
                    AppendLine_Middle(bbHairetu.Length, syuturyoku);
                }
            }
            AppendLine_Bottom(bbHairetu.Length, syuturyoku);
        }


        public static void AppendLine_Top_Kyokumen(int banSu, StringBuilder syuturyoku)
        {
            for (int ban = 0; ban < banSu; ban++)
            {
                syuturyoku.Append("┌");
                for (int j = 0; j < Option_Application.Optionlist.BanYokoHaba; j++)
                {
                    if (j + 1 < Option_Application.Optionlist.BanYokoHaba)
                    {
                        syuturyoku.Append("──┬");
                    }
                    else
                    {
                        syuturyoku.Append("──┐");
                    }
                }
            }
            syuturyoku.AppendLine();
        }

        public static void AppendLine_SujiFugo_Kyokumen(StringBuilder syuturyoku)
        {
            syuturyoku.Append("   ");
            for (int iSuji = 0; iSuji < Option_Application.Optionlist.BanYokoHaba; iSuji++)
            {
                syuturyoku.Append(Conv_Kihon.ZenkakuAlphabet[iSuji % Option_Application.Optionlist.BanYokoHaba]);
                if (iSuji + 1 < Option_Application.Optionlist.BanYokoHaba)
                {
                    syuturyoku.Append("    ");
                }
            }
            syuturyoku.AppendLine("    ");
        }
        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// ログに向いた、シンプルな表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_Lines_Kyokumen(Kyokumen ky, StringBuilder syuturyoku)
        {
            // 1行目
            {
                // 千日手
                int sennitite = ky.Konoteme.GetSennititeCount();
                if (Const_Game.SENNITITE_COUNT == sennitite)
                {
                    Conv_Taikyokusya.Setumei_Name(OptionalPhase.ToTaikyokusya( Conv_Taikyokusya.Reverse(OptionalPhase.From( ky.Teban))), syuturyoku);
                    syuturyoku.AppendLine("の着手にて　千日手");
                }
                else if (1 < sennitite)
                {
                    syuturyoku.Append("同一局面反復 ");
                    syuturyoku.Append(sennitite.ToString());
                    syuturyoku.AppendLine(" 回目");
                }
            }

            // 2行目
            {
                // 何手目
                syuturyoku.Append("図は");
                syuturyoku.Append(string.Format("{0,3}", ky.Konoteme.ScanNantemadeBango()));
                syuturyoku.Append("手まで ");

                // 手番
                Conv_Taikyokusya.Setumei_Name(ky.Teban, syuturyoku);
                syuturyoku.Append("の番");

                // #仲ルール
                if (Option_Application.Optionlist.SagareruHiyoko)
                {
                    syuturyoku.Append(" #仲");
                }

                syuturyoku.AppendLine();
            }

            // 3行目 後手の持ち駒
            {
                // 後手の持ち駒の数
                foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.Itiran)
                {
                    MotiKoma mk = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks, OptionalPhase.White);
                    if (ky.MotiKomas.HasMotiKoma(mk))
                    {
                        syuturyoku.Append(Conv_MotiKomasyurui.GetHyojiName(mks)); syuturyoku.Append(ky.MotiKomas.Get(mk).ToString());
                    }
                }
                syuturyoku.AppendLine();
            }

            // 4行目
            {
                // Ａ　Ｂ　Ｃ　Ｄ　とか
                syuturyoku.Append("  ");
                AppendLine_SujiFugo_Kyokumen(syuturyoku);
            }

            // 盤上
            {
                syuturyoku.Append("  ");
                Util_Information.AppendLine_Top_Kyokumen(1, syuturyoku);// ┌──┬──┬──┐

                for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
                {
                    syuturyoku.Append(Conv_Kihon.ToZenkakuInteger(dan + 1));
                    Util_Information.AppendLine_Data_Kyokumen(ky, dan, syuturyoku);

                    if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                    {
                        syuturyoku.Append("  ");
                        AppendLine_Middle(1, syuturyoku);//├──┼──┼──┤
                    }
                }

                syuturyoku.Append("  ");
                AppendLine_Bottom(1, syuturyoku);//└──┴──┴──┘
            }

            // 先手の持ち駒の数
            {
                foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.Itiran)
                {
                    MotiKoma mk = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks, OptionalPhase.Black);
                    if (ky.MotiKomas.HasMotiKoma(mk))
                    {
                        syuturyoku.Append(Conv_MotiKomasyurui.GetHyojiName(mks)); syuturyoku.Append(ky.MotiKomas.Get(mk).ToString());
                    }
                }
                syuturyoku.AppendLine();
            }
        }

        //private static Option<T> Option<T>(T white)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 将棋盤をコンソールへ出力するぜ☆（＾▽＾）
        /// コンソールでゲームするのに向いた表示☆
        /// </summary>
        /// <returns></returns>
        public static void Setumei_NingenGameYo(Kyokumen ky, StringBuilder syuturyoku)
        {
            // 1行目
            {
                // 千日手
                int sennitite = ky.Konoteme.GetSennititeCount();
                if (Const_Game.SENNITITE_COUNT == sennitite)
                {
                    Conv_Taikyokusya.Setumei_Name(OptionalPhase.ToTaikyokusya( Conv_Taikyokusya.Reverse(OptionalPhase.From( ky.Teban))), syuturyoku);
                    syuturyoku.Append("の着手にて　千日手");
                    syuturyoku.AppendLine();
                }
                else if (1 < sennitite)
                {
                    syuturyoku.Append("同一局面反復 ");
                    syuturyoku.Append(sennitite.ToString());
                    syuturyoku.AppendLine(" 回目");
                }
                else
                {
                    syuturyoku.AppendLine();
                }
            }

            // 2行目
            {
                // 何手目
                syuturyoku.Append("図は");
                syuturyoku.Append(string.Format("{0,3}", ky.Konoteme.ScanNantemadeBango()));
                syuturyoku.Append("手まで ");

                // 手番
                Conv_Taikyokusya.Setumei_Name(ky.Teban, syuturyoku);
                syuturyoku.Append("の番");

                // #仲ルール
                if (Option_Application.Optionlist.SagareruHiyoko)
                {
                    syuturyoku.Append(" #仲");
                }

                syuturyoku.AppendLine();
            }

            // 3行目 後手の持ち駒の数
            {
                foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.Itiran)
                {
                    MotiKoma mk = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks, OptionalPhase.White);
                    if (ky.MotiKomas.HasMotiKoma(mk))
                    {
                        syuturyoku.Append(Conv_MotiKomasyurui.GetHyojiName(mks)); syuturyoku.Append(ky.MotiKomas.Get(mk).ToString());
                    }
                }
                syuturyoku.AppendLine();
            }

            // 4行目
            {
                syuturyoku.Append("  ");
                AppendLine_SujiFugo_Kyokumen(syuturyoku);
                //syuturyoku.AppendLine("　　 Ａ 　 Ｂ 　 Ｃ 　");
            }

            // 5行目～13行目
            // 盤上
            {
                // 5行目
                syuturyoku.Append("  ");
                Util_Information.AppendLine_Top_Kyokumen(1, syuturyoku); // ┌──┬──┬──┐

                for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
                {
                    // 6,8,10,12行目
                    syuturyoku.Append(Conv_Kihon.ToZenkakuInteger(dan + 1));
                    AppendLine_Data_Kyokumen(ky, dan, syuturyoku);

                    if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                    {
                        // 7,9,11行目
                        syuturyoku.Append("  ");
                        AppendLine_Middle(1, syuturyoku);//├──┼──┼──┤
                    }
                }

                // 13行目
                syuturyoku.Append("  ");
                AppendLine_Bottom(1, syuturyoku);//└──┴──┴──┘
            }

            // 14行目
            {
                // 先手の持ち駒の数
                foreach (MotiKomasyurui mks in Conv_MotiKomasyurui.Itiran)
                {
                    MotiKoma mk = Med_Koma.MotiKomasyuruiAndPhaseToMotiKoma(mks, OptionalPhase.Black);
                    if (ky.MotiKomas.HasMotiKoma(mk))
                    {
                        syuturyoku.Append(Conv_MotiKomasyurui.GetHyojiName(mks)); syuturyoku.Append(ky.MotiKomas.Get(mk).ToString());
                    }
                }
                syuturyoku.AppendLine();
            }
        }
        public static void AppendLine_Data_Kyokumen(Kyokumen ky, int dan, StringBuilder syuturyoku)
        {
            syuturyoku.Append("│");
            for (int iMs_offset = 0; iMs_offset < Option_Application.Optionlist.BanYokoHaba; iMs_offset++)
            {
                Masu ms = (Masu)(dan * Option_Application.Optionlist.BanYokoHaba + iMs_offset);
                Koma km = ky.GetBanjoKoma(ms);
                Conv_Koma.Setumei(km, syuturyoku);
                syuturyoku.Append("│");
            }
            syuturyoku.AppendLine();
        }

        public static void AppendLine_Data_Countboard(Shogiban sg, int ms_hidariHasi, StringBuilder syuturyoku)
        {
            for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
            {
                syuturyoku.Append("│");
                for (int iMs_offset = 0; iMs_offset < Option_Application.Optionlist.BanYokoHaba; iMs_offset++)
                {
                    int kikisuZenbu = sg.CountKikisuZenbu(OptionalPhase.From( iTai), (Masu)(ms_hidariHasi + iMs_offset));
                    syuturyoku.Append(0 < kikisuZenbu ? string.Format(" {0,2} ", kikisuZenbu) : "　　");
                    syuturyoku.Append("│");
                }
            }
            syuturyoku.AppendLine();
        }
        public static void AppendLine_Data_Countboard(Option<Phase> optionalPhase, Shogiban sg, int ms_hidariHasi, StringBuilder syuturyoku)
        {
            for (int iKs = 0; iKs < Conv_Komasyurui.Itiran.Length; iKs++)
            {
                syuturyoku.Append("│");
                for (int iMs_offset = 0; iMs_offset < Option_Application.Optionlist.BanYokoHaba; iMs_offset++)
                {
                    int kikisuKomabetu = sg.CountKikisuKomabetu(Med_Koma.KomasyuruiAndTaikyokusyaToKoma((Komasyurui)iKs, optionalPhase), (Masu)(ms_hidariHasi + iMs_offset));
                    syuturyoku.Append(0 < kikisuKomabetu ? string.Format(" {0,2} ", kikisuKomabetu) : "　　");
                    syuturyoku.Append("│");
                }
            }
            syuturyoku.AppendLine();
        }


        /// <summary>
        /// 駒の居場所
        /// </summary>
        /// <param name="syuturyoku"></param>
        public static void HyojiKomanoIbasho(Shogiban shogiban, StringBuilder syuturyoku)
        {
            //IbasyoKomabetuBitboardItiran bb_koma, 
            //  KomaZenbuIbasyoBitboardItiran bb_komaZenbu
            syuturyoku.AppendLine("駒の居場所");

            // 駒全部☆
            {
                Setumei_Bitboards(new string[] { "対局者１", "対局者２" },
                    new Bitboard[] {
                        shogiban.GetBBKomaZenbu(OptionalPhase.Black),
                        shogiban.GetBBKomaZenbu(OptionalPhase.White)
                    }, syuturyoku);
                syuturyoku.AppendLine();
            }

            foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)// 対局者１、対局者２
            {
                // 見出し
                foreach (Koma km in Conv_Koma.ItiranTai[(int)tai])
                {
                    syuturyoku.Append(FormatBanWidthZenkaku(Conv_Koma.GetName(km)));
                }
                syuturyoku.AppendLine();

                // 盤
                Bitboard[] bbHairetu = new Bitboard[Conv_Komasyurui.Itiran.Length];
                int i = 0;
                foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                {
                    bbHairetu[i] = shogiban.GetBBKoma(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai)));
                    i++;
                }
                Setumei_Bitboards(bbHairetu, syuturyoku);
            }
        }
        /// <summary>
        /// 駒の利き数☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static void HyojiKomanoKikiSu(Shogiban shogiban, StringBuilder syuturyoku)
        {
            //, KikisuKomabetuCountboardItiran kikiKomabetuCB
            // KikisuZenbuCountboardItiran kikiZenbuCB

            syuturyoku.AppendLine("重ね利き数全部");
            // 対局者別　全部
            {
                // 見出し
                Setumei_Headers(Conv_Taikyokusya.NamaeItiran, syuturyoku);

                Util_Information.AppendLine_Top_Kyokumen(Conv_Taikyokusya.Itiran.Length, syuturyoku); // ┌──┬──┬──┐みたいな線☆
                for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
                {
                    AppendLine_Data_Countboard(shogiban, dan * Option_Application.Optionlist.BanYokoHaba, syuturyoku);

                    if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                    {
                        Util_Information.AppendLine_Middle(Conv_Taikyokusya.Itiran.Length, syuturyoku); // ├──┼──┼──┤みたいな線☆
                    }
                }
                Util_Information.AppendLine_Bottom(Conv_Taikyokusya.Itiran.Length, syuturyoku); // └──┴──┴──┘みたいな線☆
            }
            // 駒別
            foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran) // 対局者１、対局者２
            {
                foreach (Koma km in Conv_Koma.ItiranTai[(int)tai])
                {
                    syuturyoku.Append(Util_Information.FormatBanWidthZenkaku(Conv_Koma.GetName(km)));
                }
                syuturyoku.AppendLine();

                Util_Information.AppendLine_Top_Kyokumen(Conv_Komasyurui.Itiran.Length, syuturyoku);

                for (int dan = 0; dan < Option_Application.Optionlist.BanTateHaba; dan++)
                {
                    AppendLine_Data_Countboard(OptionalPhase.From( tai), shogiban, dan * Option_Application.Optionlist.BanYokoHaba, syuturyoku);

                    if (dan + 1 < Option_Application.Optionlist.BanTateHaba)
                    {
                        Util_Information.AppendLine_Middle(Conv_Komasyurui.Itiran.Length, syuturyoku);
                    }
                }
                Util_Information.AppendLine_Bottom(Conv_Komasyurui.Itiran.Length, syuturyoku);
            }
        }
        /// <summary>
        /// 駒の利き
        /// </summary>
        /// <param name="bbItiran_kikiZenbu"></param>
        /// <param name="bbItiran_kikiKomabetu"></param>
        /// <param name="syuturyoku"></param>
        public static void HyojiKomanoKiki(Shogiban shogiban, StringBuilder syuturyoku)
        {
            Debug.Assert(shogiban.IsActiveBBKiki(), "");

            // 利き全部
            {
                syuturyoku.AppendLine("利き（全部）");
                Bitboard[] bbHairetu = new Bitboard[Conv_Taikyokusya.Itiran.Length];
                foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)
                {
                    bbHairetu[(int)tai] = shogiban.GetBBKikiZenbu(OptionalPhase.From( tai));
                }
                Setumei_Bitboards(Conv_Taikyokusya.NamaeItiran, bbHairetu, syuturyoku);
            }
            // 駒別
            {
                syuturyoku.AppendLine("利き（駒別）");
                foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)// 対局者１、対局者２
                {
                    // 盤上
                    Bitboard[] bbHairetu = new Bitboard[Conv_Komasyurui.Itiran.Length];
                    foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                    {
                        bbHairetu[(int)ks] = shogiban.GetBBKiki(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai)));
                    }

                    Setumei_Bitboards(Med_Koma.GetKomasyuruiNamaeItiran(OptionalPhase.From( tai)), bbHairetu, syuturyoku);
                }
            }
        }
        /// <summary>
        /// 駒の動き☆
        /// </summary>
        /// <param name="komanoUgokikata"></param>
        /// <param name="syuturyoku"></param>
        public static void HyojiKomanoUgoki(Shogiban shogiban, int masuYososu, StringBuilder syuturyoku)
        {
            // KomanoUgokikata komanoUgokikata
            for (int ms = 0; ms < masuYososu; ms++)
            {
                syuturyoku.AppendLine($"ます{ms}");
                foreach (Taikyokusya tai in Conv_Taikyokusya.Itiran)
                {
                    // 盤上
                    Bitboard[] bbHairetu = new Bitboard[Conv_Komasyurui.Itiran.Length];
                    foreach (Komasyurui ks in Conv_Komasyurui.Itiran)
                    {
                        bbHairetu[(int)ks] = shogiban.GetKomanoUgokikata(Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, OptionalPhase.From(tai)), (Masu)ms);
                    }
                    Util_Information.Setumei_Bitboards(Med_Koma.GetKomasyuruiNamaeItiran(OptionalPhase.From( tai)), bbHairetu, syuturyoku);
                    syuturyoku.AppendLine();
                }
            }
        }

        public static void HyojiKomaHairetuYososuMade(Masu ms, Koma[] kmHairetu, StringBuilder syuturyoku)
        {
            syuturyoku.Append($"置くか除けるかした升=[{(Masu)ms}] 関連する飛び利き駒一覧=[");
            foreach (Koma km in kmHairetu)
            {
                if (Koma.Yososu == km)
                {
                    break;
                }
                syuturyoku.Append(km.ToString());
            }
            syuturyoku.AppendLine("]");
        }

        public static void Setumei_Discovered(Masu ms_removed, Kyokumen.Sindanyo kys, StringBuilder syuturyoku)
        {
            kys.TryInControl(ms_removed, out Koma[] kmHairetu_control);

            Bitboard bb_relative = new Bitboard();//関連のある飛び利き駒

            // 飛び利きを計算し直す
            foreach (Koma km in kmHairetu_control)
            {
                if (Koma.Yososu == km) { break; }

                // 駒の居場所
                Bitboard bb_ibasho = new Bitboard();
                kys.ToSetIbasho(km, bb_ibasho);
                while (bb_ibasho.Ref_PopNTZ(out Masu ms_ibasho))
                {
                    bb_relative.Standup(ms_ibasho);
                }
            }

            Setumei_1Bitboard("関連する飛び利き駒", bb_relative, syuturyoku);
        }
    }
}

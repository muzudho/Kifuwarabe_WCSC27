﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;
using Grayscale.A500_ShogiEngine.B523_UtilFv_____.C491____UtilFvIo;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A500_ShogiEngine.B523_UtilFv_____.C510____UtilFvLoad
{
    public abstract class Util_FvLoad
    {
        private class PP_P1Item
        {
            public string Filepath{get;set;}
            public int P1_base{get;set;}
            public PP_P1Item(string filepath, int p1_base)
            {
                this.Filepath = filepath;
                this.P1_base = p1_base;
            }
        }

        /// <summary>
        /// 棋譜ツリーを、平手初期局面 で準備します。
        /// </summary>
        public static void CreateKifuTree(
            out Earth out_earth1,
            out Sky out_positionA,
            out Grand out_kifu1
            )
        {


            // 棋譜
            out_earth1 = new EarthImpl();
            out_positionA = Util_SkyCreator.New_Hirate();
            out_kifu1 = new GrandImpl(out_positionA);
            out_earth1.SetProperty(Word_KifuTree.PropName_Startpos, "startpos");// 平手


            out_positionA.AssertFinger((Finger)0);
            Debug.Assert(!Conv_Masu.OnKomabukuro(
                Conv_Masu.ToMasuHandle(
                    Conv_Busstop.ToMasu(out_positionA.BusstopIndexOf((Finger)0))
                    )
                ), "駒が駒袋にあった。");
        }


        /// <summary>
        /// フィーチャー・ベクター関連のファイルを全て開きます。
        /// </summary>
        /// <param name="fv"></param>
        /// <param name="tv_orNull">学習でしか使いません。</param>
        /// <param name="rv_orNull">学習でしか使いません。</param>
        /// <param name="filepath_komawari_base"></param>
        /// <returns></returns>
        public static string OpenFv(FeatureVector fv, string filepath_komawari_base, KwLogger errH)
        {
            StringBuilder sb_result = new StringBuilder();

            {//駒割
                string filepath = filepath_komawari_base;
                if (!Util_FeatureVectorInput.Make_FromFile_Komawari(fv, filepath))
                {
                    sb_result.Append( "ファイルオープン失敗 Fv[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append( "開fv。");
            }

            {//スケール
                string filepath = Path.GetDirectoryName(filepath_komawari_base) + "/fv_00_Scale.csv";//komawari.csvと同じフォルダー
                if (!Util_FeatureVectorInput.Make_FromFile_Scale(fv, filepath))
                {
                    sb_result.Append("ファイルオープン失敗 Fv[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append("開Sc。");
            }

            {//KK
                string filepath = Path.GetDirectoryName(filepath_komawari_base) + "/fv_01_KK.csv";//komawari.csvと同じフォルダー
                if (!Util_FeatureVectorInput.Make_FromFile_KK(fv, filepath, errH))
                {
                    sb_result.Append( "ファイルオープン失敗 KK[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append( "開KK。");
            }

            {//1pKP
                string filepath = Path.GetDirectoryName(filepath_komawari_base) + "/fv_02_1pKP.csv";
                if (!Util_FeatureVectorInput.Make_FromFile_KP(fv, filepath, Playerside.P1, errH))
                {
                    sb_result.Append( "ファイルオープン失敗 1pKP[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append( "開1pKP。");
            }

            {//2pKP
                string filepath = Path.GetDirectoryName(filepath_komawari_base) + "/fv_03_2pKP.csv";
                if (!Util_FeatureVectorInput.Make_FromFile_KP(fv, filepath, Playerside.P2, errH))
                {
                    sb_result.Append( "ファイルオープン失敗 2pKP[" + filepath + "]。");
                    goto gt_EndMethod;
                }
                sb_result.Append( "開2pKP。");
            }

            {//盤上の駒
                List<PP_P1Item> p1List = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_04_PP_1p____Fu__.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____FU_____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_05_PP_1p____Kyo_.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KYO____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_06_pp_1p____Kei_.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KEI____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_07_pp_1p____Gin_.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____GIN____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_08_pp_1p____Kin_.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KIN____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_09_pp_1p____Hi__.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____HISYA__),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_10_pp_1p____Kaku.csv",FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_____KAKU___),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_18_pp_2p____Fu__.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____FU_____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_19_pp_2p____Kyo_.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KYO____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_20_pp_2p____Kei_.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KEI____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_21_pp_2p____Gin_.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____GIN____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_22_pp_2p____Kin_.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KIN____),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_23_pp_2p____Hi__.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____HISYA__),
                    new PP_P1Item( Path.GetDirectoryName(filepath_komawari_base) + "/fv_24_pp_2p____Kaku.csv",FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_____KAKU___),
                };

                foreach (PP_P1Item p1Item in p1List)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Banjo(fv, p1Item.Filepath, p1Item.P1_base, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + p1Item.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(p1Item.Filepath) + "。");
                }
            }

            {//１９枚の持ち駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_11_PP_1pMotiFu__.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIFU_____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_25_pp_2pMotiFu__.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIFU_____)
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti19Mai(fv, ppItem.Filepath, ppItem.P1_base, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

            {//３枚の持駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_12_PP_1pMotiKyo_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKYO____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_13_pp_1pMotiKei_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKEI____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_14_pp_1pMotiGin_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIGIN____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_15_pp_1pMotiKin_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKIN____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_26_pp_2pMotiKyo_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKYO____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_27_pp_2pMotiKei_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKEI____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_28_pp_2pMotiGin_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIGIN____),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_29_pp_2pMotiKin_.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKIN____),
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti3or5Mai(fv, ppItem.Filepath, ppItem.P1_base, 5, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

            {//２枚の持駒
                List<PP_P1Item> p1Items = new List<PP_P1Item>()
                {
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_16_pp_1pMotiHi__.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIHISYA__),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_17_pp_1pMotiKaku.csv"),FeatureVectorImpl.CHOSA_KOMOKU_1P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKAKU___),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_30_pp_2pMotiHi__.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIHISYA__),
                    new PP_P1Item( Path.Combine( Path.GetDirectoryName(filepath_komawari_base), "fv_31_pp_2pMotiKaku.csv"),FeatureVectorImpl.CHOSA_KOMOKU_2P + FeatureVectorImpl.CHOSA_KOMOKU_MOTIKAKU___),
                };

                foreach (PP_P1Item ppItem in p1Items)
                {
                    if (!Util_FeatureVectorInput.Make_FromFile_PP_Moti3or5Mai(fv, ppItem.Filepath, ppItem.P1_base, 3, errH))
                    {
                        sb_result.Append("ファイルオープン失敗 PP_Banjo[" + ppItem.Filepath + "]。");
                        goto gt_EndMethod;
                    }
                    sb_result.Append("開" + Path.GetFileName(ppItem.Filepath) + "。");
                }
            }

        gt_EndMethod:
            ;
            return sb_result.ToString();
        }

        

    }
}

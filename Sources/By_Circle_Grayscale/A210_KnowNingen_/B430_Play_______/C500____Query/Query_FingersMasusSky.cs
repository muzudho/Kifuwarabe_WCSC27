﻿using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct;
using Grayscale.A210_KnowNingen_.B430_Play_______.C250____Calc;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B430_Play_______.C500____Query
{
    //public class Arg_KmEffectSameIKUSA()
    //{
    //    public Arg_KmEffectSameIKUSA()
    //    {
    //    }
    //}

    public class Query_FingersMasusSky
    {

        /// <summary>
        /// 盤上の駒の利き（利きを調べる側）
        /// </summary>
        /// <param name="fs_sirabetaiKoma">fingers</param>
        /// <param name="masus_mikata_Banjo"></param>
        /// <param name="masus_aite_Banjo"></param>
        /// <param name="src_Sky"></param>
        /// <param name="errH_orNull"></param>
        /// <returns></returns>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> To_KomabetuKiki_OnBanjo(
            Fingers fs_sirabetaiKoma,
            SySet<SyElement> masus_mikata_Banjo,
            SySet<SyElement> masus_aite_Banjo,
            Position src_Sky,
            KwLogger errH_orNull
            )
        {
            // 利きを調べる側の利き（戦駒）
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuKiki = Query_SkyFingers.Get_PotentialMoves(src_Sky, fs_sirabetaiKoma, errH_orNull);

            // 盤上の現手番の駒利きから、 現手番の駒がある枡を除外します。
            komabetuKiki = Play_KomaAndMove.MinusMasus(src_Sky, komabetuKiki, masus_mikata_Banjo, errH_orNull);

            // そこから、相手番の駒がある枡「以降」を更に除外します。
            komabetuKiki = Play_KomaAndMove.Minus_OverThereMasus(src_Sky, komabetuKiki, masus_aite_Banjo, errH_orNull);

            return komabetuKiki;
        }




    }
}

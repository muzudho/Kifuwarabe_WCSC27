using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B220_ZobrishHash.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Diagnostics;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter
{
    public abstract class Conv_Position
    {
        public static ShogibanImpl ToShogiban(Playerside pside, Sky src_Sky, KwLogger logger)
        {
            ShogibanImpl shogiban = new ShogibanImpl();

            shogiban.Playerside = pside;// 表示データ


            // 将棋の駒４０個の場所を確認します。
            foreach (Finger finger in src_Sky.Fingers_All().Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop busstop = src_Sky.BusstopIndexOf(finger);

                SyElement masu = Conv_Busstop.ToMasu(busstop);
                Debug.Assert(Conv_Masu.OnAll(Conv_Masu.ToMasuHandle(masu)), "(int)koma.Masu=[" + Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(busstop)) + "]");//升番号

                // マスが重複することがある☆？（＾～＾）？
                shogiban.AddKoma(
                    masu,
                    busstop,
                    logger
                );
            }

            return shogiban;
        }

        /// <summary>
        /// 千日手判定用の、局面ハッシュを返します。
        /// 
        /// TODO: 持ち駒も判定したい。
        /// </summary>
        /// <returns></returns>
        public static ulong ToKyokumenHash(Sky position)
        {
            ulong hash = 0;

            foreach (Finger fig in position.Fingers_All().Items)
            {
                position.AssertFinger(fig);
                Busstop koma = position.BusstopIndexOf(fig);

                if (
                    Conv_Busstop.ToOkiba(koma)!=Okiba.ShogiBan ||
                    Conv_Busstop.ToKomadai(koma)
                    )
                {
                    // FIXME: 持ち駒はまだ見ていない。
                    continue;
                }

                // 盤上の駒。
                ulong value = Util_ZobristHashing.GetValue(
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma)),
                    Conv_Busstop.ToPlayerside(koma),
                    Conv_Busstop.ToKomasyurui(koma)
                    );

                hash ^= value;
            }

            return hash;
        }

        /// <summary>
        /// TODO: 持ち駒も判定したい。
        /// </summary>
        /// <returns></returns>
        public static string LogStr_Description(Sky position)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("┌──────────┐");
            foreach (Finger fig in position.Fingers_All().Items)
            {
                position.AssertFinger(fig);
                Busstop koma = position.BusstopIndexOf(fig);

                sb.AppendLine(Conv_Busstop.LogStr_Description(koma));
            }
            sb.AppendLine("└──────────┘");

            return sb.ToString();
        }

        /// <summary>
        /// TODO: 持ち駒も判定したい。
        /// </summary>
        /// <returns></returns>
        public static string LogStr_Graphical(Playerside pside, Sky position, KwLogger logger)
        {
            return Conv_Shogiban.ToLog(Conv_Position.ToShogiban(pside, position, logger));
        }
    }
}

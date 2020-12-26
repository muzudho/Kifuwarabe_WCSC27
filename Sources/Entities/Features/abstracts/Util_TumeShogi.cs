using System.Text;
using Grayscale.Kifuwarakei.Entities.Game;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    public abstract class Util_TumeShogi
    {
        /// <summary>
        /// 詰将棋を用意するぜ☆
        /// </summary>
        public static void TumeShogi(bool isSfen, int bango, Kyokumen ky, StringBuilder syuturyoku)
        {
            // FIXME: 終わったら元に戻したいが☆（＾～＾）
            Option_Application.Optionlist.PNChar[(int)Phase.Black] = MoveCharacter.TansakuNomi;
            Option_Application.Optionlist.PNChar[(int)Phase.White] = MoveCharacter.TansakuNomi;
            //Option_Application.Optionlist.BetaCutPer = 0; // ベータ・カットは使わないぜ☆（＾▽＾）ｗｗｗｗ
            //Option_Application.Optionlist.TranspositionTableTukau = false; // トランスポジション・テーブルは使わないぜ☆（＾▽＾）ｗｗｗｗ
            Option_Application.Optionlist.JosekiPer = 0; // 定跡は使わないぜ☆（＾▽＾）
            Option_Application.Optionlist.NikomaHyokaKeisu = 0; // 二駒関係の評価値も使わないぜ☆（＾▽＾）
            Option_Application.Optionlist.SikoJikan = 60000; // とりあえず 60 秒ぐらい☆
            Option_Application.Optionlist.SikoJikanRandom = 0;
            Option_Application.Optionlist.JohoJikan = 0; // 情報全部出すぜ☆

            // 詰め手数 + 1 にしないと、詰んでるか判断できないぜ☆（＾▽＾）
            //int motonoSaidaiFukasa = Option_Application.Optionlist.SaidaiFukasa;

            switch (bango)
            {
                #region 1手詰め
                case 0:
                    {
                        syuturyoku.AppendLine("# 1手詰め");
                        Option_Application.Optionlist.SaidaiFukasa = 1 + 1;
                        ky.SetBanjo(isSfen,
                            "　ラ　" +
                            "き　ひ" +
                            "　ら　" +
                            "　　　", false, syuturyoku);
                        ky.MotiKomas.Clear().Set(MotiKoma.H, 1);// = new int[] { 0, 0, 1, 0, 0, 0 };
                        ky.Tekiyo(true, syuturyoku);
                        Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                        syuturyoku.AppendLine();
                    }
                    break;
                #endregion
                #region 1手詰め
                case 1:
                    {
                        syuturyoku.AppendLine("# 1手詰め");
                        Option_Application.Optionlist.SaidaiFukasa = 3 + 1;
                        ky.SetBanjo(isSfen,
                            "　　ラ" +
                            "き　　" +
                            "　ら　" +
                            "　　　", false, syuturyoku);
                        ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        ky.Tekiyo(true, syuturyoku);
                        Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                        syuturyoku.AppendLine();
                    }
                    break;
                #endregion
                #region 3手詰め
                case 2:
                    {
                        syuturyoku.AppendLine("# 3手詰め");
                        Option_Application.Optionlist.SaidaiFukasa = 3 + 1;
                        ky.SetBanjo(isSfen,
                            "　ゾラ" +
                            "　　　" +
                            "ぞ　　" +
                            "ら　　", false, syuturyoku);
                        ky.MotiKomas.Clear().Set(MotiKoma.Z, 1).Set(MotiKoma.H, 1);// = new int[] { 1, 0, 1, 0, 0, 0 };
                        ky.Tekiyo(true, syuturyoku);
                        Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                        syuturyoku.AppendLine();
                    }
                    break;
                #endregion
                #region 1手詰め
                case 3:
                    {
                        syuturyoku.AppendLine("# 1手詰め");
                        Option_Application.Optionlist.SaidaiFukasa = 1 + 1;
                        ky.SetBanjo(isSfen,
                            "　ゾ　" +
                            "　ぞラ" +
                            "ぞ　　" +
                            "ら　　", false, syuturyoku);
                        ky.MotiKomas.Clear().Set(MotiKoma.H, 1);// = new int[] { 0, 0, 1, 0, 0, 0 };
                        ky.Tekiyo(true, syuturyoku);
                        Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                        syuturyoku.AppendLine();
                    }
                    break;
                #endregion
                #region 1手詰め
                default:
                    {
                        syuturyoku.AppendLine("# 1手詰め");
                        Option_Application.Optionlist.SaidaiFukasa = 1 + 1;
                        ky.SetBanjo(isSfen,
                            "　ラ　" +
                            "き　き" +
                            "　にら" +
                            "ぞひぞ", false, syuturyoku);
                        ky.MotiKomas.Clear();// = new int[] { 0, 0, 0, 0, 0, 0 };
                        ky.Tekiyo(true, syuturyoku);
                        Util_Information.Setumei_Lines_Kyokumen(ky, syuturyoku);
                        syuturyoku.AppendLine();
                    }
                    break;
                    #endregion
            }
        }
    }
}

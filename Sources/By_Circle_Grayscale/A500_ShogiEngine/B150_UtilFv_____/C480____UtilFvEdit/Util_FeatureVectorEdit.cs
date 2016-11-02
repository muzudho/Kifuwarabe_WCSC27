using Grayscale.A000_Platform___.B021_Random_____.C500____Struct;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C500____Struct;

namespace Grayscale.A500_ShogiEngine.B523_UtilFv_____.C480____UtilFvEdit
{
    /// <summary>
    /// 主に学習時、あるいはゲーム起動時にだけ使い、対局中（ゲーム中）に使わないメソッドは、こちらに移動します。
    /// </summary>
    public abstract class Util_FeatureVectorEdit
    {
        /// <summary>
        /// 旧型。
        /// 適当に数字を埋めます☆
        /// </summary>
        public static void Make_Random(FeatureVector fv)
        {
            //
            // 駒割は固定。
            // コーディングの利便上、エラー駒やヌル駒にもランダム値を入れておく。
            //
            foreach (Komasyurui14 komaSyurui in Array_Komasyurui.Items_AllElements)//
            {
                fv.Komawari[(int)komaSyurui] = KwRandom.Random.Next(0, 999);
            }
            //
            // 
            //
            fv.Komawari[(int)Komasyurui14.H00_Null___] = 0;
            fv.Komawari[(int)Komasyurui14.H01_Fu_____] = 100;
            fv.Komawari[(int)Komasyurui14.H02_Kyo____] = 800;
            fv.Komawari[(int)Komasyurui14.H03_Kei____] = 200;
            fv.Komawari[(int)Komasyurui14.H04_Gin____] = 500;
            fv.Komawari[(int)Komasyurui14.H05_Kin____] = 600;
            // 玉はあとで。
            fv.Komawari[(int)Komasyurui14.H07_Hisya__] = 1600;
            fv.Komawari[(int)Komasyurui14.H08_Kaku___] = 1600;
            fv.Komawari[(int)Komasyurui14.H09_Ryu____] = 2000;
            fv.Komawari[(int)Komasyurui14.H10_Uma____] = 2000;
            fv.Komawari[(int)Komasyurui14.H11_Tokin__] = 600;
            fv.Komawari[(int)Komasyurui14.H12_NariKyo] = 600;
            fv.Komawari[(int)Komasyurui14.H13_NariKei] = 600;
            fv.Komawari[(int)Komasyurui14.H14_NariGin] = 600;
            //
            // 玉の駒割は計算で求める。 歩100×18 ＋ 香800×4 ＋ 桂200×4 ＋ 銀500×4 ＋ 金600×4 ＋ 飛1600×2 ＋ 角1600×2。
            fv.Komawari[(int)Komasyurui14.H06_Gyoku__] =
                fv.Komawari[(int)Komasyurui14.H01_Fu_____] * 18 +
                fv.Komawari[(int)Komasyurui14.H02_Kyo____] * 4 +
                fv.Komawari[(int)Komasyurui14.H03_Kei____] * 4 +
                fv.Komawari[(int)Komasyurui14.H04_Gin____] * 4 +
                fv.Komawari[(int)Komasyurui14.H05_Kin____] * 4 +
                fv.Komawari[(int)Komasyurui14.H07_Hisya__] * 2 +
                fv.Komawari[(int)Komasyurui14.H08_Kaku___] * 2 +
                0;
        }
    }
}

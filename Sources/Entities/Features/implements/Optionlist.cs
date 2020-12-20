using kifuwarabe_wcsc27.interfaces;
using kifuwarabe_wcsc27.abstracts;
using System.Diagnostics;

namespace kifuwarabe_wcsc27.implements
{
    public class Optionlist
    {
        public Optionlist()
        {
            AspirationFukasa = 7;
            AspirationWindow = Hyokati.Hyokati_SeiNoSu_Hiyoko;

            BanTateHabaOld = 0;
            BanYokoHabaOld = 0;
            BanTateHaba = 4;
            BanYokoHaba = 3;

            BetaCutPer = 100;
            HanpukuSinkaTansakuTukau = true;
            JohoJikan = 3000;
            JosekiPer = 50;
            JosekiRec = false;// 定跡は記録しない
            Learn = false;
            NikomaHyokaKeisu = 1.0d;
            NikomaGakusyuKeisu = 0.001d;// HYOKA_SCALEが 1.0d のとき、GAKUSYU_SCALE 0.00001d なら、小数点部を広く使って　じっくりしている☆（＾～＾）
            P1Com = false;
            P2Com = false;
            PNChar = new MoveCharacter[] { MoveCharacter.HyokatiYusen, MoveCharacter.HyokatiYusen };
            PNName = new string[] { "対局者１", "対局者２" };
            RandomCharacter = false;
            RandomNikoma = false;
            RandomStart = false;
            RenzokuTaikyoku = false;
            SagareruHiyoko = false;
            SaidaiEda = -1; // 負数なら未指定☆
            SaidaiFukasa = 1;
            SeisekiRec = false;// 成績は記録しない
            SennititeKaihi = false;
            SikoJikan = 500;
            SikoJikanRandom = 500;
            TranspositionTableTukau = true;
            UseTimeOver = true;
        }

        /// <summary>
        /// アスピレーション・ウィンドウ・サーチを使い始める深さだぜ☆（＾▽＾）
        /// </summary>
        public int AspirationFukasa { get; set; }

        /// <summary>
        /// アスピレーション・ウィンドウ・サーチで使う幅だぜ☆（＾▽＾）
        /// </summary>
        public Hyokati AspirationWindow { get; set; }

        /// <summary>
        /// 盤のタテ幅が何マスか。
        /// </summary>
        public int BanTateHaba { get; set; }
        public int BanTateHabaOld { get; set; }
        /// <summary>
        /// 盤のヨコ幅が何マスか。
        /// </summary>
        public int BanYokoHaba { get; set; }
        public int BanYokoHabaOld { get; set; }

        /// <summary>
        /// 説明は「定跡パーセント」参照☆（＾▽＾）
        /// ベータ・カットを採用する確率☆
        /// </summary>
        public int BetaCutPer { get; set; }

        /// <summary>
        /// 反復深化探索を使うなら真だぜ☆（＾▽＾）
        /// トランスポジション・テーブルを使うことが必要だぜ☆（＾▽＾）
        /// </summary>
        public bool HanpukuSinkaTansakuTukau { get; set; }

        /// <summary>
        /// 読み筋情報 を表示する間隔（単位：ミリ秒）
        /// </summary>
        public int JohoJikan { get; set; }

        /// <summary>
        /// 定跡パーセント☆
        /// 100以上で必ず定跡を使用☆（＾▽＾）
        /// 0以下で必ず定跡を不使用☆（＾▽＾）
        /// 1～99で定跡を1%～99%の確率で使用☆（＾▽＾）
        /// </summary>
        public int JosekiPer { get; set; }
        /// <summary>
        /// 定跡の記録をするなら真☆
        /// </summary>
        public bool JosekiRec { get; set; }
        /// <summary>
        /// 機械学習をするなら真☆
        /// </summary>
        public bool Learn { get; set; }

        /// <summary>
        /// 二駒関係評価値の係数☆
        /// 二駒関係の評価値は、手番×Ｐ×Ｐ　の組み合わせを足し合わせたものなので、大きな数になる。
        /// そこで、駒割りと　二駒関係の　バランスをとるために　割り算を行うんだぜ☆（＾～＾）
        /// </summary>
        public double NikomaHyokaKeisu { get; set; }
        /// <summary>
        /// 学習スケール☆
        /// 学習で動かす二駒関係の評価値も 0～1 点でやっているんだが　それでも大きすぎたりするので、
        /// もっと小さくしたいときに　減らすんだぜ☆（＾▽＾）
        /// </summary>
        public double NikomaGakusyuKeisu { get; set; }

        /// <summary>
        /// 対局者Ｎの指し手の性格☆（＾▽＾）
        /// </summary>
        public MoveCharacter[] PNChar { get; set; }
        /// <summary>
        /// 対局者Ｎの表示名☆（＾▽＾）コンソール・ゲーム用だぜ☆
        /// </summary>
        public string[] PNName { get; set; }
        /// <summary>
        /// 対局者１はコンピューター☆（＾▽＾）
        /// </summary>
        public bool P1Com { get; set; }
        /// <summary>
        /// 対局者２はコンピューター☆（＾▽＾）
        /// </summary>
        public bool P2Com { get; set; }

        /// <summary>
        /// 対局終了時に、コンピューターの指し手の性格をランダムに変えるぜ☆（＾▽＾） 主にルール別定跡をまんべんなく作る用だぜ☆
        /// </summary>
        public bool RandomCharacter { get; set; }
        /// <summary>
        /// ランダム二駒関係評価値を混ぜるぜ☆（＾▽＾）主に機械学習用だぜ☆（＾▽＾）
        /// </summary>
        public bool RandomNikoma { get; set; }
        /// <summary>
        /// ランダム局面から開始するぜ☆（＾▽＾）主に機械学習用だぜ☆（＾▽＾）
        /// </summary>
        public bool RandomStart { get; set; }
        /// <summary>
        /// 手番をランダムに選んで開始するぜ☆（＾▽＾）主に機械学習用だぜ☆（＾▽＾）
        /// </summary>
        public bool RandomStartTaikyokusya { get; set; }
        /// <summary>
        /// 連続対局モードで、ルール設定をランダムに変えるぜ☆（＾▽＾） 主にルール別定跡をまんべんなく作る用だぜ☆
        /// </summary>
        public bool RenzokuRandomRule { get; set; }
        /// <summary>
        /// アプリケーションを強制終了するまで、ノンストップの対局だぜ☆（＾▽＾）
        /// </summary>
        public bool RenzokuTaikyoku { get; set; }

        /// <summary>
        /// 何枝まで読むか☆ 負数で未指定扱い☆ テスト用☆（＾～＾）
        /// </summary>
        public int SaidaiEda { get; set; }
        /// <summary>
        /// 何手まで読むか☆
        /// 0～
        /// </summary>
        public int SaidaiFukasa { get; set; }
        /// <summary>
        /// 下がれる　ひよこ　モードフラグ☆　普通のひよこはいなくなるぜ☆（＾▽＾） #仲ルール
        /// </summary>
        public bool SagareruHiyoko { get; set; }
        /// <summary>
        /// 成績の記録をするなら真☆
        /// </summary>
        public bool SeisekiRec { get; set; }
        /// <summary>
        /// コンピューターが千日手を必ず回避するなら真☆（＾～＾）　機械学習で利用するぜ☆
        /// </summary>
        public bool SennititeKaihi { get; set; }
        /// <summary>
        /// 思考に使っていい時間。単位はミリ秒☆
        /// </summary>
        public long SikoJikan { get; set; }
        /// <summary>
        /// 思考に使っていい時間に、ランダムに追加される分の最大量☆　単位はミリ秒☆
        /// ランダム関数の制限で int 型☆ 0～（この数字未満）
        /// </summary>
        public int SikoJikanRandom { get; set; }
        /// <summary>
        /// 今回の探索で、思考に使っていい時間。単位はミリ秒☆（ランダム時間込み）
        /// </summary>
        public long SikoJikan_KonkaiNoTansaku { get; private set; }
        /// <summary>
        /// 今回の探索で使っていい時間（ランダム時間込み）
        /// </summary>
        /// <returns></returns>
        public void SetSikoJikan_KonkaiNoTansaku()
        {
            SikoJikan_KonkaiNoTansaku = SikoJikan + Option_Application.Random.Next(SikoJikanRandom);

            Debug.Assert(0<Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku, $@"思考時間が1ミリ秒も無いぜ☆（＾～＾）！
SikoJikan={SikoJikan}
SikoJikan_KonkaiNoTansaku={SikoJikan_KonkaiNoTansaku}
");
        }

        /// <summary>
        /// トランスポジション・テーブルを使うなら真だぜ☆（＾▽＾）
        /// </summary>
        public bool TranspositionTableTukau { get; set; }

        /// <summary>
        /// 時間切れの使用有無☆ 主にデバッグのトレース時に使用☆（＾～＾）
        /// </summary>
        public bool UseTimeOver { get; set; }

        /// <summary>
        /// USI通信モードを途中でやめたくなったら偽にして使う☆（＾～＾）
        /// </summary>
        public bool USI { get; set; }
    }
}

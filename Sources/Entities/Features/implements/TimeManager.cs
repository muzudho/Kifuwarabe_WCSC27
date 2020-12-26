using System.Diagnostics;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// 思考時間を管理したり、
    /// 定跡ファイルの読み書きタイミングを調整したりと、時間周りを一括で担当するぜ☆（＾～＾）
    /// </summary>
    public class TimeManager
    {
        public TimeManager()
        {
            this.Stopwatch_Tansaku = new Stopwatch();
            this.Stopwatch_Savefile = new Stopwatch();
            this.Stopwatch_RenzokuRandomRule = new Stopwatch();
        }

        /// <summary>
        /// ストップウォッチ（探索用）
        /// </summary>
        public Stopwatch Stopwatch_Tansaku { get; set; }
        public void RestartStopwatch_Tansaku()
        {
            this.Stopwatch_Tansaku.Restart();
        }
        /// <summary>
        /// ストップウォッチ（定跡読み書き用）
        ///
        /// ・アプリケーション開始とともに　計測スタート。
        /// ・定跡等外部ファイルの保存時、計測スタートから　Ｎミリ秒経過後は、定跡等外部ファイルの保存が可能。（通常、再読み込みは不要）
        /// 　定跡等外部ファイルの保存後に　計測リスタート。
        /// ・アプリケーション終了時に　定跡等外部ファイルを保存すること。（quitコマンドの重要性）
        /// </summary>
        public Stopwatch Stopwatch_Savefile { get; set; }
        public void RestartStopwatch_Savefile()
        {
            this.Stopwatch_Savefile.Restart();
        }
        /// <summary>
        /// ストップウォッチ（自動対局時、ルール変更用）
        /// </summary>
        public Stopwatch Stopwatch_RenzokuRandomRule { get; set; }
        public void RestartStopwatch_RenzokuRandomRule()
        {
            this.Stopwatch_RenzokuRandomRule.Restart();
        }

        /// <summary>
        /// 前回　読み筋情報　を出力した時間☆（単位はミリ秒）
        /// </summary>
        public long LastJohoTime { get; set; }
        /// <summary>
        /// 前回　“定跡”　を読み書きした時間☆（単位はミリ秒）
        /// </summary>
        public long LastJosekiTime { get; set; }

        /// <summary>
        /// 探索を打ち切る時間を超えていれば真☆（探索の葉用）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_TansakuHappa()
        {
            bool isTimeOver =
                Option_Application.Optionlist.UseTimeOver && // 時間切れ設定を使用
                Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku < this.Stopwatch_Tansaku.ElapsedMilliseconds;
            if (isTimeOver)
            {
                Util_Tansaku.BadUtikiri = true;
            }
            return isTimeOver;
        }

        /// <summary>
        /// 探索を打ち切る時間を超えていれば真☆（反復深化探索用）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_IterationDeeping()
        {
            bool isTimeOver = Option_Application.Optionlist.SikoJikan_KonkaiNoTansaku < this.Stopwatch_Tansaku.ElapsedMilliseconds;
            if (isTimeOver)
            {
                Util_Tansaku.BadUtikiri = true;
            }
            return isTimeOver;
        }

        /// <summary>
        /// 定跡等外部ファイルの保存間隔の調整だぜ☆　もう保存していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_Savefile()
        {
            // （＾～＾）少なくとも１分は間隔を開けるようにするかだぜ☆
            return 60 * 1000 < this.Stopwatch_Savefile.ElapsedMilliseconds;
        }
        /// <summary>
        /// 連続対局時のルール変更間隔の調整だぜ☆　もう変更していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_RenzokuRandomRule()
        {
            // （＾～＾）少なくとも１分は間隔を開けるようにするかだぜ☆
            return 60 * 1000 < this.Stopwatch_RenzokuRandomRule.ElapsedMilliseconds;
        }
    }
}

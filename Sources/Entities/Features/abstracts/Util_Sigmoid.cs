﻿using kifuwarabe_wcsc27.machine;
using kifuwarabe_wcsc27.interfaces;
using System;
using System.Text;
using Grayscale.Kifuwarakei.Entities.Logging;

namespace kifuwarabe_wcsc27.abstracts
{
    /// <summary>
    /// シグモイド曲線とは☆（＾▽＾）
    /// 
    /// 横を x、縦を y のグラフにしたときの大まかなイメージ☆
    /// 
    /// 　　シグモイド曲線
    /// 1        ┌------
    ///          │
    /// 0.5      │
    ///          │
    /// 0  ------┘
    ///   -∞     0     ∞
    /// 
    /// この線を水平、垂直な部分が無いように　あらゆる部分で微妙に　カーブ　させた感じ☆
    /// x = 0 にある垂直線は　ごく細のS字カーブ　をしていると思ってもらいたい☆（＾～＾）
    /// x が 0 からちょっと離れれば、ほとんど　y は 0 か 1 か、といったグラフだぜ☆（＾～＾）
    /// 
    /// x が 正の数      のとき y は 　1 　に限りなく近い　0.9999...だったりする☆
    /// x が   0        のとき y は 　0.5 ぴったり☆
    /// x が 極めて0付近 のとき y は 　0.4 だったり 0.2 だったり 0.01 だったりして、
    /// 　　　　　　　　　　　　　　　　　　なんかストローで吸うタピオカのようにタテを速く移動する☆
    /// x が 負の数      のとき y は 　0 　に限りなく近い　0.000...1 だったりする☆
    /// 
    /// という形をしているんだぜ☆
    /// 
    /// -∞ ≦ ｘ ≦ ∞　というアナログな数字ｘを、　0 ＜ ｙ ＜ １　というデジタルな数字ｙに分けるのに使う☆
    /// 
    /// ────────────────────────────────────────
    /// 
    /// シグモイド曲線の便利さとは☆（＾▽＾）
    /// 
    /// 
    /// で、ｙ　が　０　か　１　のどっちかというのは　全然　重要ではなく、
    /// 
    /// x = 0 付近のどっちつかずな場所では　線の傾斜が急　となっており、
    /// x が 0 からある程度離れた場合は、
    /// もう　０ から　だいぶ離れているｘだろうが、思いっきり離れているｘだろうが、
    /// そのどちらも　線の傾斜の急さ　は同じようなもので、緩い、
    /// といった　傾斜の緩急　という違いが付いていることが重要だぜ☆（＾▽＾）ｗｗｗ
    /// （性質：　x=0からの遠さに応じた、線の傾斜の急さ、緩さの違い）
    /// 
    /// 
    /// この性質の　傾斜の緩急　を調整量として利用し、
    /// ｘが０に　近いものほど　ｘを調整する　（ｘ＝０　から遠ざける）
    /// ｘが０から遠いものは　　ｘに変化が起こらないようにさせる、
    /// 
    /// といった　だんだんと　変化を落ち着かせていく働き　を持たせることができるぜ☆（＾～＾）
    /// 
    /// 丁度いい調整で止まる、ということが期待できるな☆（＾▽＾）
    /// 
    /// </summary>
    public abstract class Util_Sigmoid
    {
        /// <summary>
        /// 窓関数というものがあり、その x が取りうる幅だぜ☆（＾～＾）
        /// 256 にしておくと、x = -256 ～ 256 までを取るぜ☆
        /// 
        /// Bonanza6.0 では、歩の初期値 100 点の交換値 200 （実際には 174）が収まる程度の 256 という人力調整数字だぜ☆
        /// ちょうど x は、指し手２つの間の評価値の差　として使っているので、
        /// 歩を交換する程度より小さな差は、　とても関心のある差　と見ているようだな☆（＾▽＾）
        /// </summary>
        private const float WINDOW = 256.0f;

        /// <summary>
        /// シグモイド関数の係数☆（＾～＾）
        /// 1 を基準として、
        /// 1 より大きくすると x の動きに敏感に y が 0 か 1 のどちらかに早く寄り、
        /// 1 より小さくすると 遅く寄るぜ☆
        /// 
        /// Bonanza6.0 では、
        /// const double delta = (double)FV_WINDOW / 7.0;
        /// としていて、 x / delta のように使っているぜ☆　x * (1/delta) と同じだな☆（＾～＾）
        /// 
        /// WINDOW / 7 の意味
        /// -----------------
        /// WINDOW / 8 にしておくと、 x = 256 のとき、y = 1 / (1 + 10^8)　になるぜ☆
        /// WINDOW / 7 にしておくと、 x = 256 のとき、y = 1 / (1 + 10^7)　になるぜ☆
        /// WINDOW / 6 にしておくと、 x = 256 のとき、y = 1 / (1 + 10^6)　になるぜ☆
        /// 
        /// 係数を 1 / (WINDOW / ●)　にしておくと、●は、x=256のときの指数になるぜ☆（＾～＾）
        /// ●を 1 増やすと y が使用する小数点以下が 1桁深まり、 1 減らすと 1桁浅まるぜ☆
        /// 
        /// x の枠の横幅をいっぱい使ったときに、どれぐらいの桁まで細かく見るかという数字だな☆
        /// で、7 が選ばれているのは人力調整数字だろう☆（＾～＾）
        /// </summary>
        private const float ALPHA = 1.0f / ( WINDOW / 7.0f );

        /// <summary>
        /// 微分した値で三駒関係のパラメーター（feature vector）の修正を行うには
        /// 値が大きすぎるので、この数字で割って　小さくしているぜ☆（＾～＾）
        /// </summary>
        private const float FV_SCALE = 32;

        /// <summary>
        /// 詳しい説明は Sigmoid( float a, float x ) の方を見ろだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Sigmoid(float x)
        {
            if (x < -WINDOW) { x = -WINDOW; }
            else if (x > WINDOW) { x = WINDOW; }

            float y =
                1.0f / (
                1.0f + (float)Math.Exp(-x)
                );

            return y;
        }

        /// <summary>
        /// 使い方の例：　ｙ　＝　シグモイド関数（　ｘ　）
        /// </summary>
        /// <param name="a">基準が 1。正の実数を入れる☆ x の動きによって y が 0 か 1 のどちらかに寄る早さを、aが大きいほど敏感、小さいほど鈍感にさせるぜ☆</param>
        /// <param name="x">本来 -∞～∞ だが、-FV_WINDOW～FV_WINDOW の範囲に限っているぜ☆</param>
        /// <returns>0 ＜ ｙ ＜ 1 だぜ☆　x が 0 のときは 0.5 だぜ☆
        /// x が 0 から少しでも離れると、y は 0 か 1 のどちらか側にすぐ寄るので、
        /// 基本的には　0 か 1 かを分けるグラフになるぜ☆
        /// ただし、細かく言うと、0 や 1 に限りなく近くに寄るだけで、ぴったり　0 や 1 になることはない☆
        /// </returns>
        public static float Sigmoid(float a, float x)
        {
            if (x < -WINDOW) { x = -WINDOW; }
            else if (x > WINDOW) { x = WINDOW; }

            // シグモイド曲線の式
            // y = 1 / (1 + 10^-ax)
            //
            // ここで、
            //
            // y = 1 / (1 + 10^-ax)
            //          ~~~
            //
            // 波線部を詳しく見ると、
            // y = 1 / ( 1より大きな数 )
            // が約束されている☆
            //
            // y = 1 / ( 1 + ● )
            // の形になっていて、●は正の数になる。
            // 確かめてみよう☆
            // 10^-1 は 0.1 のことで、10^0 は 1、 10^1 は 10☆　マイナスにはならないぜ☆
            //
            //
            // 分母が 1 より大きく、1 をどれだけ割っても 0 よりは小さくならないので、
            // y は　0 ＜ ～ ＜ 1　の範囲に収まる☆
            float y =
                1.0f / (
                1.0f + (float)Math.Exp(a * -x)
                );

            return y;
        }


        public static void Test(StringBuilder syuturyoku)
        {

            //*
            {
                float[] alphas = new float[] {
                    1 / (WINDOW / 5.0f),
                    1 / (WINDOW /6.0f),
                    1 / (WINDOW /7.0f),
                    1 / (WINDOW /8.0f),
                    1 / (WINDOW /9.0f),
                };
                float[] nazos = new float[] {//謎の数字
                    5.0f,
                    6.0f,
                    7.0f,
                    8.0f,
                    9.0f,
                };

                for (int iA = 0; iA < alphas.Length; iA++)
                {
                    float alpha = alphas[iA];
                    float nazo = nazos[iA];
                    syuturyoku.AppendLine($"nazo = [{nazo}]    alpha = [{ alpha }]");
                    // nazo が 7 の時に合わせて x を入れてあるぜ☆（＾～＾）
                    syuturyoku.AppendLine($"sisu(-256.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha ,- 256.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha,-256.0000f)) })");//[7]
                    syuturyoku.AppendLine($"sisu(-255.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, -255.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -255.0000f)) })");//
                    syuturyoku.AppendLine($"sisu(-219.4285) → [{ string.Format("{0,12:F9}", Sisu(alpha, -219.4285f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -219.4285f)) })");//[5.999998]
                    syuturyoku.AppendLine($"sisu(-192.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, -192.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -192.0000f)) })");//[5.25]
                    syuturyoku.AppendLine($"sisu(-182.8571) → [{ string.Format("{0,12:F9}", Sisu(alpha, -182.8571f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -182.8571f)) })");//[4.999999]
                    syuturyoku.AppendLine($"sisu(-146.2857) → [{ string.Format("{0,12:F9}", Sisu(alpha, -146.2857f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -146.2857f)) })");//[4]
                    syuturyoku.AppendLine($"sisu(-128.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, -128.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -128.0000f)) })");//[3.5]
                    syuturyoku.AppendLine($"sisu(-109.7142) → [{ string.Format("{0,12:F9}", Sisu(alpha, -109.7142f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -109.7142f)) })");//[2.999998]
                    syuturyoku.AppendLine($"sisu(- 73.1428) → [{ string.Format("{0,12:F9}", Sisu(alpha, -73.1428f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -73.1428f)) })");//[1.999998]
                    syuturyoku.AppendLine($"sisu(- 36.5714) → [{ string.Format("{0,12:F9}", Sisu(alpha, -36.5714f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -36.5714f)) })");//[0.9999992]
                    syuturyoku.AppendLine($"sisu(-  1.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, -1.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, -1.0000f)) })");//[0.02734375]
                    syuturyoku.AppendLine($"sisu(   0.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 0.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 0.0000f)) })");//[0]
                    syuturyoku.AppendLine($"sisu(   1.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 1.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 1.0000f)) })");//[-0.02734375]
                    syuturyoku.AppendLine($"sisu(  36.5714) → [{ string.Format("{0,12:F9}", Sisu(alpha, 36.5714f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 36.5714f)) })");//[-0.9999992]
                    syuturyoku.AppendLine($"sisu(  73.1428) → [{ string.Format("{0,12:F9}", Sisu(alpha, 73.1428f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 73.1428f)) })");//[-1.999998]
                    syuturyoku.AppendLine($"sisu( 109.7142) → [{ string.Format("{0,12:F9}", Sisu(alpha, 109.7142f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 109.7142f)) })");//[-2.999998]
                    syuturyoku.AppendLine($"sisu( 128.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 128.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 128.0000f)) })");//[-3.5]
                    syuturyoku.AppendLine($"sisu( 146.2857) → [{ string.Format("{0,12:F9}", Sisu(alpha, 146.2857f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 146.2857f)) })");//[-4]
                    syuturyoku.AppendLine($"sisu( 182.8571) → [{ string.Format("{0,12:F9}", Sisu(alpha, 182.8571f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 182.8571f)) })");//[-4.999999]
                    syuturyoku.AppendLine($"sisu( 192.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 192.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 192.0000f)) })");//[-5.25]
                    syuturyoku.AppendLine($"sisu( 219.4285) → [{ string.Format("{0,12:F9}", Sisu(alpha, 219.4285f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 219.4285f)) })");//[-5.999998]
                    syuturyoku.AppendLine($"sisu( 255.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 255.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 255.0000f)) })");//
                    syuturyoku.AppendLine($"sisu( 256.0000) → [{ string.Format("{0,12:F9}", Sisu(alpha, 256.0000f)) }] y=Sigmoid({ string.Format("{0,12:F9}", Sigmoid(alpha, 256.0000f)) })");//[-7]
                    syuturyoku.AppendLine();
                }
            }
            // */
            /*
nazo = [5]    alpha = [0.01953125]
sisu(-256.0000) → [ 5.000000000] y=Sigmoid( 0.006692851)
sisu(-255.0000) → [ 4.980469000] y=Sigmoid( 0.006823955)
sisu(-219.4285) → [ 4.285713000] y=Sigmoid( 0.013576940)
sisu(-192.0000) → [ 3.750000000] y=Sigmoid( 0.022977370)
sisu(-182.8571) → [ 3.571428000] y=Sigmoid( 0.027346810)
sisu(-146.2857) → [ 2.857143000] y=Sigmoid( 0.054313280)
sisu(-128.0000) → [ 2.500000000] y=Sigmoid( 0.075858180)
sisu(-109.7142) → [ 2.142856000] y=Sigmoid( 0.105000700)
sisu(- 73.1428) → [ 1.428570000] y=Sigmoid( 0.193321600)
sisu(- 36.5714) → [ 0.714285100] y=Sigmoid( 0.328652700)
sisu(-  1.0000) → [ 0.019531250] y=Sigmoid( 0.495117400)
sisu(   0.0000) → [ 0.000000000] y=Sigmoid( 0.500000000)
sisu(   1.0000) → [-0.019531250] y=Sigmoid( 0.504882600)
sisu(  36.5714) → [-0.714285100] y=Sigmoid( 0.671347300)
sisu(  73.1428) → [-1.428570000] y=Sigmoid( 0.806678500)
sisu( 109.7142) → [-2.142856000] y=Sigmoid( 0.894999300)
sisu( 128.0000) → [-2.500000000] y=Sigmoid( 0.924141800)
sisu( 146.2857) → [-2.857143000] y=Sigmoid( 0.945686700)
sisu( 182.8571) → [-3.571428000] y=Sigmoid( 0.972653200)
sisu( 192.0000) → [-3.750000000] y=Sigmoid( 0.977022600)
sisu( 219.4285) → [-4.285713000] y=Sigmoid( 0.986423100)
sisu( 255.0000) → [-4.980469000] y=Sigmoid( 0.993176000)
sisu( 256.0000) → [-5.000000000] y=Sigmoid( 0.993307200)

nazo = [6]    alpha = [0.0234375]
sisu(-256.0000) → [ 6.000000000] y=Sigmoid( 0.002472623)
sisu(-255.0000) → [ 5.976563000] y=Sigmoid( 0.002531111)
sisu(-219.4285) → [ 5.142856000] y=Sigmoid( 0.005807068)
sisu(-192.0000) → [ 4.500000000] y=Sigmoid( 0.010986940)
sisu(-182.8571) → [ 4.285713000] y=Sigmoid( 0.013576930)
sisu(-146.2857) → [ 3.428571000] y=Sigmoid( 0.031414380)
sisu(-128.0000) → [ 3.000000000] y=Sigmoid( 0.047425870)
sisu(-109.7142) → [ 2.571427000] y=Sigmoid( 0.071000150)
sisu(- 73.1428) → [ 1.714284000] y=Sigmoid( 0.152608800)
sisu(- 36.5714) → [ 0.857142200] y=Sigmoid( 0.297936800)
sisu(-  1.0000) → [ 0.023437500] y=Sigmoid( 0.494140900)
sisu(   0.0000) → [ 0.000000000] y=Sigmoid( 0.500000000)
sisu(   1.0000) → [-0.023437500] y=Sigmoid( 0.505859100)
sisu(  36.5714) → [-0.857142200] y=Sigmoid( 0.702063300)
sisu(  73.1428) → [-1.714284000] y=Sigmoid( 0.847391200)
sisu( 109.7142) → [-2.571427000] y=Sigmoid( 0.928999800)
sisu( 128.0000) → [-3.000000000] y=Sigmoid( 0.952574100)
sisu( 146.2857) → [-3.428571000] y=Sigmoid( 0.968585600)
sisu( 182.8571) → [-4.285713000] y=Sigmoid( 0.986423100)
sisu( 192.0000) → [-4.500000000] y=Sigmoid( 0.989013100)
sisu( 219.4285) → [-5.142856000] y=Sigmoid( 0.994193000)
sisu( 255.0000) → [-5.976563000] y=Sigmoid( 0.997468900)
sisu( 256.0000) → [-6.000000000] y=Sigmoid( 0.997527400)

nazo = [7]    alpha = [0.02734375]
sisu(-256.0000) → [ 7.000000000] y=Sigmoid( 0.000911051)
sisu(-255.0000) → [ 6.972656000] y=Sigmoid( 0.000936283)
sisu(-219.4285) → [ 5.999998000] y=Sigmoid( 0.002472628)
sisu(-192.0000) → [ 5.250000000] y=Sigmoid( 0.005220126)
sisu(-182.8571) → [ 4.999999000] y=Sigmoid( 0.006692858)
sisu(-146.2857) → [ 4.000000000] y=Sigmoid( 0.017986210)
sisu(-128.0000) → [ 3.500000000] y=Sigmoid( 0.029312230)
sisu(-109.7142) → [ 2.999998000] y=Sigmoid( 0.047425980)
sisu(- 73.1428) → [ 1.999998000] y=Sigmoid( 0.119203100)
sisu(- 36.5714) → [ 0.999999200] y=Sigmoid( 0.268941600)
sisu(-  1.0000) → [ 0.027343750] y=Sigmoid( 0.493164500)
sisu(   0.0000) → [ 0.000000000] y=Sigmoid( 0.500000000)
sisu(   1.0000) → [-0.027343750] y=Sigmoid( 0.506835500)
sisu(  36.5714) → [-0.999999200] y=Sigmoid( 0.731058400)
sisu(  73.1428) → [-1.999998000] y=Sigmoid( 0.880796900)
sisu( 109.7142) → [-2.999998000] y=Sigmoid( 0.952574000)
sisu( 128.0000) → [-3.500000000] y=Sigmoid( 0.970687700)
sisu( 146.2857) → [-4.000000000] y=Sigmoid( 0.982013800)
sisu( 182.8571) → [-4.999999000] y=Sigmoid( 0.993307100)
sisu( 192.0000) → [-5.250000000] y=Sigmoid( 0.994779900)
sisu( 219.4285) → [-5.999998000] y=Sigmoid( 0.997527400)
sisu( 255.0000) → [-6.972656000] y=Sigmoid( 0.999063700)
sisu( 256.0000) → [-7.000000000] y=Sigmoid( 0.999088900)

nazo = [8]    alpha = [0.03125]
sisu(-256.0000) → [ 8.000000000] y=Sigmoid( 0.000335350)
sisu(-255.0000) → [ 7.968750000] y=Sigmoid( 0.000345992)
sisu(-219.4285) → [ 6.857141000] y=Sigmoid( 0.001050812)
sisu(-192.0000) → [ 6.000000000] y=Sigmoid( 0.002472623)
sisu(-182.8571) → [ 5.714284000] y=Sigmoid( 0.003287666)
sisu(-146.2857) → [ 4.571428000] y=Sigmoid( 0.010237290)
sisu(-128.0000) → [ 4.000000000] y=Sigmoid( 0.017986210)
sisu(-109.7142) → [ 3.428569000] y=Sigmoid( 0.031414450)
sisu(- 73.1428) → [ 2.285712000] y=Sigmoid( 0.092313180)
sisu(- 36.5714) → [ 1.142856000] y=Sigmoid( 0.241796300)
sisu(-  1.0000) → [ 0.031250000] y=Sigmoid( 0.492188100)
sisu(   0.0000) → [ 0.000000000] y=Sigmoid( 0.500000000)
sisu(   1.0000) → [-0.031250000] y=Sigmoid( 0.507811800)
sisu(  36.5714) → [-1.142856000] y=Sigmoid( 0.758203700)
sisu(  73.1428) → [-2.285712000] y=Sigmoid( 0.907686800)
sisu( 109.7142) → [-3.428569000] y=Sigmoid( 0.968585600)
sisu( 128.0000) → [-4.000000000] y=Sigmoid( 0.982013800)
sisu( 146.2857) → [-4.571428000] y=Sigmoid( 0.989762700)
sisu( 182.8571) → [-5.714284000] y=Sigmoid( 0.996712300)
sisu( 192.0000) → [-6.000000000] y=Sigmoid( 0.997527400)
sisu( 219.4285) → [-6.857141000] y=Sigmoid( 0.998949200)
sisu( 255.0000) → [-7.968750000] y=Sigmoid( 0.999654000)
sisu( 256.0000) → [-8.000000000] y=Sigmoid( 0.999664700)

nazo = [9]    alpha = [0.03515625]
sisu(-256.0000) → [ 9.000000000] y=Sigmoid( 0.000123395)
sisu(-255.0000) → [ 8.964844000] y=Sigmoid( 0.000127809)
sisu(-219.4285) → [ 7.714283000] y=Sigmoid( 0.000446206)
sisu(-192.0000) → [ 6.750000000] y=Sigmoid( 0.001169510)
sisu(-182.8571) → [ 6.428570000] y=Sigmoid( 0.001612155)
sisu(-146.2857) → [ 5.142857000] y=Sigmoid( 0.005807060)
sisu(-128.0000) → [ 4.500000000] y=Sigmoid( 0.010986940)
sisu(-109.7142) → [ 3.857140000] y=Sigmoid( 0.020691170)
sisu(- 73.1428) → [ 2.571427000] y=Sigmoid( 0.071000150)
sisu(- 36.5714) → [ 1.285713000] y=Sigmoid( 0.216579300)
sisu(-  1.0000) → [ 0.035156250] y=Sigmoid( 0.491211900)
sisu(   0.0000) → [ 0.000000000] y=Sigmoid( 0.500000000)
sisu(   1.0000) → [-0.035156250] y=Sigmoid( 0.508788200)
sisu(  36.5714) → [-1.285713000] y=Sigmoid( 0.783420700)
sisu(  73.1428) → [-2.571427000] y=Sigmoid( 0.928999800)
sisu( 109.7142) → [-3.857140000] y=Sigmoid( 0.979308800)
sisu( 128.0000) → [-4.500000000] y=Sigmoid( 0.989013100)
sisu( 146.2857) → [-5.142857000] y=Sigmoid( 0.994193000)
sisu( 182.8571) → [-6.428570000] y=Sigmoid( 0.998387900)
sisu( 192.0000) → [-6.750000000] y=Sigmoid( 0.998830500)
sisu( 219.4285) → [-7.714283000] y=Sigmoid( 0.999553800)
sisu( 255.0000) → [-8.964844000] y=Sigmoid( 0.999872200)
sisu( 256.0000) → [-9.000000000] y=Sigmoid( 0.999876600)
            */

            Logger.Flush(syuturyoku);
            Util_Machine.ReadKey();
        }

        /// <summary>
        /// （＾～＾）勉強用☆
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static float Sisu(float a, float x)
        {
            if (x < -WINDOW) { x = -WINDOW; }
            else if (x > WINDOW) { x = WINDOW; }

            return a * -x;
        }

        /// <summary>
        /// シグモイドの導関数(導関数；derived function)
        /// 
        /// 式
        /// ( 1 - Sigmoid(x) ) * Sigmoid(x)
        /// 
        /// （＾～＾）なぜ、こんな形なのかの説明だぜ☆
        /// 
        /// シグモイドの世界観では、
        /// ｘ世界は大雑把に見て　０～∞　の世界で、
        /// ｙ世界は大雑把に見て　０～１　の世界だった☆（＾～＾）
        /// 
        /// で、ここで一旦　∞　や　指数関数的な性質が出てくる世界　は想像しにくいので、
        /// 一旦　シグモイドから　指数関数　的な性質を取り除いて、
        /// 単に x を０．０１倍するだけの　Zoom(x)　ということにし、
        /// 仮に　シグモイドの性質　を持っているということにして説明してしまおう☆（＾▽＾）
        /// 
        /// ( 1 - Zoom(x) ) * Zoom(x)
        /// 
        /// 等倍にすると、もっと簡単になるぜ☆！（＾▽＾）
        /// 
        /// ( 1 - n ) * n
        /// 
        /// あなたは　シグモイド　というガリバー・トンネルをくぐることができて、
        /// アナログ世界の住所　ｘ　から
        /// デジタル世界の住所　ｙ　に　行き来できるとするぜ☆（＾▽＾）
        /// 
        /// まあ　Sigmoid(x)　が　Zoom(x)　になり　n　にまでなったので、
        /// ただのトンネルだぜ☆（＾＿＾）ｗｗｗ
        /// 
        /// 
        /// この２つの世界ｘ、ｙで、架空の共通単位、例えば　１歩幅　というのが
        /// どういう距離間隔かというと、
        /// 
        /// ( 1 - n ) * n
        /// 
        /// なわけだぜ☆（＾＿＾）
        /// もしこれが、
        /// 
        /// ( 1 - n ) + n
        /// 
        /// だったら、とたんにハナシは簡単になるぜ☆（＾～＾）
        /// 世界の果てから　　　　　０．１歩幅　前に進むと同時に、
        /// 世界の反対側の果てから　９．９歩幅　縮まったということだぜ☆（＾▽＾）
        /// どれほどの歩幅で歩いても世界は　イッパツで　埋まってしまう☆
        /// 
        /// ではこれが、
        /// 
        /// ( 1 - n ) * n
        /// 
        /// だったらどうか☆（＾～＾）
        /// 足し算と　掛け算の　違いは、　長さが　広さ　に変わるようなものだぜ☆（＾▽＾）
        /// そこで、太る　ということにしよう☆
        /// 
        /// 世界の果てで　ヨコハバが　世界の長さの半分の　０．５長さ　太っても、
        /// 　　　　　　　タテハバは　世界の長さの半分の　０．５長さ　しか太らない、ということだぜ☆（＾▽＾）
        /// 
        /// 面積的には　４分の１、つまり　世界の２５％　ぐらいしか　狭まっていないことが
        /// ポイントだぜ☆（＾～＾）
        /// 
        /// ( 1 - n ) + n
        /// 
        /// に比べれば　むしろ
        /// 
        /// ( 1 - n ) * n
        /// 
        /// は　埋め尽くす容量は　おとなしいもんだぜ☆（＾▽＾）
        /// 
        /// 
        /// ここで　次の早見表を　見ていただこう☆
        /// 
        /// 　　　　　　　　　　　（数字は割合）
        ///      TATE列   YOKO列   FUTOSA列
        /// A行     0.9 *    0.1 =     0.09
        /// B行     0.8 *    0.2 =     0.16
        /// C行     0.7 *    0.3 =     0.21
        /// D行     0.6 *    0.4 =     0.24
        /// E行     0.5 *    0.5 =     0.25
        /// F行     0.4 *    0.6 =     0.24
        /// G行     0.3 *    0.7 =     0.21
        /// H行     0.2 *    0.8 =     0.16
        /// I行     0.1 *    0.9 =     0.09
        /// 
        /// ここで、 E行は、体がタテ・ヨコとも世界の長さの半分程度　太ったときに、
        /// 世界の面積の　0.25　ぐらいのでかさになるんだぜ☆（＾～＾）
        /// 
        /// 早見表の見方なんだが、
        /// FUTOSA列は、ｘの動きに対してｙがよく動くといった感じで見るんだぜ☆（＾～＾）
        /// 
        /// これが
        /// 
        /// ( 1 - n ) * n
        /// 
        /// だぜ☆
        /// では、等倍のガリバー・トンネルをくぐってほしい☆（＾▽＾）
        /// 
        /// Ｘの世界で　　世界の面積の　0.25　ぐらい太った　あなた　は、
        /// Ｙの世界でも　世界の長さの　0.25　ぐらい太っているぜ☆
        /// 
        /// さっきの早見表を見ると、正方形のときが　一番太く、
        /// ２辺の差が離れるほど　痩せていっているみたいだな☆（＾▽＾）
        /// 
        /// で、ここでハナシを戻すぜ☆
        /// 
        /// ( 1 - n ) * n
        /// 
        /// ( 1 - Zoom(x) ) * Zoom(x)
        /// 
        /// ( 1 - Sigmoid(x) ) * Sigmoid(x)
        /// 
        /// シグモイド　になると、ｘ　が　指数関数　的な細かさになるぜ☆（＾～＾）
        /// がんばって　ズームしろだぜ☆（＾▽＾）
        /// 
        /// なぜか Sigmoid(x) を微分すると、
        /// ( 1 - Sigmoid(x) ) * Sigmoid(x) のような性質をしているらしい☆
        /// 世界を等倍の１０×１０の盤として　図にすると、こんな感じ☆
        /// 
        /// .
        /// .
        /// .
        /// .
        /// .
        /// .
        /// .
        /// .
        /// .　（９つの点）ｘが－∞の近く☆
        /// ↓
        /// ..
        /// ..
        /// ..
        /// ..
        /// ..
        /// ..
        /// ..
        /// ..　（１６個の点）
        /// ↓
        /// ...
        /// ...
        /// ...
        /// ...
        /// ...
        /// ...
        /// ...　（２１個の点）
        /// ↓
        /// ....
        /// ....
        /// ....
        /// ....
        /// ....
        /// ....　（２４個の点）
        /// ↓
        /// .....
        /// .....
        /// .....
        /// .....
        /// .....　（２５個の点）ｘ＝０．５
        /// ↓
        /// ......
        /// ......
        /// ......
        /// ......　（２４個の点）
        /// ↓
        /// .......
        /// .......
        /// .......　（２１個の点）
        /// ↓
        /// ........
        /// ........　（１６個の点）
        /// ↓
        /// .........　（９個の点）ｘが∞の近く
        /// 
        /// 
        /// で、この点を左端から　縦　に積み上げていくと　シグモイド曲線に
        /// 戻るんじゃないか☆（＾～＾）
        /// テキストではグラフが描けないから描かないけど☆
        /// 　　　　　　　　　　　　　　　　　.　　　.　　.
        /// 　　　　　　　　　　　　　　.
        /// 　　　　　　　　　　　.
        /// 　　　　　　　　.
        /// .    .    . 
        /// ────────────────────────────────────────────
        /// 　　　　　　　　　　　　　　　　　　　　　　　　９
        /// 　　　　　　　　　　　　　　　　　　　　１６　１６
        /// 　　　　　　　　　　　　　　　　　２１　２１　２１
        /// 　　　　　　　　　　　　　　２４　２４　２４　２４
        /// 　　　　　　　　　　　２５　２５　２５　２５　２５
        /// 　　　　　　　　２４　２４　２４　２４　２４　２４
        /// 　　　　　２１　２１　２１　２１　２１　２１　２１
        /// 　　１６　１６　１６　１６　１６　１６　１６　１６
        /// ９　　９　　９　　９　　９　　９　　９　　９　　９
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static float DSigmoid(float x)
        {
            if (x <= -WINDOW || WINDOW <= x) { return 0.0f; }

            // ボナンザ 6.0 では WINDOW / 7 だが、
            // これは　x / (WINDOW / 7)　のように割り算の形で使うからなだけで、
            // 掛け算にすれば x * 7 / WINDOW　でも同じだぜ☆
            // 大樹の枝はそう書いているぜ☆（＾～＾）

            // 大樹の枝によると、符号の + - だけ知りたいので、
            // コンピューター将棋で使う分には a は掛けなくてもいいらしい☆（＾～＾）
            const float a = 7.0f / WINDOW;

		    return a * (1 - Sigmoid(x)) * Sigmoid(x);

            // ボナンザ6.0 での微分の仕方は、なんか分からなかったぜ☆（＾～＾）
            //
            //  e^(-x/(WINDOW/7))
            // ----------------------------------------------------------------------
            //  (WINDOW / 7) * ( e^(-x/(WINDOW/7)) + 1 ) * ( e^(-x/(WINDOW/7)) + 1 )
        }
    }
}

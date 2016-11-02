﻿using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Position___.C___500_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B270_Position___.C500____Struct
{

    /// <summary>
    /// 駒データベースだぜ☆（＾▽＾）
    /// </summary>
    public class PositionImpl : Position
    {
        /// <summary>
        /// 棋譜を新規作成するときに使うコンストラクター。
        /// </summary>
        public PositionImpl()
        {
            this.m_temezumi_ = 0;//初期局面は 0手目済み
            this.m_busstops_ = new List<Busstop>();
            this.MotiSu = new int[(int)Pieces.Num];
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        public PositionImpl(Position src)
        {
            // 局面ハッシュのクローン
            this.KyokumenHash = src.KyokumenHash;

            // 手番のクローン
            this.m_temezumi_ = src.Temezumi;

            // 星々のクローン
            this.m_busstops_ = new List<Busstop>();
            src.Foreach_Busstops((Finger finger, Busstop busstop, ref bool toBreak) =>
            {
                this.m_busstops_.Add(busstop);
            });
            this.MotiSu = new int[(int)Pieces.Num];
            Array.Copy(src.MotiSu, this.MotiSu, src.MotiSu.Length);
        }
        /// <summary>
        /// 追加分があれば。
        /// </summary>
        /// <param name="addsFinger1"></param>
        /// <param name="addsBusstops1"></param>
        public void AddObjects(Finger[] addsFinger1, Busstop[] addsBusstops1)
        {
            for (int i = 0; i < addsFinger1.Length; i++)
            {
                if (addsFinger1[i] != Fingers.Error_1)
                {
                    if (this.m_busstops_.Count == (int)addsFinger1[i])
                    {
                        // オブジェクトを追加します。
                        this.m_busstops_.Add(addsBusstops1[i]);
                    }
                    else if (this.m_busstops_.Count + 1 <= (int)addsFinger1[i])
                    {
                        // エラー
                        Debug.Assert((int)addsFinger1[i] < this.m_busstops_.Count, "要素の個数より2大きいインデックスを指定しました。 インデックス[" + (int)addsFinger1[i] + "]　要素の個数[" + this.m_busstops_.Count + "]");

                        string message = this.GetType().Name + "#SetStarPos：　リストの要素より2多いインデックスを指定されましたので、追加しません。starIndex=[" + addsFinger1[i] + "] / this.stars.Count=[" + this.m_busstops_.Count + "]";
                        //LarabeLogger.GetInstance().WriteLineError(LarabeLoggerList.ERROR, message);
                        throw new Exception(message);
                    }
                    else
                    {
                        this.m_busstops_[(int)addsFinger1[i]] = addsBusstops1[i];
                    }
                }
            }
        }


        public ulong KyokumenHash { get; set; }


        #region 手目済
        public void IncreaseTemezumi()
        {
            this.SetTemezumi(this.Temezumi + 1);// 1手進めます。
        }
        public void DecreaseTemezumi()
        {
            this.SetTemezumi(this.Temezumi - 1);// 1手戻します。
        }

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        public int Temezumi { get { return this.m_temezumi_; } }
        private int m_temezumi_;
        public void SetTemezumi(int temezumi)
        {
            this.m_temezumi_ = temezumi;
        }
        #endregion


        #region プロパティー
        /// <summary>
        /// 盤面なので、動かないもの（駒）の位置のリストだぜ☆（＾～＾）駒しかないはずなので、４０個のはずだぜ☆（＾～＾）
        /// </summary>
        private List<Busstop> m_busstops_;
        public List<Busstop> Busstops
        {
            get
            {
                return this.m_busstops_;
            }
        }
        /// <summary>
        /// 持ち駒の枚数だぜ☆（＾▽＾）
        /// </summary>
        public int[] MotiSu { get; set; }

        #endregion



        /// <summary>
        /// 盤上の駒数と、持ち駒の数の合計。
        /// </summary>
        public int Count
        {
            get
            {
                int maisu = this.m_busstops_.Count;

                for (int iMoti=0; iMoti<(int)Pieces.Num; iMoti++)
                {
                    maisu += this.MotiSu[iMoti];
                }

                return maisu;
            }
        }

        public void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (0 <= (int)finger && (int)finger < this.m_busstops_.Count)
            {
                return;
            }
            else
            {
                // エラー
                string message = this.GetType().Name + "#StarIndexOf：　スプライト配列の範囲を外れた添え字を指定されましたので、取得できません。スプライト番号=[" + finger + "] / スプライトの数=[" + this.m_busstops_.Count + "]\n memberName=" + memberName + "\n sourceFilePath=" + sourceFilePath + "\n sourceLineNumber=" + sourceLineNumber;
                Debug.Fail(message);
                throw new Exception(message);
            }
        }

        public Busstop BusstopIndexOf(Finger finger)
        {
            this.AssertFinger(finger);

            return this.m_busstops_[(int)finger];
        }


        public void Foreach_Busstops(DELEGATE_Sky_Foreach delegate_Sky_Foreach)
        {
            bool toBreak = false;

            Finger finger = 0;
            foreach (Busstop busstop in this.m_busstops_)
            {
                delegate_Sky_Foreach(finger, busstop, ref toBreak);

                finger = (int)finger + 1;
                if (toBreak)
                {
                    break;
                }
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 天上のすべての星の光
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="okiba"></param>
        /// <returns></returns>
        public Fingers Fingers_All()
        {
            Fingers fingers = new Fingers();

            this.Foreach_Busstops((Finger finger, Busstop light, ref bool toBreak) =>
            {
                fingers.Add(finger);
            });

            return fingers;
        }





        /// <summary>
        /// 駒台に戻すとき。上書き、または追加。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="finger"></param>
        /// <param name="busstop"></param>
        public void PutBusstop(Finger finger, Busstop busstop)
        {
            if (this.m_busstops_.Count == (int)finger)
            {
                // 新しい駒なら、オブジェクトを追加します。
                this.Busstops.Add(busstop);
            }
            else if (this.m_busstops_.Count + 1 <= (int)finger)
            {
                // エラー
                Debug.Assert((int)finger < this.Busstops.Count, "要素の個数より2大きいインデックスを指定しました。 インデックス[" + (int)finger + "]　要素の個数[" + this.Busstops.Count + "]");

                string message = this.GetType().Name + "#SetStarPos：　リストの要素より2多いインデックスを指定されましたので、追加しません。starIndex=[" + finger + "] / this.stars.Count=[" + this.m_busstops_.Count + "]";
                throw new Exception(message);
            }
            else
            {
                this.Busstops[(int)finger] = busstop;
            }
        }

    }
}
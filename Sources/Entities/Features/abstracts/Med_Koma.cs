using Grayscale.Kifuwarakei.Entities.Game;
using Grayscale.Kifuwarakei.Entities.Language;
using System;

namespace Grayscale.Kifuwarakei.Entities.Features
{
    /// <summary>
    /// mediator.
    /// </summary>
    public abstract class Med_Koma
    {
        static Med_Koma()
        {
            komasyuruiNamaeItiran = new string[Conv_Taikyokusya.Itiran.Length][];
            for (int iTai = 0; iTai < Conv_Taikyokusya.Itiran.Length; iTai++)
            {
                komasyuruiNamaeItiran[iTai] = new string[Conv_Komasyurui.Itiran.Length];
                int iKs = 0;
                foreach (Koma km in Conv_Koma.ItiranTai[(int)iTai])
                {
                    komasyuruiNamaeItiran[iTai][iKs] = Conv_Koma.GetName(km);
                    iKs++;
                }
            }
        }

        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// [対局者,駒種類]
        /// </summary>
        static string[][] komasyuruiNamaeItiran;
        public static string[] GetKomasyuruiNamaeItiran(Option<Phase> optionalPhase)
        {
            return komasyuruiNamaeItiran[OptionalPhase.ToInt(optionalPhase)];
        }
        public static string GetKomasyuruiNamae(Option<Phase> optionalPhase, Komasyurui ks)
        {
            return komasyuruiNamaeItiran[OptionalPhase.ToInt(optionalPhase)][(int)ks];
        }


        #region 駒→駒種類
        static Komasyurui[] m_KomaToKomasyurui_ = {
            // らいおん（対局者１、対局者２）
            Komasyurui.R,
            Komasyurui.R,

            // ぞう
            Komasyurui.Z,
            Komasyurui.Z,

            // パワーアップぞう
            Komasyurui.PZ,
            Komasyurui.PZ,

            // きりん
            Komasyurui.K,
            Komasyurui.K,

            // パワーアップきりん
            Komasyurui.PK,
            Komasyurui.PK,

            // ひよこ
            Komasyurui.H,
            Komasyurui.H,

            // にわとり
            Komasyurui.PH,
            Komasyurui.PH,

            // いぬ
            Komasyurui.I,
            Komasyurui.I,

            // ねこ
            Komasyurui.Neko,
            Komasyurui.Neko,

            // パワーアップねこ
            Komasyurui.PNeko,
            Komasyurui.PNeko,

            // うさぎ
            Komasyurui.U,
            Komasyurui.U,

            // パワーアップうさぎ
            Komasyurui.PU,
            Komasyurui.PU,

            // いのしし
            Komasyurui.S,
            Komasyurui.S,

            // パワーアップいのしし
            Komasyurui.PS,
            Komasyurui.PS,

            Komasyurui.Yososu,// 駒のない升だぜ☆（＾▽＾）
            Komasyurui.Yososu// 要素数だが、空白升、該当無し、としても使うぜ☆（＾▽＾）
        };
        public static Komasyurui KomaToKomasyurui(Koma km) { return Med_Koma.m_KomaToKomasyurui_[(int)km]; }
        #endregion

        #region 駒→手番
        static Taikyokusya[] m_KomaToTaikyokusya_ = {
            // らいおん（対局者１、対局者２）
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ぞう
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップぞう
            Taikyokusya.T1,
            Taikyokusya.T2,

            // きりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップきりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ひよこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // にわとり
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いぬ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // うさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップうさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップいのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            Taikyokusya.Yososu,//駒のない升だぜ☆（＾▽＾）
            Taikyokusya.Yososu// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };

        public static Taikyokusya KomaToTaikyokusya(Koma km)
        {
            // FIXME: 範囲外の引数を指定できるのがそもそもダメ☆（＾～＾）
            if (-1 < (int)km && (int)km < m_KomaToTaikyokusya_.Length)
            {
                return m_KomaToTaikyokusya_[(int)km];
            }
            else
            {
                throw new Exception($"km={(int)km} < m_KomaToTaikyokusya_.Length={m_KomaToTaikyokusya_.Length}");
            }
        }
        #endregion

        #region 盤上の駒→持駒
        /// <summary>
        /// 盤上の駒を、駒台の駒（相手の方に行く）に変換するぜ☆（＾▽＾）
        /// [駒]
        /// </summary>
        static MotiKoma[] m_BanjoKomaToMotiKoma_ = {
            // らいおん（対局者１、対局者２）
            MotiKoma.Yososu,
            MotiKoma.Yososu,

            // ぞう
            MotiKoma.z,
            MotiKoma.Z,

            // パワーアップぞう
            MotiKoma.z,
            MotiKoma.Z,

            // きりん
            MotiKoma.k,
            MotiKoma.K,

            // パワーアップきりん
            MotiKoma.k,
            MotiKoma.K,

            // ひよこ
            MotiKoma.h,
            MotiKoma.H,

            // にわとり
            MotiKoma.h,
            MotiKoma.H,

            // いぬ
            MotiKoma.i,
            MotiKoma.I,

            // ねこ
            MotiKoma.neko,
            MotiKoma.Neko,

            // パワーアップねこ
            MotiKoma.neko,
            MotiKoma.Neko,

            // うさぎ
            MotiKoma.u,
            MotiKoma.U,

            // パワーアップうさぎ
            MotiKoma.u,
            MotiKoma.U,

            // いのしし
            MotiKoma.s,
            MotiKoma.S,

            // パワーアップいのしし
            MotiKoma.s,
            MotiKoma.S,

            MotiKoma.Yososu,//駒のない升だぜ☆（＾▽＾）
            MotiKoma.Yososu// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };
        public static MotiKoma BanjoKomaToMotiKoma(Koma km) { return Med_Koma.m_BanjoKomaToMotiKoma_[(int)km]; }
        #endregion

        #region 駒種類と手番→持駒
        /// <summary>
        /// 指し手の駒の種類を、駒台の駒に変換するぜ☆（＾▽＾）
        /// [駒種類][手番]
        /// </summary>
        static MotiKoma[,] m_KomasyuruiAndTaikyokusyaToMotiKoma_ = {
            // らいおん打
            { MotiKoma.Yososu, MotiKoma.Yososu },

            // ぞう打
            { MotiKoma.Z, MotiKoma.z },

            // パワーアップぞう打
            { MotiKoma.Z, MotiKoma.z },

            // きりん打
            { MotiKoma.K, MotiKoma.k },

            // パワーアップきりん打
            { MotiKoma.K, MotiKoma.k },

            // ひよこ打
            { MotiKoma.H, MotiKoma.h },

            // にわとり打　→　持駒ひよこ　（成らずとして判定する必要あり）
            { MotiKoma.H, MotiKoma.h },//(2017-05-02 22:10 Modify){ MotiKoma.H, MotiKoma.H },

            // いぬ打
            { MotiKoma.I, MotiKoma.i },

            // ねこ打
            { MotiKoma.Neko, MotiKoma.neko },

            // パワーアップねこ打
            { MotiKoma.Neko, MotiKoma.neko },

            // うさぎ打
            { MotiKoma.U, MotiKoma.u },

            // パワーアップうさぎ打
            { MotiKoma.U, MotiKoma.u },

            // いのしし打
            { MotiKoma.S, MotiKoma.s },

            // パワーアップいのしし打
            { MotiKoma.S, MotiKoma.s },

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { MotiKoma.Yososu, MotiKoma.Yososu },
        };
        public static MotiKoma KomasyuruiAndTaikyokusyaToMotiKoma(Komasyurui ks, Taikyokusya tai) { return Med_Koma.m_KomasyuruiAndTaikyokusyaToMotiKoma_[(int)ks, (int)tai]; }
        #endregion

        #region 駒種類と手番→駒
        static Koma[,] m_KomasyuruiAndTaikyokusyaToKoma_ =
        {
            { Koma.R, Koma.r },// らいおん
            { Koma.Z, Koma.z },// ぞう
            { Koma.PZ, Koma.pz },// パワーアップぞう
            { Koma.K, Koma.k },// きりん
            { Koma.PK, Koma.pk },// パワーアップきりん
            { Koma.H, Koma.h },// ひよこ
            { Koma.PH, Koma.ph },// にわとり
            { Koma.I, Koma.i },// いぬ
            { Koma.Neko, Koma.neko },// ねこ
            { Koma.PNeko, Koma.pneko },// パワーアップねこ
            { Koma.U, Koma.u },// うさぎ
            { Koma.PU, Koma.pu },// パワーアップうさぎ
            { Koma.S, Koma.s },// いのしし
            { Koma.PS, Koma.ps },// パワーアップいのしし
            { Koma.Kuhaku, Koma.Kuhaku },// らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
        };
        public static Koma KomasyuruiAndTaikyokusyaToKoma(Komasyurui ks, Option<Phase> optionalPhase)
        {
            return Med_Koma.m_KomasyuruiAndTaikyokusyaToKoma_[(int)ks, OptionalPhase.ToInt(optionalPhase)];
        }
        #endregion

        #region 駒種類→持駒種類
        static MotiKomasyurui[] m_KomasyuruiToMotiKomasyurui_ =
        {
            // らいおん
            MotiKomasyurui.Yososu,

            // ぞう
            MotiKomasyurui.Z,

            // パワーアップぞう
            MotiKomasyurui.Z,

            // きりん
            MotiKomasyurui.K,

            // パワーアップきりん
            MotiKomasyurui.K,

            // ひよこ
            MotiKomasyurui.H,

            // にわとり
            MotiKomasyurui.H,

            // いぬ
            MotiKomasyurui.I,

            // ねこ
            MotiKomasyurui.Neko,

            // パワーアップねこ
            MotiKomasyurui.Neko,

            // うさぎ
            MotiKomasyurui.U,

            // パワーアップうさぎ
            MotiKomasyurui.U,

            // いのしし
            MotiKomasyurui.S,

            // パワーアップいのしし
            MotiKomasyurui.S,

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            MotiKomasyurui.Yososu,
        };
        public static MotiKomasyurui KomasyuruiToMotiKomasyrui(Komasyurui ks)
        {
            return Med_Koma.m_KomasyuruiToMotiKomasyurui_[(int)ks];
        }
        #endregion

        #region 持駒種類→駒種類
        static Komasyurui[] m_MotiKomasyuruiToKomasyurui_ =
        {
            // ぞう
            Komasyurui.Z,

            // きりん
            Komasyurui.K,

            // ひよこ
            Komasyurui.H,

            // いぬ
            Komasyurui.I,

            // ねこ
            Komasyurui.Neko,

            // うさぎ
            Komasyurui.U,

            // いのしし
            Komasyurui.S,

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            Komasyurui.Yososu,
        };
        public static Komasyurui MotiKomasyuruiToKomasyrui(MotiKomasyurui mks)
        {
            return Med_Koma.m_MotiKomasyuruiToKomasyurui_[(int)mks];
        }
        #endregion

        #region 持駒→駒
        public static Koma MotiKomaToKoma(MotiKoma mk)
        {
            return MotiKomasyuruiAndTaikyokusyaToKoma(MotiKomaToMotiKomasyrui(mk), MotiKomaToTaikyokusya(mk));
        }
        #endregion

        #region 持駒→駒種類
        static Komasyurui[] m_MotiKomaToKomasyurui_ =
        {
            // ぞう（対局者１、対局者２）
            Komasyurui.Z,
            Komasyurui.Z,

            // きりん
            Komasyurui.K,
            Komasyurui.K,

            // ひよこ
            Komasyurui.H,
            Komasyurui.H,

            // いぬ
            Komasyurui.I,
            Komasyurui.I,

            // ねこ
            Komasyurui.Neko,
            Komasyurui.Neko,

            // うさぎ
            Komasyurui.U,
            Komasyurui.U,

            // いのしし
            Komasyurui.S,
            Komasyurui.S,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            Komasyurui.Yososu,
        };
        public static Komasyurui MotiKomaToKomasyrui(MotiKoma mk)
        {
            return Med_Koma.m_MotiKomaToKomasyurui_[(int)mk];
        }
        #endregion

        #region 持駒→持駒種類
        static MotiKomasyurui[] m_MotiKomaToMotiKomasyurui_ =
        {
            // ぞう（対局者１、対局者２）
            MotiKomasyurui.Z,
            MotiKomasyurui.Z,

            // きりん
            MotiKomasyurui.K,
            MotiKomasyurui.K,

            // ひよこ
            MotiKomasyurui.H,
            MotiKomasyurui.H,

            // いぬ
            MotiKomasyurui.I,
            MotiKomasyurui.I,

            // ねこ
            MotiKomasyurui.Neko,
            MotiKomasyurui.Neko,

            // うさぎ
            MotiKomasyurui.U,
            MotiKomasyurui.U,

            // いのしし
            MotiKomasyurui.S,
            MotiKomasyurui.S,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            MotiKomasyurui.Yososu,
        };
        public static MotiKomasyurui MotiKomaToMotiKomasyrui(MotiKoma mk)
        {
            return Med_Koma.m_MotiKomaToMotiKomasyurui_[(int)mk];
        }
        #endregion

        #region 持駒→手番
        static Taikyokusya[] m_MotiKomaToTaikyokusya_ =
        {
            // ぞう（対局者１、対局者２）
            Taikyokusya.T1,
            Taikyokusya.T2,

            // きりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ひよこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いぬ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // うさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            Taikyokusya.Yososu,
        };
        public static Taikyokusya MotiKomaToTaikyokusya(MotiKoma mk)
        {
            return Med_Koma.m_MotiKomaToTaikyokusya_[(int)mk];
        }
        #endregion

        #region 持駒種類と手番→持駒
        static MotiKoma[,] m_MotiKomasyuruiAndPhaseToMotiKoma_ =
        {
            // ぞう
            { MotiKoma.Z, MotiKoma.z },

            // きりん
            { MotiKoma.K, MotiKoma.k },

            // ひよこ
            { MotiKoma.H, MotiKoma.h },

            // いぬ
            { MotiKoma.I, MotiKoma.i },

            // ねこ
            { MotiKoma.Neko, MotiKoma.neko },

            // うさぎ
            { MotiKoma.U, MotiKoma.u },

            // いのしし
            { MotiKoma.S, MotiKoma.s },

            // 要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { MotiKoma.Yososu, MotiKoma.Yososu },
        };
        public static MotiKoma MotiKomasyuruiAndPhaseToMotiKoma(MotiKomasyurui mks, Option<Phase> phase)
        {
            return m_MotiKomasyuruiAndPhaseToMotiKoma_[(int)mks, OptionalPhase.ToInt(phase)];
        }
        #endregion

        #region 持駒種類と手番→駒
        static Koma[,] m_MotiKomasyuruiAndTaikyokusyaToKoma_ =
        {
            // ぞう
            { Koma.Z, Koma.z },

            // きりん
            { Koma.K, Koma.k },

            // ひよこ
            { Koma.H, Koma.h },// にわとり　にはならないぜ☆（＾～＾）

            // いぬ
            { Koma.I, Koma.i },

            // ねこ
            { Koma.Neko, Koma.neko },

            // うさぎ
            { Koma.U, Koma.u },

            // いのしし
            { Koma.S, Koma.s },

            // 要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { Koma.Yososu, Koma.Yososu },
        };
        public static Koma MotiKomasyuruiAndTaikyokusyaToKoma(MotiKomasyurui mks, Taikyokusya tb)
        {
            return Med_Koma.m_MotiKomasyuruiAndTaikyokusyaToKoma_[(int)mks, (int)tb];
        }
        #endregion
    }
}

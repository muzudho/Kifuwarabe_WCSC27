namespace Grayscale.Kifuwarakei.Entities.Features
{
#if DEBUG
    using System;
    using System.Diagnostics;
    using System.Text;
    using Grayscale.Kifuwarakei.Entities.Logging;
#else
    using System;
    using System.Diagnostics;
#endif

    public class MotiKomaItiranImpl
    {
        public MotiKomaItiranImpl()
        {
            this.ValueMk = new int[(int)MotiKoma.Yososu];
        }
        public MotiKomaItiranImpl Clear()
        {
            Array.Clear(ValueMk, 0, ValueMk.Length);
            return this;
        }

        /// <summary>
        /// 持ち駒の数だぜ☆（＾▽＾）
        /// [持駒]
        /// </summary>
        int[] ValueMk { get; set; }

        public int GetArrayLength()
        {
            return ValueMk.Length;
        }
        public int Get(MotiKoma mk)
        {
            return ValueMk[(int)mk];
        }
        public MotiKomaItiranImpl Set(MotiKoma mk, int count)
        {
            ValueMk[(int)mk] = count;
#if DEBUG
            if (ValueMk[(int)mk] < 0)
            {
                StringBuilder reigai1 = new StringBuilder();
                reigai1.AppendLine("error 持駒の数にマイナスをセットした☆");
                Logger.Flush(reigai1);
                throw new Exception(reigai1.ToString());
            }
#endif

            return this;
        }
        /// <summary>
        /// 値渡し☆（＾～＾）
        /// </summary>
        /// <param name="copy"></param>
        /// <returns></returns>
        public MotiKomaItiranImpl Set(MotiKomaItiranImpl copy)
        {
            Debug.Assert(ValueMk.Length == copy.ValueMk.Length, "持ち駒配列の長さが違う☆");
            Array.Copy(copy.ValueMk, ValueMk, ValueMk.Length);
            return this;
        }
        public MotiKomaItiranImpl Add(MotiKoma mk, int count)
        {
            ValueMk[(int)mk] += count;
            return this;
        }
        public MotiKomaItiranImpl Fuyasu(MotiKoma mk)
        {
            ValueMk[(int)mk]++;
            return this;
        }
        public MotiKomaItiranImpl Herasu(MotiKoma mk)
        {
            ValueMk[(int)mk]--;
#if DEBUG
            if (ValueMk[(int)mk] < 0)
            {
                StringBuilder reigai1 = new StringBuilder();
                reigai1.AppendLine("error 持駒の数がマイナス");
                Logger.Flush(reigai1);
                throw new Exception(reigai1.ToString());
            }
#endif
            return this;
        }
        /// <summary>
        /// 持駒を持っているなら真☆
        /// </summary>
        /// <param name="mk"></param>
        /// <returns></returns>
        public bool HasMotiKoma(MotiKoma mk)
        {
            return 0 < ValueMk[(int)mk];
        }
        public bool IsEmpty()
        {
            for (int i = 0; i < ValueMk.Length; i++)
            {
                if (0 < ValueMk[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

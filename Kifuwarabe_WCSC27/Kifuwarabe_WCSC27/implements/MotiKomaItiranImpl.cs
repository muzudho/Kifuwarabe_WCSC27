﻿using kifuwarabe_wcsc27.interfaces;
using System;
using kifuwarabe_wcsc27.machine;
using System.Diagnostics;

namespace kifuwarabe_wcsc27.implements
{
    public class MotiKomaItiranImpl
    {
        public MotiKomaItiranImpl()
        {
            this.ValueMk = new int[(int)MotiKoma.Yososu];
        }
        public MotiKomaItiranImpl Clear()
        {
            Array.Clear(ValueMk,0,ValueMk.Length);
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
                Mojiretu reigai1 = new MojiretuImpl();
                reigai1.AppendLine("error 持駒の数にマイナスをセットした☆");
                Util_Machine.Flush(reigai1);
                throw new Exception(reigai1.ToContents());
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
            Debug.Assert(ValueMk.Length==copy.ValueMk.Length,"持ち駒配列の長さが違う☆");
            Array.Copy(copy.ValueMk,ValueMk,ValueMk.Length);
            return this;
        }
        public MotiKomaItiranImpl Add(MotiKoma mk, int count)
        {
            ValueMk[(int)mk] += count;
            return this;
        }
        public MotiKomaItiranImpl Fuyasu(MotiKoma mk)
        {
            ValueMk[(int)mk] ++;
            return this;
        }
        public MotiKomaItiranImpl Herasu(MotiKoma mk)
        {
            ValueMk[(int)mk]--;
#if DEBUG
            if (ValueMk[(int)mk] < 0)
            {
                Mojiretu reigai1 = new MojiretuImpl();
                reigai1.AppendLine("error 持駒の数がマイナス");
                Util_Machine.Flush(reigai1);
                throw new Exception(reigai1.ToContents());
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
            for (int i=0; i<ValueMk.Length;i++)
            {
                if (0<ValueMk[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

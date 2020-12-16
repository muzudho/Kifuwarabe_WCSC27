using System.Text;
using System;
using kifuwarabe_wcsc27.interfaces;

namespace kifuwarabe_wcsc27.implements
{
    /// <summary>
    /// Unity の C# はバージョンが古いのか、StringBuilderに使えないメソッドがあるのでラッピングしておくぜ☆（＾～＾）
    /// </summary>
    public class MojiretuImpl : Mojiretu
    {
        public MojiretuImpl()
        {
            this.m_str_ = new StringBuilder();
        }

        StringBuilder m_str_;

        public bool IsHataraku { get { return true; } }

        public void Append(string val) { this.m_str_.Append(val); }
        public void Append(int val) { this.m_str_.Append(val); }
        public void Append(uint val) { this.m_str_.Append(val); }
        public void Append(long val) { this.m_str_.Append(val); }
        public void Append(ulong val) { this.m_str_.Append(val); }
        public void Append(float val) { this.m_str_.Append(val); }
        public void Append(double val) { this.m_str_.Append(val); }
        public void Append(bool val) { this.m_str_.Append(val); }

        public void AppendLine(string val) { this.m_str_.AppendLine(val); }
        public void AppendLine(int val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(uint val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(long val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(ulong val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(float val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(double val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine(bool val) { this.m_str_.AppendLine(val.ToString()); }
        public void AppendLine() { this.m_str_.AppendLine(); }

        public void Clear()
        {
#if UNITY
            this.m_str_.Length = 0;
#else
            this.m_str_.Clear();
#endif
        }

        public int Length
        {
            get
            {
                return this.m_str_.Length;
            }
        }

        public void Insert(int nanmojime, string mojiretu)
        {
            this.m_str_.Insert( nanmojime, mojiretu);
        }

        public string ToContents()
        {
            return this.m_str_.ToString();
        }

        public override string ToString()
        {
            throw new Exception("ToString ではなく ToContents を使えだぜ☆（＞＿＜）");
        }
    }

    /// <summary>
    /// 仕事をしないクラスだぜ☆（＾▽＾）
    /// </summary>
    public class KarappoMojiretuImpl : Mojiretu
    {
        public KarappoMojiretuImpl()
        {
        }

        public bool IsHataraku { get { return false; } }

        public void Append(string val) {}
        public void Append(int val) {}
        public void Append(uint val) {}
        public void Append(long val) {}
        public void Append(ulong val) {}
        public void Append(float val) {}
        public void Append(double val) {}
        public void Append(bool val) {}

        public void AppendLine(string val) {}
        public void AppendLine(int val) {}
        public void AppendLine(uint val) {}
        public void AppendLine(long val) {}
        public void AppendLine(ulong val) {}
        public void AppendLine(float val) {}
        public void AppendLine(double val) {}
        public void AppendLine(bool val) {}
        public void AppendLine(){}

        public void Clear(){}

        public int Length { get { return 0; } }

        public void Insert(int nanmojime, string mojiretu) { }

        public string ToContents() { return ""; }

        public override string ToString()
        {
            throw new Exception("ToString ではなく ToContents を使えだぜ☆（＞＿＜）");
        }
    }
}

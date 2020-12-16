namespace kifuwarabe_wcsc27.interfaces
{
    public interface Mojiretu
    {
        bool IsHataraku { get; }

        void Append(string val);
        void Append(int val);
        void Append(uint val);
        void Append(long val);
        void Append(ulong val);
        void Append(float val);
        void Append(double val);
        void Append(bool val);

        void AppendLine(string val);
        void AppendLine(int val);
        void AppendLine(uint val);
        void AppendLine(long val);
        void AppendLine(ulong val);
        void AppendLine(float val);
        void AppendLine(double val);
        void AppendLine(bool val);

        void AppendLine();

        void Clear();

        int Length { get; }

        void Insert(int nanmojime, string mojiretu);

        string ToContents();
    }
}

using Grayscale.Kifuwarakei.Entities.Language;
using System;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public static class OptionalPhase
    {
        static OptionalPhase()
        {
            Black = new Option<Phase>(Phase.Black);
            White = new Option<Phase>(Phase.White);
        }

        public static readonly Option<Phase> Black;
        public static readonly Option<Phase> White;

        public static Option<Phase> From(int tai)
        {
            return From((Taikyokusya)tai);
        }
        public static Option<Phase> From(Taikyokusya tai)
        {
            switch (tai)
            {
                case Taikyokusya.T1:
                    return OptionalPhase.Black;
                case Taikyokusya.T2:
                    return OptionalPhase.White;
                case Taikyokusya.Yososu:
                    return Option<Phase>.None;
                default:
                    throw new Exception($"tai={tai} is fail.");
            }
        }
        public static int ToInt(Option<Phase> optionalPhase)
        {
            var (exists, phase) = optionalPhase.Match;
            if (exists)
            {
                return (int)phase;
            }
            else
            {
                return 2;
            }
        }
    }
}

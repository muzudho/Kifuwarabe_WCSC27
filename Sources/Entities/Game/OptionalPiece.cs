using Grayscale.Kifuwarakei.Entities.Language;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public static class OptionalPiece
    {
        /*
        static OptionalPiece()
        {
            King1 = new Option<Piece>(Piece.R);
            King2 = new Option<Piece>(Piece.r);
            Bishop1 = new Option<Piece>(Piece.Z);
            Bishop2 = new Option<Piece>(Piece.z);
        }


        public static readonly Option<Piece> King1;
        public static readonly Option<Piece> King2;
        public static readonly Option<Piece> Bishop1;
        public static readonly Option<Piece> Bishop2;

        public static Option<Phase> Some(Phase phase)
        {
            switch (phase)
            {
                case Phase.Black:
                    return OptionalPhase.Black;
                case Phase.White:
                    return OptionalPhase.White;
                default:
                    throw new Exception($"phase={phase} is fail.");
            }
        }
        public static Option<Phase> From(int phaseIndex)
        {
            switch (phaseIndex)
            {
                case 0:
                    return OptionalPhase.Black;
                case 1:
                    return OptionalPhase.White;
                case 2:
                    return Option<Phase>.None;
                default:
                    throw new Exception($"phaseIndex={phaseIndex} is fail.");
            }
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
        public static int IndexOf(Option<Phase> optionalPhase)
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
        public static Taikyokusya ToTaikyokusya(Option<Phase> optionalPhase)
        {
            var (exists, phase) = optionalPhase.Match;
            if (exists)
            {
                switch (phase)
                {
                    case Phase.Black: return Taikyokusya.T1;
                    case Phase.White: return Taikyokusya.T2;
                    default: throw new Exception($"optionalPhase={phase} is fail.");
                }
            }
            else
            {
                return Taikyokusya.Yososu;
            }
        }
        */
    }
}

using Grayscale.Kifuwarakei.Entities.Language;
using System;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public static class OptionalPiece
    {
        static OptionalPiece()
        {
            King1 = new Option<Piece>(Piece.King1);
            King2 = new Option<Piece>(Piece.King2);
            Bishop1 = new Option<Piece>(Piece.Bishop1);
            Bishop2 = new Option<Piece>(Piece.Bishop2);
            ProBishop1 = new Option<Piece>(Piece.ProBishop1);
            ProBishop2 = new Option<Piece>(Piece.ProBishop2);
            Rook1 = new Option<Piece>(Piece.Rook1);
            Rook2 = new Option<Piece>(Piece.Rook2);
            ProRook1 = new Option<Piece>(Piece.ProRook1);
            ProRook2 = new Option<Piece>(Piece.ProRook2);
            Pawn1 = new Option<Piece>(Piece.Pawn1);
            Pawn2 = new Option<Piece>(Piece.Pawn2);
            ProPawn1 = new Option<Piece>(Piece.ProPawn1);
            ProPawn2 = new Option<Piece>(Piece.ProPawn2);
            Gold1 = new Option<Piece>(Piece.Gold1);
            Gold2 = new Option<Piece>(Piece.Gold2);
            Silver1 = new Option<Piece>(Piece.Silver1);
            Silver2 = new Option<Piece>(Piece.Silver2);
            ProSilver1 = new Option<Piece>(Piece.ProSilver1);
            ProSilver2 = new Option<Piece>(Piece.ProSilver2);
            Knight1 = new Option<Piece>(Piece.Knight1);
            Knight2 = new Option<Piece>(Piece.Knight2);
            ProKnight1 = new Option<Piece>(Piece.ProKnight1);
            ProKnight2 = new Option<Piece>(Piece.ProKnight2);
            Lance1 = new Option<Piece>(Piece.Lance1);
            Lance2 = new Option<Piece>(Piece.Lance2);
            ProLance1 = new Option<Piece>(Piece.ProLance1);
            ProLance2 = new Option<Piece>(Piece.ProLance2);
        }

        public static readonly Option<Piece> King1;
        public static readonly Option<Piece> King2;
        public static readonly Option<Piece> Bishop1;
        public static readonly Option<Piece> Bishop2;
        public static readonly Option<Piece> ProBishop1;
        public static readonly Option<Piece> ProBishop2;
        public static readonly Option<Piece> Rook1;
        public static readonly Option<Piece> Rook2;
        public static readonly Option<Piece> ProRook1;
        public static readonly Option<Piece> ProRook2;
        public static readonly Option<Piece> Pawn1;
        public static readonly Option<Piece> Pawn2;
        public static readonly Option<Piece> ProPawn1;
        public static readonly Option<Piece> ProPawn2;
        public static readonly Option<Piece> Gold1;
        public static readonly Option<Piece> Gold2;
        public static readonly Option<Piece> Silver1;
        public static readonly Option<Piece> Silver2;
        public static readonly Option<Piece> ProSilver1;
        public static readonly Option<Piece> ProSilver2;
        public static readonly Option<Piece> Knight1;
        public static readonly Option<Piece> Knight2;
        public static readonly Option<Piece> ProKnight1;
        public static readonly Option<Piece> ProKnight2;
        public static readonly Option<Piece> Lance1;
        public static readonly Option<Piece> Lance2;
        public static readonly Option<Piece> ProLance1;
        public static readonly Option<Piece> ProLance2;

        public static Option<Piece> Some(Piece piece)
        {
            switch (piece)
            {
                case Piece.King1: return OptionalPiece.King1;
                case Piece.King2: return OptionalPiece.King2;
                case Piece.Bishop1: return OptionalPiece.Bishop1;
                case Piece.Bishop2: return OptionalPiece.Bishop2;
                case Piece.ProBishop1: return OptionalPiece.ProBishop1;
                case Piece.ProBishop2: return OptionalPiece.ProBishop2;
                case Piece.Rook1: return OptionalPiece.Rook1;
                case Piece.Rook2: return OptionalPiece.Rook2;
                case Piece.ProRook1: return OptionalPiece.ProRook1;
                case Piece.ProRook2: return OptionalPiece.ProRook2;
                case Piece.Pawn1: return OptionalPiece.Pawn1;
                case Piece.Pawn2: return OptionalPiece.Pawn2;
                case Piece.ProPawn1: return OptionalPiece.ProPawn1;
                case Piece.ProPawn2: return OptionalPiece.ProPawn2;
                case Piece.Gold1: return OptionalPiece.Gold1;
                case Piece.Gold2: return OptionalPiece.Gold2;
                case Piece.Silver1: return OptionalPiece.Silver1;
                case Piece.Silver2: return OptionalPiece.Silver2;
                case Piece.ProSilver1: return OptionalPiece.ProSilver1;
                case Piece.ProSilver2: return OptionalPiece.ProSilver2;
                case Piece.Knight1: return OptionalPiece.Knight1;
                case Piece.Knight2: return OptionalPiece.Knight2;
                case Piece.ProKnight1: return OptionalPiece.ProKnight1;
                case Piece.ProKnight2: return OptionalPiece.ProKnight2;
                case Piece.Lance1: return OptionalPiece.Lance1;
                case Piece.Lance2: return OptionalPiece.Lance2;
                case Piece.ProLance1: return OptionalPiece.ProLance1;
                case Piece.ProLance2: return OptionalPiece.ProLance2;
                default: throw new Exception($"piece={piece} is fail.");
            }
        }

        public static Option<Piece> From(Koma km)
        {
            switch (km)
            {
                case Koma.King1: return OptionalPiece.King1;
                case Koma.King2: return OptionalPiece.King2;
                case Koma.Bishop1: return OptionalPiece.Bishop1;
                case Koma.Bishop2: return OptionalPiece.Bishop2;
                case Koma.ProBishop1: return OptionalPiece.ProBishop1;
                case Koma.ProBishop2: return OptionalPiece.ProBishop2;
                case Koma.Rook1: return OptionalPiece.Rook1;
                case Koma.Rook2: return OptionalPiece.Rook2;
                case Koma.ProRook1: return OptionalPiece.ProRook1;
                case Koma.ProRook2: return OptionalPiece.ProRook2;
                case Koma.Pawn1: return OptionalPiece.Pawn1;
                case Koma.Pawn2: return OptionalPiece.Pawn2;
                case Koma.ProPawn1: return OptionalPiece.ProPawn1;
                case Koma.ProPawn2: return OptionalPiece.ProPawn2;
                case Koma.Gold1: return OptionalPiece.Gold1;
                case Koma.Gold2: return OptionalPiece.Gold2;
                case Koma.Silver1: return OptionalPiece.Silver1;
                case Koma.Silver2: return OptionalPiece.Silver2;
                case Koma.ProSilver1: return OptionalPiece.ProSilver1;
                case Koma.ProSilver2: return OptionalPiece.ProSilver2;
                case Koma.Knight1: return OptionalPiece.Knight1;
                case Koma.Knight2: return OptionalPiece.Knight2;
                case Koma.ProKnight1: return OptionalPiece.ProKnight1;
                case Koma.ProKnight2: return OptionalPiece.ProKnight2;
                case Koma.Lance1: return OptionalPiece.Lance1;
                case Koma.Lance2: return OptionalPiece.Lance2;
                case Koma.ProLance1: return OptionalPiece.ProLance1;
                case Koma.ProLance2: return OptionalPiece.ProLance2;
                case Koma.SpaceSq: return Option<Piece>.None;
                case Koma.PieceNum: return Option<Piece>.None;
                default: throw new Exception($"km={km} is fail.");
            }
        }

        public static Option<Piece> From(int phaseIndex)
        {
            switch (phaseIndex)
            {
                case 0: return OptionalPiece.King1;
                case 1: return OptionalPiece.King2;
                case 2: return OptionalPiece.Bishop1;
                case 3: return OptionalPiece.Bishop2;
                case 4: return OptionalPiece.ProBishop1;
                case 5: return OptionalPiece.ProBishop2;
                case 6: return OptionalPiece.Rook1;
                case 7: return OptionalPiece.Rook2;
                case 8: return OptionalPiece.ProRook1;
                case 9: return OptionalPiece.ProRook2;
                case 10: return OptionalPiece.Pawn1;
                case 11: return OptionalPiece.Pawn2;
                case 12: return OptionalPiece.ProPawn1;
                case 13: return OptionalPiece.ProPawn2;
                case 14: return OptionalPiece.Gold1;
                case 15: return OptionalPiece.Gold2;
                case 16: return OptionalPiece.Silver1;
                case 17: return OptionalPiece.Silver2;
                case 18: return OptionalPiece.ProSilver1;
                case 19: return OptionalPiece.ProSilver2;
                case 20: return OptionalPiece.Knight1;
                case 21: return OptionalPiece.Knight2;
                case 22: return OptionalPiece.ProKnight1;
                case 23: return OptionalPiece.ProKnight2;
                case 24: return OptionalPiece.Lance1;
                case 25: return OptionalPiece.Lance2;
                case 26: return OptionalPiece.ProLance1;
                case 27: return OptionalPiece.ProLance2;
                default: throw new Exception($"phaseIndex={phaseIndex} is fail.");
            }
        }

        public static int IndexOf(Option<Piece> optionalPiece)
        {
            var (exists, piece) = optionalPiece.Match;
            if (exists)
            {
                return (int)piece;
            }
            else
            {
                return 28; // または 29
            }
        }


        /*

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

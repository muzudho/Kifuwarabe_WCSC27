using Grayscale.Kifuwarakei.Entities.Language;
using System;
using Grayscale.Kifuwarakei.Entities.Take1Base;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public static class OptionalPiece
    {
        static OptionalPiece()
        {
            King1 = new Option<Piece>(Piece.K1);
            King2 = new Option<Piece>(Piece.K2);
            Bishop1 = new Option<Piece>(Piece.B1);
            Bishop2 = new Option<Piece>(Piece.B2);
            ProBishop1 = new Option<Piece>(Piece.PB1);
            ProBishop2 = new Option<Piece>(Piece.PB2);
            Rook1 = new Option<Piece>(Piece.R1);
            Rook2 = new Option<Piece>(Piece.R2);
            ProRook1 = new Option<Piece>(Piece.PR1);
            ProRook2 = new Option<Piece>(Piece.PR2);
            Pawn1 = new Option<Piece>(Piece.P1);
            Pawn2 = new Option<Piece>(Piece.P2);
            ProPawn1 = new Option<Piece>(Piece.PP1);
            ProPawn2 = new Option<Piece>(Piece.PP2);
            Gold1 = new Option<Piece>(Piece.G1);
            Gold2 = new Option<Piece>(Piece.G2);
            Silver1 = new Option<Piece>(Piece.S1);
            Silver2 = new Option<Piece>(Piece.S2);
            ProSilver1 = new Option<Piece>(Piece.PS1);
            ProSilver2 = new Option<Piece>(Piece.PS2);
            Knight1 = new Option<Piece>(Piece.N1);
            Knight2 = new Option<Piece>(Piece.N2);
            ProKnight1 = new Option<Piece>(Piece.PN1);
            ProKnight2 = new Option<Piece>(Piece.PN2);
            Lance1 = new Option<Piece>(Piece.L1);
            Lance2 = new Option<Piece>(Piece.L2);
            ProLance1 = new Option<Piece>(Piece.PL1);
            ProLance2 = new Option<Piece>(Piece.PL2);
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
                case Piece.K1: return OptionalPiece.King1;
                case Piece.K2: return OptionalPiece.King2;
                case Piece.B1: return OptionalPiece.Bishop1;
                case Piece.B2: return OptionalPiece.Bishop2;
                case Piece.PB1: return OptionalPiece.ProBishop1;
                case Piece.PB2: return OptionalPiece.ProBishop2;
                case Piece.R1: return OptionalPiece.Rook1;
                case Piece.R2: return OptionalPiece.Rook2;
                case Piece.PR1: return OptionalPiece.ProRook1;
                case Piece.PR2: return OptionalPiece.ProRook2;
                case Piece.P1: return OptionalPiece.Pawn1;
                case Piece.P2: return OptionalPiece.Pawn2;
                case Piece.PP1: return OptionalPiece.ProPawn1;
                case Piece.PP2: return OptionalPiece.ProPawn2;
                case Piece.G1: return OptionalPiece.Gold1;
                case Piece.G2: return OptionalPiece.Gold2;
                case Piece.S1: return OptionalPiece.Silver1;
                case Piece.S2: return OptionalPiece.Silver2;
                case Piece.PS1: return OptionalPiece.ProSilver1;
                case Piece.PS2: return OptionalPiece.ProSilver2;
                case Piece.N1: return OptionalPiece.Knight1;
                case Piece.N2: return OptionalPiece.Knight2;
                case Piece.PN1: return OptionalPiece.ProKnight1;
                case Piece.PN2: return OptionalPiece.ProKnight2;
                case Piece.L1: return OptionalPiece.Lance1;
                case Piece.L2: return OptionalPiece.Lance2;
                case Piece.PL1: return OptionalPiece.ProLance1;
                case Piece.PL2: return OptionalPiece.ProLance2;
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
                return 29; // または 28
            }
        }

        public static Koma ToKoma(Option<Piece> optionalPiece)
        {
            var (exists, piece) = optionalPiece.Match;
            if (exists)
            {
                switch (piece)
                {
                    case Piece.K1: return Koma.King1;
                    case Piece.K2: return Koma.King2;
                    case Piece.B1: return Koma.Bishop1;
                    case Piece.B2: return Koma.Bishop2;
                    case Piece.PB1: return Koma.ProBishop1;
                    case Piece.PB2: return Koma.ProBishop2;
                    case Piece.R1: return Koma.Rook1;
                    case Piece.R2: return Koma.Rook2;
                    case Piece.PR1: return Koma.ProRook1;
                    case Piece.PR2: return Koma.ProRook2;
                    case Piece.P1: return Koma.Pawn1;
                    case Piece.P2: return Koma.Pawn2;
                    case Piece.PP1: return Koma.ProPawn1;
                    case Piece.PP2: return Koma.ProPawn2;
                    case Piece.G1: return Koma.Gold1;
                    case Piece.G2: return Koma.Gold2;
                    case Piece.S1: return Koma.Silver1;
                    case Piece.S2: return Koma.Silver2;
                    case Piece.PS1: return Koma.ProSilver1;
                    case Piece.PS2: return Koma.ProSilver2;
                    case Piece.N1: return Koma.Knight1;
                    case Piece.N2: return Koma.Knight2;
                    case Piece.PN1: return Koma.ProKnight1;
                    case Piece.PN2: return Koma.ProKnight2;
                    case Piece.L1: return Koma.Lance1;
                    case Piece.L2: return Koma.Lance2;
                    case Piece.PL1: return Koma.ProLance1;
                    case Piece.PL2: return Koma.ProLance2;
                    default: throw new Exception($"piece={piece} is fail.");
                }
            }
            else
            {
                return Koma.PieceNum;
            }
        }
    }
}

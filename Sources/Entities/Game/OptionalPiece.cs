﻿using Grayscale.Kifuwarakei.Entities.Language;
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

        /*

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
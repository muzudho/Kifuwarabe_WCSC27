using System;

namespace Grayscale.Kifuwarakei.Entities.Game
{
    public class OptionalPhase
    {
        static OptionalPhase()
        {
            none_ = new OptionalPhase(false, Phase.Black); // このブラックの値は使わせないぜ☆（＾～＾）
            black_ = new OptionalPhase(true, Phase.Black);
            white_ = new OptionalPhase(true, Phase.White);
        }
        static readonly OptionalPhase black_;
        static readonly OptionalPhase white_;
        static readonly OptionalPhase none_;

        public static OptionalPhase None
        {
            get
            {
                return none_;
            }
        }
        public static OptionalPhase Black
        {
            get
            {
                return black_;
            }
        }
        public static OptionalPhase White
        {
            get
            {
                return white_;
            }
        }

        public static OptionalPhase Some(Phase phase)
        {
            switch (phase)
            {
                case Phase.Black:
                    return OptionalPhase.Black;
                case Phase.White:
                    return OptionalPhase.White;
                default:
                    throw new Exception($"phase={phase}");
            }
        }

        private OptionalPhase(bool isNone, Phase phase)
        {
            this.isNone_ = isNone;
            this.phase_ = phase;
        }

        public (bool, Phase) Match
        {
            get
            {
                if (this.isNone_)
                {
                    return (false, Phase.Black); // このブラックの値は使ってはいけないぜ☆（＾～＾）
                }
                return (true, this.phase_);
            }
        }

        public Phase Unwrap()
        {
            if (this.isNone_)
            {
                throw new Exception("Unwrap fail. Phase is none.");
            }
            return this.phase_;
        }

        public bool isNone_;
        Phase phase_;
    }
}

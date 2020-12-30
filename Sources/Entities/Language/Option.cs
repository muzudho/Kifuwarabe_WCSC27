using System;

namespace Grayscale.Kifuwarakei.Entities.Language
{
    public class Option<T>
    {
        public static Option<T> None
        {
            get
            {
                return new Option<T>();
            }
        }

        Option()
        {
        }

        public Option(T item)
        {
            this.Item = item;
        }

        T Item { get; set; }

        public T Unwrap()
        {
            if (this.Item == null)
            {
                throw new Exception("This is none.");
            }
            return this.Item;
        }

        public (bool, T) Match
        {
            get
            {
                if (this.Item == null)
                {
                    return (false, this.Item);
                }
                return (true, this.Item);
            }
        }
    }
}

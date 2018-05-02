using System;

namespace FiatSharp
{
    public class FutureMove<T>
    {
        public T Move { get; set; }
        public DateTime DateTime { get; set; }
    }
}
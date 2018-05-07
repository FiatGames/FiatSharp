using System;

namespace FiatSharp.Models
{
    public class FutureMove<T>
    {
        public T Move { get; set; }
        public DateTime DateTime { get; set; }
    }
}
using System;

namespace MachinaWrapper.Common
{
    public class NoRegionException : Exception
    {
        public NoRegionException() { }
        public NoRegionException(string message) : base(message) { }
        public NoRegionException(string message, Exception inner) : base(message, inner) { }
    }

    public enum Region : byte
    {
        Global,
        KR,
        CN
    }
}
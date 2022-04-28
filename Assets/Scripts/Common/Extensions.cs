using System;

namespace Common
{
    public static class Extensions
    {
        public static bool IsApproximateZero(this float number)
        {
            return Math.Abs(number) < 0.001d;
        }
    }
}
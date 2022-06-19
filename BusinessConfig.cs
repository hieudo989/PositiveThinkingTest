using System;

namespace PapayaTest
{
    public static class BusinessConfig
    {
        public static readonly TimeSpan endBookTime = new(16, 00, 00);
        public static readonly TimeSpan startBookTime = new(9, 00, 00);
        public static readonly int meetingTime = 60;
        public static readonly int maxSimultaneousSettlements = 4;
    }
}

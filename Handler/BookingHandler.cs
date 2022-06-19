using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PapayaTest.Controllers
{

    public class BookingHandler
    {
        private static ConcurrentDictionary<int, List<Tuple<string, Guid>>> businessTimeDict;
        static BookingHandler()
        {
            int nmbrOfMinutes = (int) BusinessConfig.endBookTime.Subtract(BusinessConfig.startBookTime).TotalMinutes;
            businessTimeDict = new ConcurrentDictionary<int, List<Tuple<string, Guid>>>(4, nmbrOfMinutes);
            for (int i = 0; i < nmbrOfMinutes; i++)
            {
                businessTimeDict[i] = new List<Tuple<string,Guid>>();
            }
        }
        internal Guid CreateBooking(string bookerName,TimeSpan inputTime)
        {
            int inputTimeSlot = TimespanToMinuteFromStartTime(inputTime);
            int endInputSlot = inputTimeSlot + BusinessConfig.meetingTime;
            Guid newGuid = Guid.NewGuid();
            for (int i = inputTimeSlot; i < endInputSlot; i++)
            {
                if (businessTimeDict.TryGetValue(i, out List<Tuple<string, Guid>> bookers))
                {
                    if (bookers.Count == BusinessConfig.maxSimultaneousSettlements) return Guid.Empty;
                    foreach (var booker in bookers)
                    {
                        if (bookerName.Equals(booker.Item1))
                        {
                            return Guid.Empty;
                        }
                    }                    
                    bookers.Add(new Tuple<string, Guid>(bookerName, newGuid));                   
                }
            }

            return newGuid;

        }
        // already filtered out of businessTime @controller
        private static int TimespanToMinuteFromStartTime(TimeSpan inputTime)
        {
            return (int) inputTime.Subtract(BusinessConfig.startBookTime).TotalMinutes;
        }
    }
}

using System;

namespace PapayaTest.Response
{
    public class BookingResponse
    {
        public BookingResponse(Guid bookingID)
        {
            bookingId = bookingID;
        }

        public Guid bookingId { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using PapayaTest.Command;
using PapayaTest.Response;
using System;

namespace PapayaTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
        
        private readonly BookingHandler _handler;
        public BookingController(BookingHandler handler)
        {
            _handler = handler;
        }
        [HttpPost]
        public IActionResult Create(CreateBookingCommand booking)
        {
            TimeSpan inTime;
            IActionResult res = ValidateBookingFormat(booking,out inTime);
            if (res is OkResult)
            {
                Guid bookingID = _handler.CreateBooking(booking.name, inTime);
                if (bookingID != Guid.Empty) return Ok(new BookingResponse(bookingID));
                else return Conflict();
            }
            return res;

        }
        private IActionResult ValidateBookingFormat(CreateBookingCommand booking,out TimeSpan inTime)
        {
            inTime = default;
            if (booking is null) return BadRequest();
            if (string.IsNullOrEmpty(booking.name)) return BadRequest();
            if (!TimeSpan.TryParseExact(booking.bookingTime, "hh\\:mm", null, out TimeSpan tempTimespan))
            {
                return BadRequest();
            }
            if (tempTimespan.CompareTo(BusinessConfig.endBookTime) > 0 || tempTimespan.CompareTo(BusinessConfig.startBookTime)< 0)  return BadRequest(); 
            inTime = tempTimespan;
            return Ok();
        }
    }
}

using BookingHotel.Models;
using BookingHotel.Services;
using Microsoft.AspNetCore.Mvc;


namespace BookingHotel.Controllers
{
    public class HotelController : Controller
    {
        private readonly BookingApiService _bookingApiService;

        public HotelController(BookingApiService bookingApiService)
        {
            _bookingApiService = bookingApiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HotelSearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(HotelSearchModel model)
        {
            if (ModelState.IsValid)
            {
                // Önce şehirden destinasyon ID'si al
                var destId = await _bookingApiService.GetDestinationIdAsync(model.City.Trim());
                if (string.IsNullOrEmpty(destId))
                {
                    // Hata veya bulunamadıysa boş liste dön
                    return View("SearchResults", new List<HotelModel>());
                }
                // Sonra bu ID ile otel ara
                var hotels = await _bookingApiService.SearchHotelsByDestIdAsync(destId, model.CheckInDate, model.CheckOutDate, model.Adults);
                return View("SearchResults", hotels);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetHotelDetail(string hotelId)
        {
            if (string.IsNullOrEmpty(hotelId))
                return BadRequest("HotelId is required");

            var hotelDetail = await _bookingApiService.GetHotelDetailAsync(hotelId);

            if (hotelDetail == null)
                return StatusCode(500, "API'den veri alınamadı veya otel bulunamadı");

            return View("GetHotelDetail", hotelDetail);
        }
        public IActionResult View()
        {
            return View();
        }

    }
}

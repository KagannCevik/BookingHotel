using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.ViewComponents
{
    public class SearchComponentPartiaL:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new BookingHotel.Models.HotelSearchModel());
        }
    }
   
}

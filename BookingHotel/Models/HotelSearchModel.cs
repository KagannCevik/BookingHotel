namespace BookingHotel.Models
{
    public class HotelSearchModel
    {

        public string City { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
    }
}

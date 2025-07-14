using BookingHotel.Models;
using System.Text.Json;

namespace BookingHotel.Services
{
    public class BookingApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BookingApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "2522808563msh5677c580366a3dap131bf8jsn099ac7e72c0f");
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "booking-com15.p.rapidapi.com");
        }

        // 1. Şehirden destinasyon ID'si bulma
        public async Task<string> GetDestinationIdAsync(string city)
        {
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={Uri.EscapeDataString(city)}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            //System.IO.File.WriteAllText("api_response.json", content);
            //Console.WriteLine("Hotel Search API Response: " + content);

            using var doc = JsonDocument.Parse(content);
            var data = doc.RootElement.GetProperty("data");
            foreach (var item in data.EnumerateArray())
            {
                if (item.GetProperty("type").GetString() == "ci") // city
                {
                    return item.GetProperty("dest_id").GetString();
                }
            }
            return null;
        }


        public async Task<List<HotelModel>> SearchHotelsByDestIdAsync(string destId, DateTime checkIn, DateTime checkOut, int adults)
        {

            //checkIn = new DateTime(2025, 07, 20);
            //checkOut = new DateTime(2025, 07, 23);

             var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type=city&arrival_date={checkIn:yyyy-MM-dd}&departure_date={checkOut:yyyy-MM-dd}&adults_number={adults}&room_number=1&order_by=popularity&locale=en&currency=USD";



            Console.WriteLine("Deneme URL: " + url); // 

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Hatası: {response.StatusCode}");
                return new List<HotelModel>();
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Hotel Search API Response: " + content);

            var hotels = new List<HotelModel>();
            using (var doc = JsonDocument.Parse(content))
            {
                if (doc.RootElement.TryGetProperty("data", out var data) && data.TryGetProperty("hotels", out var hotelsArray))
                {
                    foreach (var hotel in hotelsArray.EnumerateArray())
                    {
                        var property = hotel.GetProperty("property");
                        hotels.Add(new HotelModel
                        {
                            HotelId = hotel.GetProperty("hotel_id").ToString(),
                            Name = property.GetProperty("name").GetString(),
                            Address = property.TryGetProperty("wishlistName", out var wishlist) ? wishlist.GetString() : "",
                            City = property.TryGetProperty("wishlistName", out var city) ? city.GetString() : "",
                            Country = property.TryGetProperty("countryCode", out var country) ? country.GetString() : "",
                            Rating = property.TryGetProperty("reviewScore", out var rating) ? rating.GetDouble() : 0,
                            ImageUrl = property.TryGetProperty("photoUrls", out var photos) && photos.GetArrayLength() > 0 ? photos[0].GetString() : "",
                            //Price = property.TryGetProperty("priceBreakdown", out var priceBreakdown) && priceBreakdown.TryGetProperty("grossPrice", out var grossPrice) && grossPrice.TryGetProperty("value", out var priceValue) ? (decimal)priceValue.GetDouble() : 0,
                             Price = property.TryGetProperty("priceBreakdown", out var priceBreakdown)
                            && priceBreakdown.TryGetProperty("grossPrice", out var grossPrice)
                            && grossPrice.TryGetProperty("value", out var priceValue)
                            ? Math.Round((decimal)priceValue.GetDouble(), 2)
    :                            0,
                            Currency = property.TryGetProperty("priceBreakdown", out var pb) && pb.TryGetProperty("grossPrice", out var gp) && gp.TryGetProperty("currency", out var curr) ? curr.GetString() : "",
                            Description = hotel.TryGetProperty("accessibilityLabel", out var desc) ? desc.GetString() : ""
                        });
                    }
                }
            }

            return hotels;
        }
        private string GetStringSafe(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return null;

            return prop.ValueKind switch
            {
                JsonValueKind.String => prop.GetString(),
                JsonValueKind.Number => prop.GetRawText(),
                _ => null
            };
        }
        public HotelModel ParseHotelDetailFromJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("data", out var data))
                return null;

            var hotelRoot = data;

            var hotel = new HotelModel();

            hotel.HotelId = GetStringSafe(hotelRoot, "hotel_id");
            hotel.Description = hotelRoot.TryGetProperty("accessibilityLabel", out var desc) ? desc.GetString() : "";

            if (hotelRoot.TryGetProperty("property", out var property))
            {
                hotel.Name = GetStringSafe(property, "name");
                hotel.Address = GetStringSafe(property, "address");
                hotel.City = GetStringSafe(property, "city");
                hotel.Country = GetStringSafe(property, "countryCode");
                hotel.Rating = property.TryGetProperty("reviewScore", out var rating) && rating.ValueKind == JsonValueKind.Number ? rating.GetDouble() : 0;
                hotel.ImageUrl = property.TryGetProperty("photoUrls", out var photos) && photos.GetArrayLength() > 0 ? photos[0].GetString() : "";
                if (property.TryGetProperty("priceBreakdown", out var pb) &&
                    pb.TryGetProperty("grossPrice", out var gp))
                {
                    hotel.Price = gp.TryGetProperty("value", out var val) && val.ValueKind == JsonValueKind.Number ? (decimal)val.GetDouble() : 0;
                    hotel.Currency = gp.TryGetProperty("currency", out var cur) ? cur.GetString() : "";
                }
            }

            return hotel;
        }
        public async Task<HotelDetailViewModel?> GetHotelDetailAsync(string hotelId)
        {
            var fullPath = @"C:\\Users\\ACER\\Desktop\\6 Proje Booking Case\\BookingHotel\\BookingHotel\\Models\\HotelDetail.json";
            if (!File.Exists(fullPath))
                return null;

            var json = await File.ReadAllTextAsync(fullPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("data", out var data))
                return null;

            var vm = new HotelDetailViewModel
            {
                HotelId = data.GetProperty("hotel_id").GetInt32(),
                Name = data.GetProperty("hotel_name").GetString(),
                Address = data.GetProperty("address").GetString(),
                City = data.GetProperty("city").GetString(),
                CountryCode = data.GetProperty("countrycode").GetString(),
                Description = data.TryGetProperty("description", out var desc) ? desc.GetString() : null,
                Latitude = data.TryGetProperty("latitude", out var lat) ? (float?)lat.GetDouble() : null,
                Longitude = data.TryGetProperty("longitude", out var lng) ? (float?)lng.GetDouble() : null,
                ReviewScore = data.TryGetProperty("review_nr", out var reviewScore) ? (float?)reviewScore.GetInt32() : null,
                ReviewCount = data.TryGetProperty("review_nr", out var reviewCount) ? (int?)reviewCount.GetInt32() : null,
                ReviewScoreWord = data.TryGetProperty("reviewScoreWord", out var reviewWord) ? reviewWord.GetString() : null,
                Url = data.TryGetProperty("url", out var url) ? url.GetString() : null,
                PhotoUrls = data.TryGetProperty("photoUrls", out var photos) && photos.ValueKind == JsonValueKind.Array ? photos.EnumerateArray().Select(p => p.GetString() ?? string.Empty).Where(p => !string.IsNullOrEmpty(p)).ToList() : new List<string>(),
                SpokenLanguages = data.TryGetProperty("spoken_languages", out var langs) && langs.ValueKind == JsonValueKind.Array ? langs.EnumerateArray().Select(l => l.GetString() ?? string.Empty).Where(l => !string.IsNullOrEmpty(l)).ToList() : new List<string>(),
                Amenities = data.TryGetProperty("facilities_block", out var facBlock) && facBlock.TryGetProperty("facilities", out var facs) && facs.ValueKind == JsonValueKind.Array ? facs.EnumerateArray().Select(f => f.GetProperty("name").GetString() ?? string.Empty).Where(f => !string.IsNullOrEmpty(f)).ToList() : new List<string>(),
                Rooms = new List<RoomViewModel>()
            };

            // Rooms
            if (data.TryGetProperty("rooms", out var rooms) && rooms.ValueKind == JsonValueKind.Object)
            {
                foreach (var roomProp in rooms.EnumerateObject())
                {
                    var room = roomProp.Value;
                    var roomVm = new RoomViewModel
                    {
                        RoomTypeId = roomProp.Name,
                        Name = room.TryGetProperty("room_name", out var rname) ? rname.GetString() : null,
                        Description = room.TryGetProperty("description", out var rdesc) ? rdesc.GetString() : null,
                        MaxOccupancy = room.TryGetProperty("max_occupancy", out var maxOcc) && maxOcc.ValueKind == JsonValueKind.String && int.TryParse(maxOcc.GetString(), out var occ) ? occ : (int?)null,
                        Price = null,
                        Currency = null,
                        RoomPhotoUrls = room.TryGetProperty("photos", out var rphotos) && rphotos.ValueKind == JsonValueKind.Array ? rphotos.EnumerateArray().Select(p => p.TryGetProperty("url_original", out var u) ? u.GetString() ?? string.Empty : string.Empty).Where(u => !string.IsNullOrEmpty(u)).ToList() : new List<string>(),
                        Facilities = room.TryGetProperty("facilities", out var rfac) && rfac.ValueKind == JsonValueKind.Array ? rfac.EnumerateArray().Select(f => f.TryGetProperty("name", out var n) ? n.GetString() ?? string.Empty : string.Empty).Where(n => !string.IsNullOrEmpty(n)).ToList() : new List<string>(),
                        BedConfigurations = room.TryGetProperty("bed_configurations", out var beds) && beds.ValueKind == JsonValueKind.Array ? beds.EnumerateArray().SelectMany(bc => bc.TryGetProperty("bed_types", out var bt) && bt.ValueKind == JsonValueKind.Array ? bt.EnumerateArray().Select(b => b.TryGetProperty("name_with_count", out var n) ? n.GetString() ?? string.Empty : string.Empty).Where(n => !string.IsNullOrEmpty(n)) : new List<string>()).ToList() : new List<string>()
                    };
                    vm.Rooms.Add(roomVm);
                }
            }

            return vm;
        }

    }
}

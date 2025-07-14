namespace BookingHotel.Models
{
    public class HotelDetailViewModel
    {
            public int HotelId { get; set; }
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? CountryCode { get; set; }

            public string?
                Description
            {
                get;
                set;
            } // Otel genel açıklaması, API'de Hotel_Text yerine genel açıklama olarak gelebilir.

            public float? Latitude { get; set; } // API'nin Data objesinden gelebilir.
            public float? Longitude { get; set; } // API'nin Data objesinden gelebilir.
            public float? ReviewScore { get; set; }
            public int? ReviewCount { get; set; }
            public string? ReviewScoreWord { get; set; }
            public string? Url { get; set; }

            // API'den gelen Data objesinde bu bilgiler doğrudan yoktu, istersen ekleyebiliriz veya başka bir yerden almalıyız.
            // public string? CheckinDate { get; set; }
            // public string? CheckoutDate { get; set; }
            // public string? Currency { get; set; }

            // API'den gelen Rooms.RoomTypes'taki her bir oda tipi için bir RoomViewModel oluşturulacak.
            public List<RoomViewModel>? Rooms { get; set; } = new();

            // API'den gelen Data.Amenities veya Facilities_Block gibi yerlerden çekilebilir.
            public List<string>? Amenities { get; set; } = new();

            // HotelDetail objesinde doğrudan PhotoUrls listesi yoktu.
            // Genellikle otelin genel fotoğrafları başka bir endpoint'ten gelir ya da her oda tipinin kendi fotoğrafları olur.
            // Eğer otel genel fotoğrafları varsa, buraya eşlenebilir.
            public List<string>? PhotoUrls { get; set; } = new();

            public List<string>? SpokenLanguages { get; set; } = new();
        }

        public class RoomViewModel
        {
            public string? RoomTypeId { get; set; } // Dinamik oda anahtarını (örn: "682714101") burada tutabiliriz.

            public string?
                Name
            { get; set; } // Oda tipi adı, orijinal API'de Block içindeki name veya room_name'den gelebilir.

            public string? Description { get; set; } // Odaların kendi description'ı

            public int?
                MaxOccupancy
            { get; set; } // Orijinal API'nin Block kısmında max_occupancy vardı, buradan gelebilir.

            // Fiyat bilgisi Booking.com API'sinde çok katmanlı, ViewModel'de sade tutabiliriz.
            // Eğer belirli bir block veya rezervasyon için fiyatı göstereceksek, RoomViewModel'e eklenebilir.
            public float? Price { get; set; }
            public string? Currency { get; set; }

            public List<string>? RoomPhotoUrls { get; set; } = new(); // Oda resimleri
            public List<string>? Facilities { get; set; } = new(); // Oda içindeki olanaklar
            public List<string>? BedConfigurations { get; set; } = new(); // Yatak tipleri
        }
    }


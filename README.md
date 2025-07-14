# 🏨 Booking.com API Entegrasyonu - ASP.NET Core 8.0

Bu proje, **M&Y Yazılım Eğitim Akademi Danışmanlık** bünyesinde ve **Murat Yücedağ** hocamın mentörlüğünde geliştirilmiştir. RapidAPI üzerinden Booking.com servislerini kullanarak **gerçek zamanlı bir otel arama uygulaması** oluşturulmuştur.

## 🚀 Proje Özeti

Bu uygulama, kullanıcının girdiği seyahat bilgilerine göre Booking.com API'sinden otelleri dinamik olarak çeker ve detayları ile birlikte kullanıcıya sunar. Projede herhangi bir veritabanı kullanılmamıştır, tüm veriler gerçek zamanlı olarak API üzerinden alınmaktadır.

## 🛠️ Kullanılan Teknolojiler

- ASP.NET Core 8.0
- C#
- HttpClient (API çağrıları)
- Newtonsoft.Json (JSON verisi ayrıştırma)
- ViewModel Yapıları (veri taşımak ve UI düzenlemek için)
- Razor Pages / MVC Görünümleri

## 👤 Kullanıcıdan Alınan Bilgiler

- Şehir adı
- Giriş ve çıkış tarihleri
- Yetişkin sayısı
- Oda sayısı

## 🖥️ Kullanıcıya Sunulan Bilgiler

- Otel adı
- Puanı ve değerlendirme sayısı
- Fiyat bilgisi (USD bazlı)
- Otelin adresi (şehir, ülke dahil)
- Yüksek çözünürlüklü görseller
- Otelin sunduğu tüm olanaklar

## 🌐 Entegre Edilen API Endpoint’leri

| Endpoint | Açıklama |
|----------|----------|
| `/stays/auto-complete` | Kullanıcının girdiği şehir adına karşılık gelen destinasyon ID’sini alır |
| `/stays/search` | Tarih, kişi sayısı ve oda sayısına göre otel arar |
| `/hotels/photos` | Otellere ait yüksek çözünürlüklü görselleri çeker |
| `/hotels/details` | Otel açıklaması, puanı ve olanakları gibi detayları döner |

## 🧩 Proje Özellikleri

- Gerçek zamanlı API veri yönetimi
- Veritabanı olmadan dinamik içerik sunumu
- ViewModel kullanımıyla veri modelleme
- Responsive kullanıcı arayüzü (Razor View)

## 📂 Proje Yapısı

<img width="1915" height="947" alt="Image" src="https://github.com/user-attachments/assets/4b5ce3b7-16a0-47be-914e-599090b230ec" />
<br> <br> 

<img width="1916" height="943" alt="Image" src="https://github.com/user-attachments/assets/cd316271-7037-4787-8989-70a498dd62c0" />
<br> <br> 

<img width="1918" height="830" alt="Image" src="https://github.com/user-attachments/assets/1e5786c5-3c89-474f-bb83-18c7d92a5350" />
<br> <br> 

<img width="1917" height="959" alt="Image" src="https://github.com/user-attachments/assets/b96f63be-150a-4168-927a-f3c093ee856c" />


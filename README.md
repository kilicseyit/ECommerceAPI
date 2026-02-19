🛒 E-Ticaret REST API (.NET 8)

Clean Architecture prensiplerine uygun olarak geliştirilmiş, JWT tabanlı kimlik doğrulama ve rol bazlı yetkilendirme sistemi içeren RESTful bir E-Ticaret backend uygulamasıdır.

Bu proje, gerçek dünya e-ticaret senaryolarını simüle etmek ve güvenli, ölçeklenebilir bir backend mimarisi oluşturmak amacıyla geliştirilmiştir.

🚀 Proje Hakkında

Bu API aşağıdaki temel ihtiyaçları karşılamak üzere tasarlanmıştır:

Güvenli kimlik doğrulama sistemi

Rol bazlı erişim kontrolü (Admin / Kullanıcı)

Ürün ve kategori yönetimi

Sipariş ve stok yönetimi

Merkezi hata yönetimi

Katmanlı ve genişletilebilir mimari yapı

Sistem, üretim ortamına uygun şekilde tasarlanmıştır.

🏗️ Mimari Yapı

Proje aşağıdaki prensiplere uygun olarak geliştirilmiştir:

✅ Clean Architecture

✅ Repository (Depo) Tasarım Deseni

✅ Service (Servis) Katmanı

✅ DTO (Veri Transfer Nesnesi) kullanımı

✅ Middleware tabanlı global hata yönetimi

✅ Merkezi istek kayıt (loglama) yapısı

Katmanlar:

Controllers → HTTP isteklerini karşılar

Services → İş kurallarını içerir

Repositories → Veritabanı erişimini yönetir

DTOs → Veri taşıma işlemlerini düzenler

Middlewares → Ortak işlemleri yönetir

Bu yapı sayesinde sistem:

Test edilebilir

Genişletilebilir

Bakımı kolay

Katmanlar arası bağımlılığı düşük

🔐 Kimlik Doğrulama ve Yetkilendirme

JWT tabanlı kimlik doğrulama

Rol bazlı yetkilendirme (Admin / Kullanıcı)

BCrypt ile şifre hashleme

Token süre kontrolü

Yetkili erişim gerektiren korumalı endpointler

Güvenlik öncelikli bir yaklaşım benimsenmiştir.

📦 İş Kuralları
🛍️ Ürün Yönetimi

Admin kullanıcı ürün ekleyebilir, güncelleyebilir ve silebilir

Kategori bazlı ürün yönetimi yapılabilir

Stok takibi yapılır

Stok 0’ın altına düşemez

📦 Sipariş Yönetimi

Kullanıcı sipariş oluşturabilir

Sipariş oluşturulduğunda stok otomatik olarak düşer

Sipariş iptal edilirse stok iade edilir

Kullanıcı yalnızca kendi siparişlerini görüntüleyebilir

Admin tüm siparişleri görüntüleyebilir

Sipariş durum akışı:

Bekliyor

Kargoda

Teslim Edildi

İptal Edildi

🛠️ Kullanılan Teknolojiler

.NET 8 Web API

Entity Framework Core

SQL Server

JWT Kimlik Doğrulama

BCrypt.Net

Swagger / OpenAPI

REST mimari prensipleri

⚙️ Kurulum
Gereksinimler

.NET 8 SDK

SQL Server

Visual Studio 2022 veya VS Code

Kurulum Adımları
# Repoyu klonla
git clone https://github.com/kilicseyit/ecommerce-api.git

# Proje klasörüne gir
cd ECommerceAPI

# Bağımlılıkları yükle
dotnet restore

# appsettings.json dosyasını düzenle
# Veritabanı bağlantısı ve JWT ayarlarını gir

# Veritabanını oluştur
dotnet ef database update

# Projeyi çalıştır
dotnet run

🔧 Yapılandırma (appsettings.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SUNUCU_ADI;Database=ECommerceDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "en-az-32-karakter-uzunlugunda-gizli-anahtar",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "ExpiryInDays": 7
  }
}

📁 Proje Yapısı
ECommerceAPI/
├── Controllers/
├── Services/
├── Repositories/
├── DTOs/
├── Models/
├── Middlewares/
├── Data/
└── Program.cs

📌 API Uç Noktaları
🔐 Kimlik
Yöntem	Adres	Açıklama
POST	/api/auth/register	Kullanıcı kaydı
POST	/api/auth/login	Kullanıcı girişi
GET	/api/auth/profile	Profil bilgisi
PUT	/api/auth/profile	Profil güncelleme
GET	/api/auth/users	Tüm kullanıcılar (Admin)
📦 Ürünler
Yöntem	Adres	Açıklama
GET	/api/products	Tüm ürünler
GET	/api/products/{id}	Ürün detayı
POST	/api/products	Ürün ekleme (Admin)
PUT	/api/products/{id}	Ürün güncelleme (Admin)
DELETE	/api/products/{id}	Ürün silme (Admin)
🛍️ Siparişler
Yöntem	Adres	Açıklama
GET	/api/orders	Siparişler
GET	/api/orders/{id}	Sipariş detayı
POST	/api/orders	Sipariş oluşturma
PUT	/api/orders/{id}/status	Sipariş durumu güncelleme (Admin)
DELETE	/api/orders/{id}	Sipariş silme (Admin)
📊 API Dokümantasyonu

Swagger arayüzüne aşağıdaki adresten erişebilirsiniz:

https://localhost:{port}/swagger

🔮 Gelecek Geliştirmeler

Birim testleri (xUnit)

Entegrasyon testleri

Refresh Token sistemi

Sayfalama ve filtreleme

Docker desteği

CI/CD süreci

Redis ile önbellekleme

Rate limiting (istek sınırlandırma)

Soft delete (yumuşak silme)

👨‍💻 Geliştirici

Seyit Kılıç
Backend Developer (.NET)

GitHub: https://github.com/kilicseyit

LinkedIn: https://linkedin.com/in/kilicseyit

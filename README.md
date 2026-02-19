# 🛒 E-Ticaret Yönetim Paneli

Modern ve kullanıcı dostu bir e-ticaret yönetim paneli. Vue.js frontend ve .NET Core Web API backend ile geliştirilmiştir.

## 🚀 Özellikler

### 👤 Kimlik Doğrulama & Yetkilendirme
- JWT tabanlı kimlik doğrulama
- Rol bazlı yetkilendirme (Admin / User)
- Kullanıcı profil yönetimi

### 📦 Ürün Yönetimi
- Ürün ekleme, düzenleme, silme
- Kategori bazlı ürün yönetimi
- Stok takibi ve düşük stok uyarısı

### 🛍️ Sipariş Yönetimi
- Sipariş oluşturma ve takip
- Otomatik stok güncelleme (sipariş & iptal)
- Sipariş durumu güncelleme (Bekliyor → İşleniyor → Kargoda → Teslim Edildi)
- Admin panelinde kullanıcı bazlı sipariş oluşturma
- Kullanıcılar sadece kendi siparişlerini görüntüler

### 📊 Dashboard
- Toplam ürün, kategori, sipariş istatistikleri
- Stok durumu özeti
- Bekleyen sipariş sayısı

## 🛠️ Kullanılan Teknolojiler

### Frontend
- Vue.js 3 (Composition API)
- Vue Router
- Axios
- Vite

### Backend
- .NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI

## ⚙️ Kurulum

### Backend
```bash
cd ECommerceAPI
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend
```bash
cd ecommerce-ui
npm install
npm run dev
```

## 🔧 Ortam Değişkenleri

`appsettings.json` dosyasında şu alanları düzenle:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
}
```

## 📁 Proje Yapısı
```
ecommerce-ui/
├── src/
│   ├── components/
│   ├── views/
│   ├── services/
│   ├── stores/
│   └── router/
```

## 👤 Geliştirici

**Seyit Kılıç**
- GitHub: [@kilicseyit](https://github.com/kilicseyit)
- LinkedIn: [linkedin.com/in/seyiitklc](https://www.linkedin.com/in/seyiitklc/)

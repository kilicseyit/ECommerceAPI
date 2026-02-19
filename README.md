🛒 E-Commerce REST API (.NET 8)

Production-ready, Clean Architecture principles ile geliştirilmiş, JWT authentication ve role-based authorization içeren RESTful E-Commerce backend uygulaması.

Bu proje, gerçek dünya e-ticaret senaryolarını simüle etmek ve ölçeklenebilir, güvenli bir backend mimarisi oluşturmak amacıyla geliştirilmiştir.

🚀 Project Overview

Bu API aşağıdaki temel iş ihtiyaçlarını karşılamak üzere tasarlanmıştır:

Güvenli kimlik doğrulama sistemi

Rol bazlı erişim kontrolü (Admin / User)

Ürün & kategori yönetimi

Sipariş ve stok yönetimi

Merkezi hata yönetimi

Katmanlı ve genişletilebilir mimari yapı

Sistem, production ortamına hazır olacak şekilde yapılandırılmıştır.

🏗️ Architecture

Proje aşağıdaki prensiplere uygun geliştirilmiştir:

✅ Clean Architecture

✅ Repository Pattern

✅ Service Layer abstraction

✅ DTO (Data Transfer Object) separation

✅ Middleware-based global exception handling

✅ Centralized request logging

Katmanlı yapı:

Controllers → HTTP layer

Services → Business logic

Repositories → Data access

DTOs → Data transfer abstraction

Middlewares → Cross-cutting concerns

Bu yapı sayesinde sistem:

Test edilebilir

Genişletilebilir

Maintainable

Loosely coupled

🔐 Authentication & Authorization

JWT Token-based authentication

Role-Based Authorization (Admin / User)

BCrypt password hashing

Protected endpoints

Token expiration management

Security-first yaklaşımı benimsenmiştir.

📦 Business Logic
🛍️ Product Management

Admin ürün CRUD işlemleri yapabilir

Kategori bazlı ürün yönetimi

Stok takibi

Stok 0 altına düşemez

📦 Order Management

Kullanıcı sipariş oluşturabilir

Sipariş oluşturulduğunda stok otomatik düşer

Sipariş iptal edilirse stok iade edilir

Kullanıcı sadece kendi siparişlerini görür

Admin tüm siparişleri görüntüleyebilir

Sipariş durum akışı:

Pending

Shipped

Delivered

Cancelled

🛠️ Technologies

.NET 8 Web API

Entity Framework Core

SQL Server

JWT Authentication

BCrypt.Net

Swagger / OpenAPI

RESTful API principles

⚙️ Installation
Requirements

.NET 8 SDK

SQL Server

Visual Studio 2022 or VS Code

Setup Steps
# Clone repository
git clone https://github.com/kilicseyit/ecommerce-api.git

# Navigate to project folder
cd ECommerceAPI

# Restore dependencies
dotnet restore

# Configure appsettings.json
# Update database connection & JWT settings

# Apply migrations
dotnet ef database update

# Run project
dotnet run

🔧 Configuration (appsettings.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDB;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-min-32-characters",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "ExpiryInDays": 7
  }
}

📁 Project Structure
ECommerceAPI/
├── Controllers/
├── Services/
├── Repositories/
├── DTOs/
├── Models/
├── Middlewares/
├── Data/
└── Program.cs

📌 API Endpoints
🔐 Auth
Method	Endpoint	Description
POST	/api/auth/register	Register user
POST	/api/auth/login	Login
GET	/api/auth/profile	Get profile
PUT	/api/auth/profile	Update profile
GET	/api/auth/users	Get all users (Admin)
📦 Products
Method	Endpoint	Description
GET	/api/products	Get all products
GET	/api/products/{id}	Get product by id
POST	/api/products	Create product (Admin)
PUT	/api/products/{id}	Update product (Admin)
DELETE	/api/products/{id}	Delete product (Admin)
🛍️ Orders
Method	Endpoint	Description
GET	/api/orders	Get orders
GET	/api/orders/{id}	Get order detail
POST	/api/orders	Create order
PUT	/api/orders/{id}/status	Update status (Admin)
DELETE	/api/orders/{id}	Delete order (Admin)
📊 API Documentation

Swagger UI:

https://localhost:{port}/swagger

🔮 Future Improvements (Roadmap)

Unit Testing (xUnit)

Integration Tests

Refresh Token implementation

Pagination & Filtering

Docker support

CI/CD pipeline

Redis caching

Rate limiting

Soft delete support

👨‍💻 Developer

Seyit Kılıç
Backend Developer (.NET)

GitHub: https://github.com/kilicseyit

LinkedIn: https://linkedin.com/in/kilicseyit

⭐ If you found this project useful, feel free to star the repository!

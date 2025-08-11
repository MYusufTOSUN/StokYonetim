# 📦 StokYonetim

Kendi geliştirdiğim, **ASP.NET Core MVC**, **Windows Forms** ve **Oracle** veritabanı kullanan basit ama işlevli stok–sipariş yönetim sistemi.  
Amacı hem veritabanı hem de yazılım tarafında (web + masaüstü) pratik yapmaktır.

---

## 🚀 Özellikler
- **Modüller:** Sipariş, Cari, Stok, Analiz, Kullanıcı Girişi
- **Roller:** Admin (tam yetki), Personel (sadece görüntüleme)
- **Veritabanı:** Trigger, Sequence, Procedure, Function ve View kullanımı
- **Loglama:** Tüm ana tablolar için otomatik log tabloları
- **Analiz:** Chart.js ile temel grafikler (web tarafı)
- **Dışa Aktarım:** CSV, PDF, Excel desteği
- **Forms Uygulaması:**  
  - Oracle bağlantılı Windows Forms arayüzü  
  - Stok ve sipariş ekleme, silme, güncelleme  
  - Bağlantı testi, arama, veri dışa aktarma

---

## 🗂 Proje Yapısı
- **schema_ddl.sql** → Oracle veritabanı şeması (tablolar, kısıtlar, tetikleyiciler, prosedürler, fonksiyonlar, görünümler)
- **StokWebYeni/** → Web uygulaması (ASP.NET Core MVC)
  - `Controllers/Admin/` ve `Controllers/Personel/`
  - `Views/Siparis/`, `Views/Cari/`, `Views/Stok/`, `Views/AdminPanel/`
- **WindowsForms/** → Forms uygulaması (Oracle bağlantılı masaüstü uygulama)
- **StokWebYeni.sln** → Visual Studio çözüm dosyası

---

## 🗄 Veritabanı Tabloları
- **KART_STOK:** Stok kayıtları
- **KART_CARI:** Cari kayıtları
- **SIPARIS_DOSYA:** Sipariş kayıtları
- **TANIM_BIRIM:** Birim tanımları
- **LOG_*** tabloları ile değişiklik kayıtları  
Ayrıca `SIPARIS_EKLE`, `ESITLE_STOK_MIKTAR` prosedürleri, `CARI_TOPLAM_SIPARIS` fonksiyonu ve çeşitli görünümler (`VW_CARI_SIPARIS_LISTE`, `VW_STOK_TOPLAM_SIPARIS`, `VW_EKSI_STOKLAR`).

---

## 🗺 Veritabanı ER Diyagramı
![ER Diagram](https://github.com/MYusufTOSUN/StokYonetim/blob/main/StokDatabase/DbEr1.png)
---

## ⚙️ Kurulum
1. Oracle veritabanında `schema_ddl.sql` dosyasını çalıştırın.
2. `appsettings.json` veya 'cncstr.cs' dosyasında bağlantı ayarını yapın:
   ```json
   "ConnectionStrings": {
     "connStr": "Data Source=XE;User Id=XXXX;Password=XXXX;"
   }
3. Uygun şekilde .NET Forms veya ASP.NET MVC uygulamasını çalıştırın.

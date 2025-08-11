using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace StokWeb.Controllers.Admin
{
    public class AdminPanelController : Controller
    {
        string connStr;

        private readonly IConfiguration _configuration;

        public AdminPanelController(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("connStr");
        }

        public IActionResult Index()
        {
            // Varsayılan değerler
            ViewBag.Kullanici = HttpContext.Session.GetString("KULLANICI_ADI") ?? "Admin";
            ViewBag.Son7GunSiparis = 0;
            ViewBag.EksiStokSayisi = 0;
            ViewBag.CariSayisi = 0;
            ViewBag.EksiStoklar = new DataTable();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using (var cmd = new OracleCommand(
                    @"SELECT COUNT(*) FROM SIPARIS_DOSYA 
                      WHERE SIPARIS_TARIHI >= SYSDATE - 7", conn))
                    ViewBag.Son7GunSiparis = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                using (var cmd = new OracleCommand(
                    @"SELECT COUNT(*) FROM KART_STOK 
                      WHERE (ALINAN_SIPARIS_MIKTARI - VERILEN_SIPARIS_MIKTARI) < 0", conn))
                    ViewBag.EksiStokSayisi = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                using (var cmd = new OracleCommand("SELECT COUNT(*) FROM KART_CARI", conn))
                    ViewBag.CariSayisi = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                // Eksi stokları detaylı getir
                string sqlDetay = @"
                    SELECT STOK_AD,
                           ALINAN_SIPARIS_MIKTARI,
                           VERILEN_SIPARIS_MIKTARI,
                           (ALINAN_SIPARIS_MIKTARI - VERILEN_SIPARIS_MIKTARI) AS FARK
                    FROM KART_STOK
                    WHERE (ALINAN_SIPARIS_MIKTARI - VERILEN_SIPARIS_MIKTARI) < 0";

                var table = new DataTable();
                using (var da = new OracleDataAdapter(sqlDetay, conn))
                    da.Fill(table);

                ViewBag.EksiStoklar = table;
            }
            catch (Exception ex)
            {
                // Hata durumunda ViewBag'e hata mesajı ekle
                ViewBag.ErrorMessage = $"Veritabanı bağlantı hatası: {ex.Message}";

                // Log the error (you can add proper logging here)
                Console.WriteLine($"AdminPanel Index Error: {ex.Message}");
            }

            return View();
        }
        public IActionResult ExportEksiStokCsv()
        {
            try
            {
                DataTable dt = new DataTable();

                using (var conn = new OracleConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                    SELECT STOK_AD,
                           ALINAN_SIPARIS_MIKTARI,
                           VERILEN_SIPARIS_MIKTARI,
                           (ALINAN_SIPARIS_MIKTARI - VERILEN_SIPARIS_MIKTARI) AS FARK
                    FROM KART_STOK
                    WHERE (ALINAN_SIPARIS_MIKTARI - VERILEN_SIPARIS_MIKTARI) < 0
                    ORDER BY STOK_AD";

                    using (var da = new OracleDataAdapter(sql, conn))
                        da.Fill(dt);
                }

                // Eğer veri yoksa boş CSV döndür
                if (dt.Rows.Count == 0)
                {
                    var emptyContent = "STOK_AD;ALINAN_SIPARIS_MIKTARI;VERILEN_SIPARIS_MIKTARI;FARK\r\n";
                    return File(Encoding.UTF8.GetBytes(emptyContent), "text/csv", "EksiStoklar.csv");
                }

                var csv = new StringBuilder();

                // Başlıklar - doğru formatla
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    csv.Append(dt.Columns[i].ColumnName);
                    if (i < dt.Columns.Count - 1) csv.Append(";");
                }
                csv.AppendLine();

                // Satırlar - doğru formatla
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        var value = row[i]?.ToString() ?? "";
                        // Semicolon'ları CSV formatı için escape et
                        value = value.Replace(";", ",");
                        csv.Append(value);
                        if (i < dt.Columns.Count - 1) csv.Append(";");
                    }
                    csv.AppendLine();
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(bytes, "text/csv", "EksiStoklar.csv");
            }
            catch (Exception)
            {
                // Hata durumunda log ve hata mesajı
                // Logger kullanılabilir: _logger.LogError(ex, "CSV export hatası");

                // Basit bir hata CSV'si döndür
                var errorContent = "HATA;CSV oluşturulurken hata oluştu\r\n;Lütfen sistem yöneticisi ile iletişime geçin\r\n";
                return File(Encoding.UTF8.GetBytes(errorContent), "text/csv", "EksiStoklar_Hata.csv");
            }
        }
    }
}

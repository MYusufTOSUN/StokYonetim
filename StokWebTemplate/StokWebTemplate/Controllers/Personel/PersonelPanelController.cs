using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

public class PersonelPanelController : Controller
{
    public IActionResult Index()
    {
        // İlk giriş kontrolü - hoşgeldin mesajı için
        if (HttpContext.Session.GetString("PersonelWelcomeShown") == null)
        {
            ViewBag.ShowWelcome = true;
            HttpContext.Session.SetString("PersonelWelcomeShown", "true");
        }
        else
        {
            ViewBag.ShowWelcome = false;
        }

        // Varsayılan değerler
        ViewBag.Kullanici = HttpContext.Session.GetString("KULLANICI_ADI") ?? "Personel";
        ViewBag.ToplamSiparis = 0;
        ViewBag.ToplamCari = 0;
        ViewBag.ToplamStok = 0;

        try
        {
            using (var conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;"))
            {
                conn.Open();

                ViewBag.ToplamSiparis = ExecuteCount(conn, "SELECT COUNT(*) FROM SIPARIS_DOSYA");
                ViewBag.ToplamCari = ExecuteCount(conn, "SELECT COUNT(*) FROM KART_CARI");
                ViewBag.ToplamStok = ExecuteCount(conn, "SELECT COUNT(*) FROM KART_STOK");

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda ViewBag'e hata mesajı ekle
            ViewBag.ErrorMessage = $"Veritabanı bağlantı hatası: {ex.Message}";

            // Log the error
            Console.WriteLine($"PersonelPanel Index Error: {ex.Message}");
        }

        return View();
    }

    private int ExecuteCount(OracleConnection conn, string query)
    {
        using (var cmd = new OracleCommand(query, conn))
        {
            object result = cmd.ExecuteScalar();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }
    }
}

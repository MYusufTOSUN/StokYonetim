using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace StokWeb.Controllers.Admin
{
    public class AdminAnalizController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminAnalizController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connStr = _configuration.GetConnectionString("connStr");

            using var conn = new OracleConnection(connStr);
            conn.Open();

            ViewBag.Kullanici = HttpContext.Session.GetString("KULLANICI_ADI");

            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM SIPARIS_DOSYA", conn))
                ViewBag.ToplamSiparis = Convert.ToInt32(cmd.ExecuteScalar());
            // Toplam Alım Siparişi
            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM SIPARIS_DOSYA WHERE SIPARIS_TUR = 'A'", conn))
                ViewBag.ToplamAlim = Convert.ToInt32(cmd.ExecuteScalar());

            // Toplam Verilen Siparişi
            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM SIPARIS_DOSYA WHERE SIPARIS_TUR = 'V'", conn))
                ViewBag.ToplamVerilen = Convert.ToInt32(cmd.ExecuteScalar());


            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM KART_CARI", conn))
                ViewBag.ToplamCari = Convert.ToInt32(cmd.ExecuteScalar());

            using (var cmd = new OracleCommand("SELECT COUNT(*) FROM KART_STOK", conn))
                ViewBag.ToplamStok = Convert.ToInt32(cmd.ExecuteScalar());

            string sqlEnCokStok = @"
                SELECT ks.STOK_AD, SUM(sd.MIKTAR) AS TOPLAM
                FROM SIPARIS_DOSYA sd
                JOIN KART_STOK ks ON ks.STOK_ID = sd.STOK_ID
                GROUP BY ks.STOK_AD
                ORDER BY TOPLAM DESC
            ";
            var enCokStoklar = new DataTable();
            using (var cmd = new OracleCommand(sqlEnCokStok, conn))
            using (var da = new OracleDataAdapter(cmd))
                da.Fill(enCokStoklar);
            ViewBag.EnCokStoklar = enCokStoklar;

            string sqlZamanSiparis = @"
                SELECT TO_CHAR(SIPARIS_TARIHI, 'YYYY-MM') AS AY, COUNT(*) AS ADET
                FROM SIPARIS_DOSYA
                WHERE SIPARIS_TARIHI >= ADD_MONTHS(SYSDATE, -6)
                GROUP BY TO_CHAR(SIPARIS_TARIHI, 'YYYY-MM')
                ORDER BY AY
            ";
            var zamanSiparis = new DataTable();
            using (var cmd = new OracleCommand(sqlZamanSiparis, conn))
            using (var da = new OracleDataAdapter(cmd))
                da.Fill(zamanSiparis);
            ViewBag.ZamanSiparis = zamanSiparis;

            string sqlTurDagilimi = @"
                SELECT SIPARIS_TUR, COUNT(*) AS ADET
                FROM SIPARIS_DOSYA
                GROUP BY SIPARIS_TUR
            ";
            var turDagilimi = new DataTable();
            using (var cmd = new OracleCommand(sqlTurDagilimi, conn))
            using (var da = new OracleDataAdapter(cmd))
                da.Fill(turDagilimi);
            ViewBag.TurDagilimi = turDagilimi;

            return View();
        }
    }
}

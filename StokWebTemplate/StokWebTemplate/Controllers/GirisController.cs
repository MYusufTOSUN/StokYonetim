using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace StokWeb.Controllers
{
    public class GirisController : Controller
    {
        private readonly IConfiguration _configuration;

        public GirisController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Giriş formunu göster
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Giriş formu post işlemi
        [HttpPost]
        public IActionResult Index(string KULLANICI_ADI, string SIFRE)
        {
            try
            {
                string connStr = _configuration.GetConnectionString("connStr");

                using var conn = new OracleConnection(connStr);
                conn.Open();

                string sql = "SELECT COUNT(*) FROM LOGIN_KULLANICI WHERE KULLANICI_ADI = :ad AND SIFRE = :sifre";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("ad", KULLANICI_ADI));
                cmd.Parameters.Add(new OracleParameter("sifre", SIFRE));

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    HttpContext.Session.SetString("KULLANICI_ADI", KULLANICI_ADI);

                    if (KULLANICI_ADI.ToLower().Contains("admin"))
                        return RedirectToAction("Index", "AdminPanel");
                    else
                        return RedirectToAction("Index", "PersonelPanel");
                }

                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Veritabanı bağlantı hatası: " + ex.Message;
                return View();
            }
        }

        // Bağlantı test (tarayıcıda /Giris/Test)
        public IActionResult Test()
        {
            try
            {
                string connStr = _configuration.GetConnectionString("connStr");
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = new OracleCommand("SELECT SYSDATE FROM DUAL", conn);
                var result = cmd.ExecuteScalar();

                return Content("✅ Oracle bağlantısı başarılı: " + result?.ToString());
            }
            catch (Exception ex)
            {
                return Content("❌ Oracle bağlantı hatası: " + ex.Message);
            }
        }
    }
}

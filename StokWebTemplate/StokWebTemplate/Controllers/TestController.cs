using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace StokWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string connStr;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("connStr") ?? "";
        }

        public IActionResult Database()
        {
            var model = new DatabaseTestModel();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                model.ConnectionStatus = "✅ Bağlantı başarılı!";
                model.DatabaseVersion = conn.ServerVersion;

                // Test tabloları kontrol et
                var tables = new List<string> { "SIPARIS_DOSYA", "KART_STOK", "KART_CARI" };
                foreach (var table in tables)
                {
                    try
                    {
                        using var cmd = new OracleCommand($"SELECT COUNT(*) FROM {table}", conn);
                        var count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        model.TableCounts[table] = count;
                        model.TableStatus[table] = "✅ Mevcut";
                    }
                    catch (Exception ex)
                    {
                        model.TableStatus[table] = $"❌ Hata: {ex.Message}";
                        model.TableCounts[table] = 0;
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                model.ConnectionStatus = $"❌ Bağlantı hatası: {ex.Message}";
                model.ErrorDetails = ex.ToString();
            }

            return View(model);
        }
    }

    public class DatabaseTestModel
    {
        public string ConnectionStatus { get; set; } = "";
        public string DatabaseVersion { get; set; } = "";
        public string ErrorDetails { get; set; } = "";
        public Dictionary<string, int> TableCounts { get; set; } = new();
        public Dictionary<string, string> TableStatus { get; set; } = new();
    }
}
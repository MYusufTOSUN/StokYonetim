using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace WebProject.Controllers.Admin
{
    public class AdminStokController : Controller
    {
        private readonly string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string stokad = "T", int sayfa = 1)
        {
            int sayfaBoyutu = 20;
            int toplamKayit = 0;
            int toplamSayfa = 0;
            DataTable dt = new();

            using var conn = new OracleConnection(connectionString);
            conn.Open();

            string countSql = "SELECT COUNT(*) FROM KART_STOK WHERE (:ad = 'T' OR STOK_AD = :ad)";
            using (var countCmd = new OracleCommand(countSql, conn))
            {
                countCmd.Parameters.Add(":ad", OracleDbType.Varchar2).Value = stokad;
                toplamKayit = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            toplamSayfa = (int)Math.Ceiling((double)toplamKayit / sayfaBoyutu);

            string sql = @"
                SELECT * FROM (
                    SELECT STOK_ID, STOK_AD, BIRIM, OZEL_KOD, ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI,
                           ROW_NUMBER() OVER (ORDER BY STOK_ID) AS RN
                    FROM KART_STOK
                    WHERE (:ad1 = 'T' OR STOK_AD = :ad2)
                )
                WHERE RN > :alt AND RN <= :ust";

            using (var cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(":ad1", OracleDbType.Varchar2).Value = stokad;
                cmd.Parameters.Add(":ad2", OracleDbType.Varchar2).Value = stokad;
                cmd.Parameters.Add(":alt", OracleDbType.Int32).Value = (sayfa - 1) * sayfaBoyutu;
                cmd.Parameters.Add(":ust", OracleDbType.Int32).Value = sayfa * sayfaBoyutu;

                using (OracleDataAdapter da = new(cmd))
                {
                    da.Fill(dt);
                }
            }

            // distinct stok adları
            var adlar = new List<string>();
            using (var adCmd = new OracleCommand("SELECT DISTINCT STOK_AD FROM KART_STOK ORDER BY STOK_AD", conn))
            using (var reader = adCmd.ExecuteReader())
                while (reader.Read())
                    adlar.Add(reader.GetString(0));
            ViewBag.StokAdlari = adlar;

            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;
            ViewBag.StokAd = stokad;

            return View("~/Views/Stok/AdminStok/Index.cshtml", dt);
        }

        public IActionResult ExportStokCsv(string stokad = "T")
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT STOK_ID, STOK_AD, BIRIM, OZEL_KOD, ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI FROM KART_STOK WHERE (:ad = 'T' OR STOK_AD = :ad) ORDER BY STOK_ID";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":ad", OracleDbType.Varchar2).Value = stokad;
                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("STOK_ID;STOK_AD;BIRIM;OZEL_KOD;ALINAN_SIPARIS_MIKTARI;VERILEN_SIPARIS_MIKTARI");

            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"{row["STOK_ID"]};{row["STOK_AD"]};{row["BIRIM"]};{row["OZEL_KOD"]};{row["ALINAN_SIPARIS_MIKTARI"]};{row["VERILEN_SIPARIS_MIKTARI"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "stoklar.csv");
        }

        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            using var conn = new OracleConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM KART_STOK WHERE STOK_ID = :ID";
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                using (var da = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return View("~/Views/Stok/AdminStok/Guncelle.cshtml", dt.Rows[0]);
                }
            }

            TempData["ErrorMessage"] = "Stok bulunamadı.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Guncelle(int id, IFormCollection form)
        {
            try
            {
                using var conn = new OracleConnection(connectionString);
                conn.Open();

                string sql = @"UPDATE KART_STOK 
                               SET STOK_AD = :ad, BIRIM = :birim, OZEL_KOD = :kod 
                               WHERE STOK_ID = :id";

                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":ad", OracleDbType.Varchar2).Value = form["STOK_AD"];
                cmd.Parameters.Add(":birim", OracleDbType.Varchar2).Value = form["BIRIM"];
                cmd.Parameters.Add(":kod", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(form["OZEL_KOD"]) ? DBNull.Value : form["OZEL_KOD"];
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Stok başarıyla güncellendi.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Sil(int id)
        {
            try
            {
                using var conn = new OracleConnection(connectionString);
                conn.Open();
                string sql = "DELETE FROM KART_STOK WHERE STOK_ID = :id";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                    TempData["SuccessMessage"] = "Stok başarıyla silindi.";
                else
                    TempData["ErrorMessage"] = "Silinecek stok bulunamadı.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Stok silinirken hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Log(int id)
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            STOK_ID, 
                            STOK_AD, 
                            BIRIM, 
                            OZEL_KOD, 
                            ALINAN_SIPARIS_MIKTARI, 
                            VERILEN_SIPARIS_MIKTARI
                         FROM LOG_KART_STOK
                         WHERE STOK_ID = :ID
                         ORDER BY LOG_TIME DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            ViewBag.StokID = id;
            return View("~/Views/Stok/AdminStok/Log.cshtml", dt);
        }

        public IActionResult ExportStokLogCsv(int id)
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            STOK_ID, 
                            STOK_AD, 
                            BIRIM, 
                            OZEL_KOD, 
                            ALINAN_SIPARIS_MIKTARI, 
                            VERILEN_SIPARIS_MIKTARI
                         FROM LOG_KART_STOK
                         WHERE STOK_ID = :ID
                         ORDER BY LOG_TIME DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            var csv = new StringBuilder();

            // Başlıklar
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                csv.Append(dt.Columns[i].ColumnName);
                if (i < dt.Columns.Count - 1) csv.Append(";");
            }
            csv.AppendLine();

            // Satırlar
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    csv.Append(row[i].ToString().Replace(";", ","));
                    if (i < dt.Columns.Count - 1) csv.Append(";");
                }
                csv.AppendLine();
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"stok_log_{id}.csv");
        }
    }
}
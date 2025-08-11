using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace WebProject.Controllers.Admin
{
    public class AdminPersonelController : Controller
    {
        private readonly string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string yetki, int sayfa = 1)
        {
            using var conn = new OracleConnection(connStr);
            conn.Open();

            string sql = "SELECT ID, KULLANICI_ADI FROM LOGIN_KULLANICI ORDER BY ID";
            var table = new DataTable();
            using (var da = new OracleDataAdapter(sql, conn))
                da.Fill(table);

            table.Columns.Add("YETKI", typeof(string));
            foreach (DataRow row in table.Rows)
            {
                string ad = row["KULLANICI_ADI"].ToString().ToLower();
                row["YETKI"] = ad.Contains("admin") ? "Admin" : "Personel";
            }

            var filtreli = table.AsEnumerable();
            if (!string.IsNullOrEmpty(yetki) && yetki != "T")
                filtreli = filtreli.Where(r => r["YETKI"].ToString() == yetki);

            filtreli = filtreli
                .OrderBy(r => r["YETKI"].ToString() == "Personel" ? 1 : 0)
                .ThenBy(r => Convert.ToInt32(r["ID"]));

            int toplamKayit = filtreli.Count();
            int toplamSayfa = (int)Math.Ceiling((double)toplamKayit / 20);
            var sayfaVerisi = filtreli.Skip((sayfa - 1) * 20).Take(20);

            var sonuc = table.Clone();
            foreach (var row in sayfaVerisi)
                sonuc.ImportRow(row);

            ViewBag.Secili = yetki ?? "T";
            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;

            return View("~/Views/AdminPersonel/Index.cshtml", sonuc);
        }

        public IActionResult ExportPersonelCsv(string yetki = "T")
        {
            using var conn = new OracleConnection(connStr);
            conn.Open();

            string sql = "SELECT ID, KULLANICI_ADI FROM LOGIN_KULLANICI ORDER BY ID";
            var table = new DataTable();
            using (var da = new OracleDataAdapter(sql, conn))
                da.Fill(table);

            table.Columns.Add("YETKI", typeof(string));
            foreach (DataRow row in table.Rows)
            {
                string ad = row["KULLANICI_ADI"].ToString().ToLower();
                row["YETKI"] = ad.Contains("admin") ? "Admin" : "Personel";
            }

            var filtreli = table.AsEnumerable();
            if (!string.IsNullOrEmpty(yetki) && yetki != "T")
                filtreli = filtreli.Where(r => r["YETKI"].ToString() == yetki);

            var sb = new StringBuilder();
            sb.AppendLine("ID;KULLANICI_ADI;YETKI");

            foreach (var row in filtreli)
            {
                sb.AppendLine($"{row["ID"]};{row["KULLANICI_ADI"]};{row["YETKI"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "kullanicilar.csv");
        }

        [HttpGet]
        public IActionResult Yeni()
        {
            return View("~/Views/AdminPersonel/Yeni.cshtml");
        }

        [HttpPost]
        public IActionResult Yeni(IFormCollection form)
        {
            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                string sql = @"INSERT INTO LOGIN_KULLANICI (ID, KULLANICI_ADI, SIFRE)
                               VALUES (SEQ_LOGIN_KULLANICI_ID.NEXTVAL, :ad, :sifre)";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":ad", OracleDbType.Varchar2).Value = form["KULLANICI_ADI"];
                cmd.Parameters.Add(":sifre", OracleDbType.Varchar2).Value = form["SIFRE"];
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Yeni kullanıcı eklendi.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Kullanıcı eklenemedi.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            using var conn = new OracleConnection(connStr);
            conn.Open();

            var sql = "SELECT * FROM LOGIN_KULLANICI WHERE ID = :id";
            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                ViewBag.ID = reader["ID"];
                ViewBag.KULLANICI_ADI = reader["KULLANICI_ADI"];
                ViewBag.SIFRE = reader["SIFRE"];
                return View("~/Views/AdminPersonel/Guncelle.cshtml");
            }

            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Guncelle(int id, IFormCollection form)
        {
            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                var sql = @"UPDATE LOGIN_KULLANICI 
                            SET KULLANICI_ADI = :ad, SIFRE = :sifre 
                            WHERE ID = :id";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":ad", OracleDbType.Varchar2).Value = form["KULLANICI_ADI"];
                cmd.Parameters.Add(":sifre", OracleDbType.Varchar2).Value = form["SIFRE"];
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Kullanıcı güncellendi.";
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
                using var conn = new OracleConnection(connStr);
                conn.Open();

                var sql = "DELETE FROM LOGIN_KULLANICI WHERE ID = :id";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                    TempData["SuccessMessage"] = "Kullanıcı silindi.";
                else
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Silme sırasında hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Log(int id)
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            ID, 
                            KULLANICI_ADI, 
                            SIFRE
                         FROM LOG_LOGIN_KULLANICI
                         WHERE ID = :ID
                         ORDER BY LOG_TIME DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            ViewBag.KullaniciID = id;
            return View("~/Views/AdminPersonel/Log.cshtml", dt);
        }

        public IActionResult ExportPersonelLogCsv(int id)
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            ID, 
                            KULLANICI_ADI, 
                            SIFRE
                         FROM LOG_LOGIN_KULLANICI
                         WHERE ID = :ID
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
            return File(bytes, "text/csv", $"kullanici_log_{id}.csv");
        }
    }
}
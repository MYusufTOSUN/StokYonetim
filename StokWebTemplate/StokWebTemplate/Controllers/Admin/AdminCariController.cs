using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace WebProject.Controllers.Admin
{
    public class AdminCariController : Controller
    {
        private readonly string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string tip = "T", int sayfa = 1)
        {
            int sayfaBoyutu = 20;
            int toplamKayit = 0;
            int toplamSayfa = 0;
            DataTable dt = new();

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string countSql = "SELECT COUNT(*) FROM KART_CARI WHERE 1=1";
                if (tip != "T")
                    countSql += " AND CARI_TIP = :tip";

                using (var countCmd = new OracleCommand(countSql, conn))
                {
                    if (tip != "T")
                        countCmd.Parameters.Add(":tip", OracleDbType.Varchar2).Value = tip;
                    toplamKayit = Convert.ToInt32(countCmd.ExecuteScalar());
                }

                toplamSayfa = (int)Math.Ceiling((double)toplamKayit / sayfaBoyutu);

                string sql = @"
            SELECT * FROM (
                SELECT CARI_ID, CARI_SIRKET, CARI_TIP, OZEL_KOD,
                       ROW_NUMBER() OVER (ORDER BY CARI_ID) AS RN
                FROM KART_CARI
                WHERE 1=1";
                if (tip != "T")
                    sql += " AND CARI_TIP = :tip";
                sql += ") WHERE RN > :alt AND RN <= :ust ORDER BY CARI_ID";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    if (tip != "T")
                        cmd.Parameters.Add(":tip", OracleDbType.Varchar2).Value = tip;
                    cmd.Parameters.Add(":alt", OracleDbType.Int32).Value = (sayfa - 1) * sayfaBoyutu;
                    cmd.Parameters.Add(":ust", OracleDbType.Int32).Value = sayfa * sayfaBoyutu;

                    using (OracleDataAdapter da = new(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;
            ViewBag.Tip = tip;

            return View("~/Views/Cari/AdminCari/Index.cshtml", dt);
        }

        public IActionResult ExportCariCsv(string tip = "T")
        {
            DataTable dt = new();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT CARI_ID, CARI_SIRKET, CARI_TIP, OZEL_KOD FROM KART_CARI WHERE 1=1";

                if (tip != "T")
                    sql += " AND CARI_TIP = :tip";

                sql += " ORDER BY CARI_ID";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    if (tip != "T")
                        cmd.Parameters.Add(":tip", OracleDbType.Varchar2).Value = tip;

                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("CARI_ID;CARI_SIRKET;CARI_TIP;OZEL_KOD");

            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"{row["CARI_ID"]};{row["CARI_SIRKET"]};{row["CARI_TIP"]};{row["OZEL_KOD"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "cariler.csv");
        }

        [HttpGet]
        public IActionResult Yeni()
        {
            return View("~/Views/Cari/AdminCari/Yeni.cshtml");
        }

        [HttpPost]
        public IActionResult Yeni(IFormCollection form)
        {
            try
            {
                using var conn = new OracleConnection(connectionString);
                conn.Open();

                string sql = @"INSERT INTO KART_CARI 
                               (CARI_ID, CARI_SIRKET, CARI_TIP, OZEL_KOD)
                               VALUES (SEQ_KART_CARI_ID.NEXTVAL, :sirket, :tip, :kod)";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":sirket", OracleDbType.Varchar2).Value = form["CARI_SIRKET"];
                cmd.Parameters.Add(":tip", OracleDbType.Varchar2).Value = form["CARI_TIP"];
                cmd.Parameters.Add(":kod", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(form["OZEL_KOD"]) ? DBNull.Value : form["OZEL_KOD"];

                cmd.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Yeni cari başarıyla eklendi.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Cari eklenirken bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            using var conn = new OracleConnection(connectionString);
            conn.Open();

            var sql = "SELECT * FROM KART_CARI WHERE CARI_ID = :id";
            using var cmd = new OracleCommand(sql, conn);
            cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
            using var da = new OracleDataAdapter(cmd);
            DataTable dt = new();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                TempData["ErrorMessage"] = "Cari bulunamadı.";
                return RedirectToAction("Index");
            }

            return View("~/Views/Cari/AdminCari/Guncelle.cshtml", dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Guncelle(int id, IFormCollection form)
        {
            try
            {
                using var conn = new OracleConnection(connectionString);
                conn.Open();

                var sql = @"UPDATE KART_CARI 
                            SET CARI_SIRKET = :sirket, CARI_TIP = :tip, OZEL_KOD = :kod 
                            WHERE CARI_ID = :id";

                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":sirket", OracleDbType.Varchar2).Value = form["CARI_SIRKET"];
                cmd.Parameters.Add(":tip", OracleDbType.Varchar2).Value = form["CARI_TIP"];
                cmd.Parameters.Add(":kod", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(form["OZEL_KOD"]) ? DBNull.Value : form["OZEL_KOD"];
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Cari başarıyla güncellendi.";
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
                string sql = "DELETE FROM KART_CARI WHERE CARI_ID = :id";
                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                    TempData["SuccessMessage"] = "Cari başarıyla silindi.";
                else
                    TempData["ErrorMessage"] = "Silinecek cari bulunamadı.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Cari silinirken bir hata oluştu.";
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
                            CARI_ID, 
                            CARI_SIRKET, 
                            CARI_TIP, 
                            OZEL_KOD
                         FROM LOG_KART_CARI
                         WHERE CARI_ID = :ID
                         ORDER BY LOG_TIME DESC";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                    using (OracleDataAdapter da = new(cmd))
                        da.Fill(dt);
                }
            }

            ViewBag.CariID = id;
            return View("~/Views/Cari/AdminCari/Log.cshtml", dt);
        }

        public IActionResult ExportCariLogCsv(int id)
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
                            CARI_ID, 
                            CARI_SIRKET, 
                            CARI_TIP, 
                            OZEL_KOD
                         FROM LOG_KART_CARI
                         WHERE CARI_ID = :ID
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
            return File(bytes, "text/csv", $"cari_log_{id}.csv");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace WebProject.Controllers.Admin
{
    public class AdminSiparisController : Controller
    {
        private readonly string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string yil = "T", string tur = "T", int sayfa = 1)
        {
            DataTable dt = new();
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();

                string sql = @"SELECT 
                                  S.SIPARIS_ID, 
                                  C.CARI_SIRKET, 
                                  ST.STOK_AD, 
                                  S.MIKTAR, 
                                  S.SIPARIS_TARIHI, 
                                  S.SIPARIS_TUR 
                               FROM SIPARIS_DOSYA S
                               JOIN KART_CARI C ON S.CARI_ID = C.CARI_ID
                               JOIN KART_STOK ST ON S.STOK_ID = ST.STOK_ID
                               WHERE 1=1";

                var parameters = new List<OracleParameter>();

                if (yil != "T")
                {
                    sql += " AND TO_CHAR(S.SIPARIS_TARIHI, 'YYYY') = :yil";
                    parameters.Add(new OracleParameter(":yil", yil));
                }

                if (tur != "T")
                {
                    sql += " AND S.SIPARIS_TUR = :tur";
                    parameters.Add(new OracleParameter(":tur", tur));
                }

                sql += " ORDER BY S.SIPARIS_ID";

                using OracleCommand cmd = new(sql, conn);
                if (parameters.Any())
                    cmd.Parameters.AddRange(parameters.ToArray());

                using OracleDataAdapter da = new(cmd);
                da.Fill(dt);
            }

            ViewBag.Yil = yil;
            ViewBag.Tur = tur;
            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = 1;
            return View("~/Views/Siparis/AdminSiparis/Index.cshtml", dt);
        }

        public IActionResult ExportSiparisCsv(string yil = "T", string tur = "T")
        {
            try
            {
                DataTable dt = new();
                using (OracleConnection conn = new(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT S.SIPARIS_ID, C.CARI_SIRKET, ST.STOK_AD, S.MIKTAR, S.SIPARIS_TARIHI, S.SIPARIS_TUR 
                                   FROM SIPARIS_DOSYA S
                                   JOIN KART_CARI C ON S.CARI_ID = C.CARI_ID
                                   JOIN KART_STOK ST ON S.STOK_ID = ST.STOK_ID
                                   WHERE 1=1";

                    var parameters = new List<OracleParameter>();

                    if (yil != "T")
                    {
                        sql += " AND TO_CHAR(S.SIPARIS_TARIHI, 'YYYY') = :yil";
                        parameters.Add(new OracleParameter(":yil", yil));
                    }

                    if (tur != "T")
                    {
                        sql += " AND S.SIPARIS_TUR = :tur";
                        parameters.Add(new OracleParameter(":tur", tur));
                    }

                    using OracleCommand cmd = new(sql, conn);
                    if (parameters.Any())
                        cmd.Parameters.AddRange(parameters.ToArray());

                    using OracleDataAdapter da = new(cmd);
                    da.Fill(dt);
                }

                var sb = new StringBuilder();
                sb.AppendLine("SIPARIS_ID;CARI_SIRKET;STOK_AD;MIKTAR;SIPARIS_TARIHI;SIPARIS_TUR");
                foreach (DataRow row in dt.Rows)
                {
                    var siparisId = row["SIPARIS_ID"]?.ToString() ?? "";
                    var cariSirket = row["CARI_SIRKET"]?.ToString()?.Replace(";", ",") ?? "";
                    var stokAd = row["STOK_AD"]?.ToString()?.Replace(";", ",") ?? "";
                    var miktar = row["MIKTAR"]?.ToString() ?? "";
                    var siparisTarihi = row["SIPARIS_TARIHI"] != DBNull.Value ?
                        Convert.ToDateTime(row["SIPARIS_TARIHI"]).ToString("dd.MM.yyyy") : "";
                    var siparisTur = row["SIPARIS_TUR"]?.ToString() ?? "";

                    sb.AppendLine($"{siparisId};{cariSirket};{stokAd};{miktar};{siparisTarihi};{siparisTur}");
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", "siparisler.csv");
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON response döndür
                return Json(new { success = false, message = "CSV export hatası: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Yeni()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                // Carileri Yükle
                using (OracleCommand cmd = new OracleCommand("SELECT CARI_ID, CARI_SIRKET FROM KART_CARI", conn))
                {
                    DataTable cariler = new DataTable();
                    new OracleDataAdapter(cmd).Fill(cariler);
                    ViewBag.Cariler = new SelectList(cariler.AsEnumerable().Select(r => new { Value = r["CARI_ID"].ToString(), Text = r["CARI_SIRKET"].ToString() }), "Value", "Text");
                }

                // Stokları Yükle
                using (OracleCommand cmd = new OracleCommand("SELECT STOK_ID, STOK_AD FROM KART_STOK", conn))
                {
                    DataTable stoklar = new DataTable();
                    new OracleDataAdapter(cmd).Fill(stoklar);
                    ViewBag.Stoklar = new SelectList(stoklar.AsEnumerable().Select(r => new { Value = r["STOK_ID"].ToString(), Text = r["STOK_AD"].ToString() }), "Value", "Text");
                }
            }
            return View("~/Views/Siparis/AdminSiparis/Yeni.cshtml");
        }

        [HttpPost]
        public IActionResult Yeni(IFormCollection form)
        {
            try
            {
                using OracleConnection conn = new(connectionString);
                conn.Open();

                int stokId;
                
                // Eğer yeni stok ekleniyorsa
                if (form["STOK_ID"] == "NEW")
                {
                    // Önce yeni stoku ekle
                    string stokQuery = @"INSERT INTO KART_STOK (STOK_AD, OZEL_KOD, ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI) 
                                        VALUES (:STOK_AD, :OZEL_KOD, 0, 0) 
                                        RETURNING STOK_ID INTO :STOK_ID";
                    
                    using OracleCommand stokCmd = new(stokQuery, conn);
                    stokCmd.Parameters.Add(":STOK_AD", OracleDbType.Varchar2).Value = form["NEW_STOK_AD"];
                    stokCmd.Parameters.Add(":OZEL_KOD", OracleDbType.Varchar2).Value = form["NEW_OZEL_KOD"];
                    stokCmd.Parameters.Add(":STOK_ID", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    stokCmd.ExecuteNonQuery();
                    
                    stokId = Convert.ToInt32(stokCmd.Parameters[":STOK_ID"].Value);
                }
                else
                {
                    stokId = Convert.ToInt32(form["STOK_ID"]);
                }

                // Siparişi ekle
                string query = @"INSERT INTO SIPARIS_DOSYA 
                                 (CARI_ID, BIRIM, STOK_ID, MIKTAR, SIPARIS_TARIHI, SIPARIS_TUR) 
                                 VALUES (:CARI_ID, :BIRIM, :STOK_ID, :MIKTAR, :SIPARIS_TARIHI, :SIPARIS_TUR)";

                using OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(":CARI_ID", OracleDbType.Int32).Value = Convert.ToInt32(form["CARI_ID"]);
                cmd.Parameters.Add(":BIRIM", OracleDbType.Varchar2).Value = form["BIRIM"];
                cmd.Parameters.Add(":STOK_ID", OracleDbType.Int32).Value = stokId;
                cmd.Parameters.Add(":MIKTAR", OracleDbType.Decimal).Value = Convert.ToDecimal(form["MIKTAR"]);
                cmd.Parameters.Add(":SIPARIS_TARIHI", OracleDbType.Date).Value = Convert.ToDateTime(form["SIPARIS_TARIHI"]);
                cmd.Parameters.Add(":SIPARIS_TUR", OracleDbType.Char).Value = form["SIPARIS_TUR"];
                cmd.ExecuteNonQuery();

                // Stok miktarlarını güncelle
                string updateStokQuery = @"UPDATE KART_STOK 
                                          SET ALINAN_SIPARIS_MIKTARI = ALINAN_SIPARIS_MIKTARI + CASE WHEN :SIPARIS_TUR = 'A' THEN :MIKTAR ELSE 0 END,
                                              VERILEN_SIPARIS_MIKTARI = VERILEN_SIPARIS_MIKTARI + CASE WHEN :SIPARIS_TUR = 'V' THEN :MIKTAR ELSE 0 END
                                          WHERE STOK_ID = :STOK_ID";
                
                using OracleCommand updateCmd = new(updateStokQuery, conn);
                updateCmd.Parameters.Add(":SIPARIS_TUR", OracleDbType.Char).Value = form["SIPARIS_TUR"];
                updateCmd.Parameters.Add(":MIKTAR", OracleDbType.Decimal).Value = Convert.ToDecimal(form["MIKTAR"]);
                updateCmd.Parameters.Add(":STOK_ID", OracleDbType.Int32).Value = stokId;
                updateCmd.ExecuteNonQuery();

                string successMessage = form["STOK_ID"] == "NEW" 
                    ? "Yeni stok ve sipariş başarıyla eklendi." 
                    : "Yeni sipariş başarıyla eklendi.";
                    
                TempData["SuccessMessage"] = successMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sipariş eklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Yeni");
            }
        }

        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            DataTable dt = new();
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();

                // Sipariş verisini çek
                string query = "SELECT * FROM SIPARIS_DOSYA WHERE SIPARIS_ID = :ID";
                using (OracleCommand cmd = new(query, conn))
                {
                    cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                    using (OracleDataAdapter adapter = new(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                    return RedirectToAction("Index");
                }

                var siparisRow = dt.Rows[0];
                var selectedCariId = siparisRow["CARI_ID"].ToString();
                var selectedStokId = siparisRow["STOK_ID"].ToString();

                // Carileri Yükle
                using (OracleCommand cmd = new OracleCommand("SELECT CARI_ID, CARI_SIRKET FROM KART_CARI", conn))
                {
                    DataTable cariler = new DataTable();
                    new OracleDataAdapter(cmd).Fill(cariler);
                    ViewBag.Cariler = new SelectList(cariler.AsEnumerable().Select(r => new { Value = r["CARI_ID"].ToString(), Text = r["CARI_SIRKET"].ToString() }), "Value", "Text", selectedCariId);
                }

                // Stokları Yükle
                using (OracleCommand cmd = new OracleCommand("SELECT STOK_ID, STOK_AD FROM KART_STOK", conn))
                {
                    DataTable stoklar = new DataTable();
                    new OracleDataAdapter(cmd).Fill(stoklar);
                    ViewBag.Stoklar = new SelectList(stoklar.AsEnumerable().Select(r => new { Value = r["STOK_ID"].ToString(), Text = r["STOK_AD"].ToString() }), "Value", "Text", selectedStokId);
                }
            }

            return View("~/Views/Siparis/AdminSiparis/Guncelle.cshtml", dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Guncelle(int id, IFormCollection form)
        {
            try
            {
                using OracleConnection conn = new(connectionString);
                conn.Open();

                string query = @"UPDATE SIPARIS_DOSYA 
                                 SET CARI_ID = :CARI_ID, 
                                     BIRIM = :BIRIM, 
                                     STOK_ID = :STOK_ID, 
                                     MIKTAR = :MIKTAR, 
                                     SIPARIS_TARIHI = :SIPARIS_TARIHI, 
                                     SIPARIS_TUR = :SIPARIS_TUR 
                                 WHERE SIPARIS_ID = :ID";

                using OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(":CARI_ID", OracleDbType.Int32).Value = Convert.ToInt32(form["CARI_ID"]);
                cmd.Parameters.Add(":BIRIM", OracleDbType.Varchar2).Value = form["BIRIM"];
                cmd.Parameters.Add(":STOK_ID", OracleDbType.Int32).Value = Convert.ToInt32(form["STOK_ID"]);
                cmd.Parameters.Add(":MIKTAR", OracleDbType.Decimal).Value = Convert.ToDecimal(form["MIKTAR"]);
                cmd.Parameters.Add(":SIPARIS_TARIHI", OracleDbType.Date).Value = Convert.ToDateTime(form["SIPARIS_TARIHI"]);
                cmd.Parameters.Add(":SIPARIS_TUR", OracleDbType.Char).Value = form["SIPARIS_TUR"];
                cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = "Sipariş başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorMessage"] = "Güncelleme sırasında hata oluştu.";
                return RedirectToAction("Guncelle", new { id });
            }
        }

        public IActionResult Sil(int id)
        {
            try
            {
                using OracleConnection conn = new(connectionString);
                conn.Open();

                string query = "DELETE FROM SIPARIS_DOSYA WHERE SIPARIS_ID = :ID";
                using OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;
                int result = cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = result > 0 ? "Sipariş silindi." : "Silinecek sipariş bulunamadı.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorMessage"] = "Sipariş silinirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Log(int id)
        {
            DataTable dt = new();

            using OracleConnection conn = new(connectionString);
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            SIPARIS_ID, 
                            CARI_ID, 
                            BIRIM, 
                            STOK_ID, 
                            MIKTAR, 
                            TO_CHAR(SIPARIS_TARIHI, 'DD.MM.YYYY') AS SIPARIS_TARIHI, 
                            SIPARIS_TUR
                         FROM LOG_SIPARIS_DOSYA
                         WHERE SIPARIS_ID = :ID
                         ORDER BY LOG_TIME DESC";

                using OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;

                using OracleDataAdapter da = new(cmd);
                da.Fill(dt);
            }

            ViewBag.SiparisID = id;
            return View("~/Views/Siparis/AdminSiparis/Log.cshtml", dt);
        }

        public IActionResult ExportSiparisLogCsv(int id)
        {
            DataTable dt = new();

            using OracleConnection conn = new(connectionString);
            {
                conn.Open();
                string query = @"SELECT 
                            LOG_OP, 
                            LOG_TIME, 
                            LOG_SCHEMA, 
                            LOG_TERMINAL, 
                            LOG_USER, 
                            LOG_IP, 
                            SIPARIS_ID, 
                            CARI_ID, 
                            BIRIM, 
                            STOK_ID, 
                            MIKTAR, 
                            TO_CHAR(SIPARIS_TARIHI, 'DD.MM.YYYY') AS SIPARIS_TARIHI, 
                            SIPARIS_TUR
                         FROM LOG_SIPARIS_DOSYA
                         WHERE SIPARIS_ID = :ID
                         ORDER BY LOG_TIME DESC";

                using OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(":ID", OracleDbType.Int32).Value = id;

                using OracleDataAdapter da = new(cmd);
                da.Fill(dt);
            }

            var csv = new System.Text.StringBuilder();

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

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"siparis_log_{id}.csv");
        }
    }
}
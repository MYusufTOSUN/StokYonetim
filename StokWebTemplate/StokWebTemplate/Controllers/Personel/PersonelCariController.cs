using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace StokWeb.Controllers.Personel
{
    public class PersonelCariController : Controller
    {
        private readonly string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string tip = "T", int sayfa = 1)
        {
            int sayfaBoyutu = 10;
            DataTable dt = new DataTable();
            int toplamKayit = 0;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                StringBuilder sqlBuilder = new StringBuilder("FROM KART_CARI WHERE 1=1 ");
                if (tip != "T")
                {
                    sqlBuilder.Append("AND CARI_TIP = :tip ");
                }

                // Toplam kayıt sayısı
                string countSql = "SELECT COUNT(*) " + sqlBuilder.ToString();
                OracleCommand countCmd = new OracleCommand(countSql, conn);
                if (tip != "T")
                {
                    countCmd.Parameters.Add(new OracleParameter("tip", tip));
                }
                toplamKayit = Convert.ToInt32(countCmd.ExecuteScalar());

                // Sayfalı veri sorgusu
                string sql = $@"
                    SELECT * FROM (
                        SELECT 
                            CARI_ID, CARI_SIRKET, CARI_TIP, OZEL_KOD,
                            ROW_NUMBER() OVER (ORDER BY CARI_ID) AS RN
                        {sqlBuilder}
                    )
                    WHERE RN > :alt AND RN <= :ust
                    ORDER BY CARI_ID";

                OracleCommand cmd = new OracleCommand(sql, conn);
                if (tip != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("tip", tip));
                }
                cmd.Parameters.Add(":alt", OracleDbType.Int32).Value = (sayfa - 1) * sayfaBoyutu;
                cmd.Parameters.Add(":ust", OracleDbType.Int32).Value = sayfa * sayfaBoyutu;

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }

            ViewBag.ToplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);
            ViewBag.SayfaNo = sayfa;
            ViewBag.Tip = tip;
            return View("~/Views/Cari/PersonelCari/Index.cshtml", dt);
        }

        public IActionResult ExportCariCsv(string tip = "T")
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                StringBuilder sqlBuilder = new StringBuilder("SELECT CARI_ID, CARI_SIRKET, CARI_TIP, OZEL_KOD FROM KART_CARI WHERE 1=1 ");

                if (tip != "T")
                {
                    sqlBuilder.Append("AND CARI_TIP = :tip ");
                }
                sqlBuilder.Append("ORDER BY CARI_ID");

                OracleCommand cmd = new OracleCommand(sqlBuilder.ToString(), conn);

                if (tip != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("tip", tip));
                }

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }

            var sb = new StringBuilder();
            sb.AppendLine("CARI_ID;CARI_SIRKET;CARI_TIP;OZEL_KOD");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"{row["CARI_ID"]};{row["CARI_SIRKET"]};{(row["CARI_TIP"].ToString() == "A" ? "Alıcı" : "Satıcı")};{row["OZEL_KOD"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "cariler.csv");
        }
    }
}

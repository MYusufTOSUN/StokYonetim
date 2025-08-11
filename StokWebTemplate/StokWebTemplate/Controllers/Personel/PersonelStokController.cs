using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace StokWeb.Controllers.Personel
{
    public class PersonelStokController : Controller
    {
        string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string stokad = "T", int sayfa = 1)
        {
            int sayfaBoyutu = 20;
            int toplamKayit = 0;
            int toplamSayfa = 0;
            DataTable dt = new DataTable();

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();

                // Stok adlarını ViewBag'e ekle
                string stokAdlariSql = "SELECT DISTINCT STOK_AD FROM KART_STOK ORDER BY STOK_AD";
                using (OracleCommand stokAdlariCmd = new OracleCommand(stokAdlariSql, conn))
                {
                    using (OracleDataReader reader = stokAdlariCmd.ExecuteReader())
                    {
                        var stokAdlari = new List<string>();
                        while (reader.Read())
                        {
                            stokAdlari.Add(reader["STOK_AD"].ToString());
                        }
                        ViewBag.StokAdlari = stokAdlari;
                    }
                }

                // Filtreleme için WHERE koşulu
                string whereCondition = "";
                if (stokad != "T")
                {
                    whereCondition = "WHERE STOK_AD = :stokad";
                }

                string countSql = $"SELECT COUNT(*) FROM KART_STOK {whereCondition}";
                using (OracleCommand countCmd = new OracleCommand(countSql, conn))
                {
                    if (stokad != "T")
                    {
                        countCmd.Parameters.Add(new OracleParameter("stokad", stokad));
                    }
                    toplamKayit = Convert.ToInt32(countCmd.ExecuteScalar());
                }

                toplamSayfa = (int)Math.Ceiling((double)toplamKayit / sayfaBoyutu);

                string sql = $@"
                    SELECT * FROM (
                    SELECT 
                    STOK_ID, STOK_AD, BIRIM, OZEL_KOD,
                    ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI,
                    ROW_NUMBER() OVER (ORDER BY STOK_ID) AS RN
                    FROM KART_STOK {whereCondition})
                    WHERE RN > :alt AND RN <= :ust
                    ORDER BY STOK_ID";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    if (stokad != "T")
                    {
                        cmd.Parameters.Add(new OracleParameter("stokad", stokad));
                    }
                    cmd.Parameters.Add(":alt", OracleDbType.Int32).Value = (sayfa - 1) * sayfaBoyutu;
                    cmd.Parameters.Add(":ust", OracleDbType.Int32).Value = sayfa * sayfaBoyutu;

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;
            ViewBag.StokAd = stokad;
            return View("~/Views/Stok/PersonelStok/Index.cshtml", dt);
        }

        public IActionResult ExportStokCsv(string stokad = "T")
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                StringBuilder sqlBuilder = new StringBuilder("SELECT STOK_ID, STOK_AD, BIRIM, OZEL_KOD, ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI FROM KART_STOK WHERE 1=1 ");

                if (stokad != "T")
                {
                    sqlBuilder.Append("AND STOK_AD = :stokad ");
                }
                sqlBuilder.Append("ORDER BY STOK_ID");

                OracleCommand cmd = new OracleCommand(sqlBuilder.ToString(), conn);

                if (stokad != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("stokad", stokad));
                }

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }

            var sb = new StringBuilder();
            sb.AppendLine("STOK_ID;STOK_AD;BIRIM;OZEL_KOD;ALINAN_SIPARIS_MIKTARI;VERILEN_SIPARIS_MIKTARI");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"{row["STOK_ID"]};{row["STOK_AD"]};{row["BIRIM"]};{row["OZEL_KOD"]};{row["ALINAN_SIPARIS_MIKTARI"]};{row["VERILEN_SIPARIS_MIKTARI"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "stoklar.csv");
        }
    }
}

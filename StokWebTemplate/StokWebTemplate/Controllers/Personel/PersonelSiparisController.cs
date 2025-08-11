using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace StokWeb.Controllers.Personel
{
    public class PersonelSiparisController : Controller
    {
        string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=YUSUF;Password=9812;";

        public IActionResult Index(string yil = "T", string tur = "T", int sayfa = 1)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                StringBuilder sqlBuilder = new StringBuilder(@"SELECT 
                                   S.SIPARIS_ID, 
                                   C.CARI_SIRKET, 
                                   ST.STOK_AD, 
                                   S.MIKTAR, 
                                   S.SIPARIS_TARIHI, 
                                   S.SIPARIS_TUR 
                               FROM SIPARIS_DOSYA S
                               JOIN KART_CARI C ON S.CARI_ID = C.CARI_ID
                               JOIN KART_STOK ST ON S.STOK_ID = ST.STOK_ID WHERE 1=1 ");

                if (yil != "T")
                {
                    sqlBuilder.Append("AND EXTRACT(YEAR FROM S.SIPARIS_TARIHI) = :yil ");
                }
                if (tur != "T")
                {
                    sqlBuilder.Append("AND S.SIPARIS_TUR = :tur ");
                }
                sqlBuilder.Append("ORDER BY S.SIPARIS_ID DESC");

                OracleCommand cmd = new OracleCommand(sqlBuilder.ToString(), conn);

                if (yil != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("yil", yil));
                }
                if (tur != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("tur", tur));
                }

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }

            int pageSize = 10;
            var pagedData = dt.AsEnumerable().Skip((sayfa - 1) * pageSize).Take(pageSize).CopyToDataTable();

            ViewBag.ToplamSayfa = (int)Math.Ceiling(dt.Rows.Count / (double)pageSize);
            ViewBag.SayfaNo = sayfa;
            ViewBag.Yil = yil;
            ViewBag.Tur = tur;

            return View("~/Views/Siparis/PersonelSiparis/Index.cshtml", pagedData);
        }

        public IActionResult ExportSiparisCsv(string yil = "T", string tur = "T")
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                StringBuilder sqlBuilder = new StringBuilder(@"SELECT 
                               S.SIPARIS_ID, 
                               C.CARI_SIRKET, 
                               ST.STOK_AD, 
                               S.MIKTAR, 
                               S.SIPARIS_TARIHI, 
                               S.SIPARIS_TUR 
                           FROM SIPARIS_DOSYA S
                           JOIN KART_CARI C ON S.CARI_ID = C.CARI_ID
                           JOIN KART_STOK ST ON S.STOK_ID = ST.STOK_ID WHERE 1=1 ");

                if (yil != "T")
                {
                    sqlBuilder.Append("AND EXTRACT(YEAR FROM S.SIPARIS_TARIHI) = :yil ");
                }
                if (tur != "T")
                {
                    sqlBuilder.Append("AND S.SIPARIS_TUR = :tur ");
                }
                sqlBuilder.Append("ORDER BY S.SIPARIS_ID DESC");

                OracleCommand cmd = new OracleCommand(sqlBuilder.ToString(), conn);

                if (yil != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("yil", yil));
                }
                if (tur != "T")
                {
                    cmd.Parameters.Add(new OracleParameter("tur", tur));
                }

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }

            var sb = new StringBuilder();
            sb.AppendLine("SIPARIS_ID;CARI_SIRKET;STOK_AD;MIKTAR;SIPARIS_TARIHI;SIPARIS_TUR");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"{row["SIPARIS_ID"]};{row["CARI_SIRKET"]};{row["STOK_AD"]};{row["MIKTAR"]};{Convert.ToDateTime(row["SIPARIS_TARIHI"]):dd.MM.yyyy};{(row["SIPARIS_TUR"].ToString() == "A" ? "Alınan" : "Verilen")}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "personel_siparisler.csv");
        }
    }
}

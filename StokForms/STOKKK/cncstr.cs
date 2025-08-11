using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace Stok
{
    public class cncstr
    {
        public OracleConnection con;
        private string connStr = "User Id=YUSUF;Password=9812;Data Source=127.0.0.1:1521/XE;";

        public bool DB()
        {
            try
            {
                con = new OracleConnection(connStr);
                con.Open();

                if (con.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Oracle bağlantısı açılamadı.");
                    return false;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle bağlantı hatası: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Genel hata: " + ex.Message);
                return false;
            }
        }

        public void BaglantiKapat()
        {
            if (con != null && con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
    }
}

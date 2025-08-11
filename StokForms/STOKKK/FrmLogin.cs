using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Stok
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        cncstr baglanti = new cncstr();

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (baglanti.DB())
            {
                string sql = "SELECT COUNT(*) FROM LOGIN_KULLANICI WHERE KULLANICI_ADI = :kadi AND SIFRE = :sifre";
                OracleCommand cmd = new OracleCommand(sql, baglanti.con);
                cmd.Parameters.Add("kadi", txtKadi.Text.Trim());
                cmd.Parameters.Add("sifre", txtSifre.Text.Trim());

                int sonuc = Convert.ToInt32(cmd.ExecuteScalar());

                if (sonuc > 0)
                {
                    MessageBox.Show("Giriş başarılı!");
                    this.Hide();
                    FrmMenu menu = new FrmMenu(txtKadi.Text.Trim());
                    menu.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                }

                baglanti.BaglantiKapat();
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

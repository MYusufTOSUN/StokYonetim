using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Stok
{
    public partial class FrmSiparisSil : Form
    {
        cncstr baglanti = new cncstr();

        public FrmSiparisSil()
        {
            InitializeComponent();
            if (baglanti.DB())
            {
                this.Text += " (Bağlantı Aktif)";
                baglanti.BaglantiKapat();
            }
            else
            {
                this.Text += " (Bağlantı Yok)";
            }

            this.Text = "Siparis Sil";
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSiparisId.Text))
            {
                MessageBox.Show("Lütfen bir Siparis ID giriniz.");
                return;
            }

            if (!int.TryParse(txtSiparisId.Text.Trim(), out int siparisId))
            {
                MessageBox.Show("Geçerli bir sayısal ID giriniz.");
                return;
            }

            var onay = MessageBox.Show("Bu siparişi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (onay != DialogResult.Yes) return;

            if (!baglanti.DB()) return;

            try
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM SIPARIS_DOSYA WHERE SIPARIS_ID = :id", baglanti.con);
                cmd.Parameters.Add("id", siparisId);

                int silinen = cmd.ExecuteNonQuery();
                if (silinen > 0)
                    MessageBox.Show("Sipariş başarıyla silindi.");
                else
                    MessageBox.Show("Belirtilen ID'ye ait sipariş bulunamadı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.BaglantiKapat();
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

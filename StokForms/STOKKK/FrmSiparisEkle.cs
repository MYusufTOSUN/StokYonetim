using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Stok
{
    public partial class FrmSiparisEkle : Form
    {
        cncstr baglanti = new cncstr();
        private int yeniStokId;

        public FrmSiparisEkle()
        {
            InitializeComponent();
            this.Text = "Yeni Siparis Olustur";
            this.BackColor = System.Drawing.Color.Gainsboro;
            txtYeniStokAdi.Visible = false;
            lblYeniStokAdi.Visible = false;
            txtOzelKod.Visible = false;
            lblOzelKod.Visible = false;
            cmbBirim.Visible = false;
            lblBirim.Visible = false;
        }



        private void FrmSiparis_Load(object sender, EventArgs e)
        {
            
            if (baglanti.DB())
            {
                this.Text += " (Bağlantı Aktif)";
                baglanti.BaglantiKapat();
            }
            else
            {
                this.Text += " (Bağlantı Yok)";
            }

            if (!baglanti.DB()) return;

            // Cari
            OracleDataAdapter daCari = new OracleDataAdapter("SELECT CARI_ID, CARI_SIRKET FROM KART_CARI", baglanti.con);
            DataTable dtCari = new DataTable();
            daCari.Fill(dtCari);
            cmbCari.DataSource = dtCari;
            cmbCari.DisplayMember = "CARI_SIRKET";
            cmbCari.ValueMember = "CARI_ID";
            cmbCari.SelectedIndex = -1;

            // Stok
            OracleDataAdapter daStok = new OracleDataAdapter("SELECT STOK_ID, STOK_AD FROM KART_STOK ORDER BY STOK_ID", baglanti.con);
            DataTable dtStok = new DataTable();
            daStok.Fill(dtStok);
            dtStok.Columns.Add("GOSTERIM", typeof(string));
            foreach (DataRow row in dtStok.Rows)
            {
                row["GOSTERIM"] = $"{row["STOK_ID"]} - {row["STOK_AD"]}";
            }

            // Yeni stok satiri ekle
            yeniStokId = dtStok.Rows.Count > 0 ? Convert.ToInt32(dtStok.Rows[dtStok.Rows.Count - 1]["STOK_ID"]) + 1 : 1;
            DataRow yeniSatir = dtStok.NewRow();
            yeniSatir["STOK_ID"] = yeniStokId;
            yeniSatir["STOK_AD"] = "Yeni";
            yeniSatir["GOSTERIM"] = $"{yeniStokId} (Yeni)";
            dtStok.Rows.Add(yeniSatir);

            cmbStok.DataSource = dtStok;
            cmbStok.DisplayMember = "GOSTERIM";
            cmbStok.ValueMember = "STOK_ID";
            cmbStok.SelectedIndex = -1;
            cmbStok.SelectedIndexChanged += cmbStok_SelectedIndexChanged;

            // Birim
            OracleDataAdapter daBirim = new OracleDataAdapter("SELECT BIRIM, BIRIM_AD FROM TANIM_BIRIM", baglanti.con);
            DataTable dtBirim = new DataTable();
            daBirim.Fill(dtBirim);
            cmbBirim.DataSource = dtBirim;
            cmbBirim.DisplayMember = "BIRIM_AD";
            cmbBirim.ValueMember = "BIRIM";
            cmbBirim.SelectedIndex = -1;

            dateSiparis.Value = DateTime.Today;
            baglanti.BaglantiKapat();
        }

        private void cmbStok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStok.SelectedValue == null) return;

            int seciliStokId = Convert.ToInt32(cmbStok.SelectedValue);

            if (seciliStokId == yeniStokId)
            {
                lblYeniStokAdi.Visible = true;
                txtYeniStokAdi.Visible = true;
                lblOzelKod.Visible = true;
                txtOzelKod.Visible = true;

                cmbBirim.Visible = true;
                cmbBirim.Enabled = true;
                lblBirim.Visible = true;
            }
            else
            {
                lblYeniStokAdi.Visible = false;
                txtYeniStokAdi.Visible = false;
                lblOzelKod.Visible = false;
                txtOzelKod.Visible = false;

                // cmbBirim gizlenir ancak eski değeri getirilir (kullanıcı görmez ama INSERT'te kullanılır)
                cmbBirim.Visible = false;
                lblBirim.Visible = false;

                if (baglanti.DB())
                {
                    try
                    {
                        string sql = "SELECT BIRIM FROM KART_STOK WHERE STOK_ID = :id";
                        OracleCommand cmd = new OracleCommand(sql, baglanti.con);
                        cmd.Parameters.Add("id", seciliStokId);
                        object birimDegeri = cmd.ExecuteScalar();
                        if (birimDegeri != null)
                            cmbBirim.SelectedValue = birimDegeri.ToString();
                    }
                    catch
                    {
                        // hata olursa sessiz geç
                    }
                    finally
                    {
                        baglanti.BaglantiKapat();
                    }
                }
            }
        }

        private void txtMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                e.Handled = true;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (cmbCari.SelectedIndex == -1 || cmbStok.SelectedIndex == -1 || cmbBirim.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtMiktar.Text) || (!rbAlim.Checked && !rbVerim.Checked))
            {
                MessageBox.Show("Lutfen tum alanlari doldurun.");
                return;
            }

            string tur = rbAlim.Checked ? "A" : "V";

            if (!baglanti.DB()) return;

            int secilenStokId = Convert.ToInt32(cmbStok.SelectedValue);

            // Yeni stok girilecekse önce KART_STOK'a ekle
            if (secilenStokId == yeniStokId)
            {
                string yeniAd = txtYeniStokAdi.Text.Trim();
                string ozelKod = txtOzelKod.Text.Trim();

                if (string.IsNullOrWhiteSpace(yeniAd))
                {
                    MessageBox.Show("Yeni stok adi giriniz.");
                    baglanti.BaglantiKapat();
                    return;
                }

                if (string.IsNullOrWhiteSpace(ozelKod) || !ozelKod.StartsWith("OK"))
                {
                    MessageBox.Show("Ozel kod 'OK' ile baslamali.");
                    baglanti.BaglantiKapat();
                    return;
                }

                decimal miktar = Convert.ToDecimal(txtMiktar.Text);
                decimal alinan = tur == "A" ? miktar : 0;
                decimal verilen = tur == "V" ? miktar : 0;

                OracleCommand stokCmd = new OracleCommand(@"
        INSERT INTO KART_STOK (STOK_ID, STOK_AD, BIRIM, ALINAN_SIPARIS_MIKTARI, VERILEN_SIPARIS_MIKTARI, OZEL_KOD)
        VALUES (:id, :ad, :birim, :alinan, :verilen, :ozelKod)", baglanti.con);

                stokCmd.Parameters.Add("id", yeniStokId);
                stokCmd.Parameters.Add("ad", yeniAd);
                stokCmd.Parameters.Add("birim", cmbBirim.SelectedValue);
                stokCmd.Parameters.Add("alinan", alinan);
                stokCmd.Parameters.Add("verilen", verilen);
                stokCmd.Parameters.Add("ozelKod", ozelKod);
                stokCmd.ExecuteNonQuery();
            }


            OracleCommand cmd = new OracleCommand(@"
                INSERT INTO SIPARIS_DOSYA (CARI_ID, STOK_ID, BIRIM, MIKTAR, SIPARIS_TARIHI, SIPARIS_TUR)
                VALUES (:cari, :stok, :birim, :miktar, :tarih, :tur)", baglanti.con);

            cmd.Parameters.Add("cari", cmbCari.SelectedValue);
            cmd.Parameters.Add("stok", secilenStokId);
            cmd.Parameters.Add("birim", cmbBirim.SelectedValue);
            cmd.Parameters.Add("miktar", Convert.ToDecimal(txtMiktar.Text));
            cmd.Parameters.Add("tarih", dateSiparis.Value);
            cmd.Parameters.Add("tur", tur);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Siparis basariyla eklendi.");
                txtMiktar.Clear();
                cmbCari.SelectedIndex = -1;
                cmbStok.SelectedIndex = -1;
                cmbBirim.SelectedIndex = -1;
                rbAlim.Checked = false;
                rbVerim.Checked = false;
                txtYeniStokAdi.Clear();
                txtYeniStokAdi.Visible = false;
                lblYeniStokAdi.Visible = false;
                dateSiparis.Value = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

            baglanti.BaglantiKapat();
        }

        private void btnSiparisCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

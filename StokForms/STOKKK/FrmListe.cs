using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Stok
{
    public partial class FrmListe : Form
    {
        cncstr baglanti = new cncstr();

        public FrmListe()
        {
            InitializeComponent();
            this.Text = "Listeleme";
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Load += FrmListe_Load;
            btnAra.Click += btnAra_Click;
            btnAra2.Click += btnAra2_Click;
            btnAra3.Click += btnAra3_Click;
            btnCikis.Click += btnCikis_Click;
            kopyalaToolStripMenuItem.Click += kopyalaToolStripMenuItem_Click;
            rdbTablo.CheckedChanged += rdb_CheckedChanged;
            rdbVeritabani.CheckedChanged += rdb_CheckedChanged;
        }

        private void FrmListe_Load(object sender, EventArgs e)
        {
            cmbTablolar.Visible = false;
            cmbTablolar.Enabled = false;

            if (baglanti.DB())
            {
                this.Text = "Listeleme (Bağlantı Aktif)";
                try
                {
                    OracleDataAdapter da = new OracleDataAdapter("SELECT table_name FROM user_tables", baglanti.con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbTablolar.DataSource = dt;
                    cmbTablolar.DisplayMember = "table_name";
                    cmbTablolar.ValueMember = "table_name";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Tablo listesi alınamadı: " + ex.Message);
                }
                finally { baglanti.BaglantiKapat(); }
            }
            else
            {
                this.Text = "Listeleme (Bağlantı Yok)";
            }
        }


        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            cmbTablolar.Visible = rdbTablo.Checked;
            cmbTablolar.Enabled = rdbTablo.Checked;
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string girilen = txtSiparisId.Text.Trim();

            if (string.IsNullOrWhiteSpace(girilen))
            {
                MessageBox.Show("Lütfen bir Siparis ID giriniz.");
                return;
            }

            if (!int.TryParse(girilen, out int siparisId))
            {
                MessageBox.Show("Siparis ID sayısal olmalıdır.");
                return;
            }

            if (!baglanti.DB()) return;

            string sql = @"
        SELECT 
            S.SIPARIS_ID,
            C.CARI_SIRKET AS CARI,
            K.STOK_AD AS STOK,
            B.BIRIM_AD AS BIRIM,
            S.MIKTAR,
            TO_CHAR(S.SIPARIS_TARIHI, 'DD.MM.YYYY') AS TARIH,
            CASE 
                WHEN S.SIPARIS_TUR = 'A' THEN 'Alim'
                WHEN S.SIPARIS_TUR = 'V' THEN 'Verim'
                ELSE 'Bilinmiyor'
            END AS TUR
        FROM SIPARIS_DOSYA S
        JOIN KART_CARI C ON S.CARI_ID = C.CARI_ID
        JOIN KART_STOK K ON S.STOK_ID = K.STOK_ID
        JOIN TANIM_BIRIM B ON S.BIRIM = B.BIRIM
        WHERE S.SIPARIS_ID = :siparisId";

            try
            {
                OracleCommand cmd = new OracleCommand(sql, baglanti.con);
                cmd.Parameters.Add("siparisId", siparisId);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSonuc.DataSource = dt;
                lblSonuc.Text = $"{dt.Rows.Count} kayıt listelendi.";

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Girilen ID'ye ait sipariş bulunamadı.");
                }
                else
                {
                    foreach (DataGridViewRow row in dgvSonuc.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            var tip = row.Cells["TUR"]?.Value?.ToString()?.ToUpperInvariant();
                            if (tip == "ALIM")
                                row.DefaultCellStyle.BackColor = Color.LightBlue;
                            else if (tip == "VERIM")
                                row.DefaultCellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
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



        private void btnAra2_Click(object sender, EventArgs e)
        {
            string girilen = txtCariListele.Text.Trim().ToUpper();

            if (!baglanti.DB()) return;

            string query;
            OracleDataAdapter da;

            if (girilen == "HEPSİ")
            {
                query = "SELECT * FROM KART_CARI";
                da = new OracleDataAdapter(query, baglanti.con);
            }
            else if (!int.TryParse(girilen, out int cariId))
            {
                MessageBox.Show("Geçerli bir Cari ID giriniz.");
                return;
            }
            else
            {
                query = "SELECT * FROM KART_CARI WHERE CARI_ID = :id";
                OracleCommand cmd = new OracleCommand(query, baglanti.con);
                cmd.Parameters.Add("id", cariId);
                da = new OracleDataAdapter(cmd);
            }

            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSonuc.DataSource = dt;
            lblSonuc.Text = $"{dt.Rows.Count} kayıt listelendi.";
            dgvSonuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Girilen ID'ye ait kayıt bulunamadı.");
            }
            else
            {
                foreach (DataGridViewRow row in dgvSonuc.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var tip = row.Cells["CARI_TIP"]?.Value?.ToString()?.ToUpper();
                        if (tip == "A")
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                        else if (tip == "V")
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }

            baglanti.BaglantiKapat();
        }

        private void btnAra3_Click(object sender, EventArgs e)
        {
            string girilen = txtStokID.Text.Trim();

            if (string.IsNullOrWhiteSpace(girilen))
            {
                MessageBox.Show("Lütfen Stok ID giriniz.");
                return;
            }

            if (!int.TryParse(girilen, out int stokId))
            {
                MessageBox.Show("Geçerli bir Stok ID giriniz.");
                return;
            }

            if (!baglanti.DB()) return;

            string sql = "SELECT * FROM KART_STOK WHERE STOK_ID = :id";
            try
            {
                OracleCommand cmd = new OracleCommand(sql, baglanti.con);
                cmd.Parameters.Add("id", stokId);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSonuc.DataSource = dt;
                lblSonuc.Text = $"{dt.Rows.Count} kayıt listelendi.";
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Girilen Stok ID’ye ait kayıt bulunamadı.");
                }
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


        private void btnScriptAl_Click(object sender, EventArgs e)
        {
            if (!baglanti.DB()) { MessageBox.Show("Bağlantı kurulamadı."); return; }

            List<string> tabloListesi = new List<string>();
            if (rdbVeritabani.Checked)
            {
                OracleCommand cmd = new OracleCommand("SELECT table_name FROM user_tables", baglanti.con);
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) tabloListesi.Add(dr.GetString(0));
                dr.Close();
            }
            else if (rdbTablo.Checked && !string.IsNullOrWhiteSpace(cmbTablolar.Text))
            {
                tabloListesi.Add(cmbTablolar.Text);
            }
            else
            {
                MessageBox.Show("Lütfen bir tablo seçiniz.");
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "SQL Dosyası|*.sql",
                FileName = "veri_script.sql"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                {
                    foreach (string tablo in tabloListesi)
                    {
                        string query = $"SELECT * FROM {tablo}";
                        OracleCommand cmd = new OracleCommand(query, baglanti.con);
                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            List<string> kolonlar = new List<string>();
                            List<string> degerler = new List<string>();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                kolonlar.Add(dr.GetName(i));
                                degerler.Add(dr.IsDBNull(i) ? "NULL" :
                                    (dr.GetFieldType(i) == typeof(string) || dr.GetFieldType(i) == typeof(DateTime))
                                    ? $"'{dr.GetValue(i).ToString().Replace("'", "''")}'"
                                    : dr.GetValue(i).ToString());
                            }
                            string insert = $"INSERT INTO {tablo} ({string.Join(", ", kolonlar)}) VALUES ({string.Join(", ", degerler)});";
                            writer.WriteLine(insert);
                        }
                        writer.WriteLine();
                        dr.Close();
                    }
                }
                MessageBox.Show("Veri scripti başarıyla oluşturuldu.");
            }

            baglanti.BaglantiKapat();
        }

        private void btnTxtAktar_Click(object sender, EventArgs e)
        {
            if (dgvSonuc.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı."); return;
            }
            if (dgvSonuc.Rows.Count == 1)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı."); return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Text Dosyası|*.txt",
                FileName = "veri.txt"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                {
                    List<string> basliklar = new List<string>();
                    foreach (DataGridViewColumn col in dgvSonuc.Columns)
                        basliklar.Add(col.HeaderText);
                    sw.WriteLine(string.Join("\t", basliklar));

                    foreach (DataGridViewRow row in dgvSonuc.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            List<string> hucreler = new List<string>();
                            foreach (DataGridViewCell cell in row.Cells)
                                hucreler.Add(cell.Value?.ToString());
                            sw.WriteLine(string.Join("\t", hucreler));
                        }
                    }
                }

                MessageBox.Show(".txt aktarımı başarılı!");
            }
        }

        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvSonuc.CurrentCell?.Value != null)
            {
                Clipboard.SetText(dgvSonuc.CurrentCell.Value.ToString());
                MessageBox.Show("Kopyalandı.");
            }
        }

        private void btnCikis_Click(object sender, EventArgs e) => this.Close();


    }
}

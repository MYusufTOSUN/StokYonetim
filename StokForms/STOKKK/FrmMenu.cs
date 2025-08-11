using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stok
{
    public partial class FrmMenu : Form
    {
        cncstr baglanti = new cncstr();

        private string kullaniciAdi;

        public FrmMenu(string kullaniciAdi)
        {

            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.Load += new System.EventHandler(this.FrmMenu_Load);

        }

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            lblTarihSaat.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit(); }

        private void FrmMenu_Load(object sender, EventArgs e)
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

            lblHosgeldin.Text = "Hoş geldiniz: " + kullaniciAdi;
            lblTarihSaat.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
            this.timerSaat.Tick += new System.EventHandler(this.timerSaat_Tick);

        }

        private void btnSiparisListele_Click(object sender, EventArgs e)
        {
            FrmListe liste = new FrmListe();
            liste.ShowDialog(); 
        }
        private void btnSiparisSil_Click(object sender, EventArgs e)
        {
            FrmSiparisSil silFormu = new FrmSiparisSil();
            silFormu.Show();
        }

        private void btnSiparisEkle_Click(object sender, EventArgs e)
        {
            FrmSiparisEkle siparisForm = new FrmSiparisEkle();
            siparisForm.ShowDialog(); 
        }
    }
}

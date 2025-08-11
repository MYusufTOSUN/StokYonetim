using System;
using System.Windows.Forms;

namespace Stok
{
    partial class FrmSiparisEkle
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblCari = new System.Windows.Forms.Label();
            this.lblStok = new System.Windows.Forms.Label();
            this.lblBirim = new System.Windows.Forms.Label();
            this.lblMiktar = new System.Windows.Forms.Label();
            this.lblTur = new System.Windows.Forms.Label();
            this.lblTarih = new System.Windows.Forms.Label();
            this.cmbCari = new System.Windows.Forms.ComboBox();
            this.cmbStok = new System.Windows.Forms.ComboBox();
            this.cmbBirim = new System.Windows.Forms.ComboBox();
            this.dateSiparis = new System.Windows.Forms.DateTimePicker();
            this.txtMiktar = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.rbAlim = new System.Windows.Forms.RadioButton();
            this.rbVerim = new System.Windows.Forms.RadioButton();
            this.grpTur = new System.Windows.Forms.GroupBox();
            this.btnSiparisCikis = new System.Windows.Forms.Button();
            this.lblYeniStokAdi = new System.Windows.Forms.Label();
            this.txtYeniStokAdi = new System.Windows.Forms.TextBox();
            this.txtOzelKod = new System.Windows.Forms.TextBox();
            this.lblOzelKod = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpTur.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCari
            // 
            this.lblCari.AutoSize = true;
            this.lblCari.Location = new System.Drawing.Point(333, 69);
            this.lblCari.Name = "lblCari";
            this.lblCari.Size = new System.Drawing.Size(66, 23);
            this.lblCari.TabIndex = 0;
            this.lblCari.Text = "Cari ID:";
            // 
            // lblStok
            // 
            this.lblStok.AutoSize = true;
            this.lblStok.Location = new System.Drawing.Point(333, 133);
            this.lblStok.Name = "lblStok";
            this.lblStok.Size = new System.Drawing.Size(70, 23);
            this.lblStok.TabIndex = 1;
            this.lblStok.Text = "Stok ID:";
            // 
            // lblBirim
            // 
            this.lblBirim.AutoSize = true;
            this.lblBirim.Location = new System.Drawing.Point(333, 294);
            this.lblBirim.Name = "lblBirim";
            this.lblBirim.Size = new System.Drawing.Size(53, 23);
            this.lblBirim.TabIndex = 2;
            this.lblBirim.Text = "Birim:";
            // 
            // lblMiktar
            // 
            this.lblMiktar.AutoSize = true;
            this.lblMiktar.Location = new System.Drawing.Point(333, 355);
            this.lblMiktar.Name = "lblMiktar";
            this.lblMiktar.Size = new System.Drawing.Size(65, 23);
            this.lblMiktar.TabIndex = 3;
            this.lblMiktar.Text = "Miktar:";
            // 
            // lblTur
            // 
            this.lblTur.AutoSize = true;
            this.lblTur.Location = new System.Drawing.Point(333, 430);
            this.lblTur.Name = "lblTur";
            this.lblTur.Size = new System.Drawing.Size(102, 23);
            this.lblTur.TabIndex = 4;
            this.lblTur.Text = "Sipariş Türü:";
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Location = new System.Drawing.Point(333, 495);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(108, 23);
            this.lblTarih.TabIndex = 5;
            this.lblTarih.Text = "Sipariş Tarihi:";
            // 
            // cmbCari
            // 
            this.cmbCari.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCari.FormattingEnabled = true;
            this.cmbCari.Location = new System.Drawing.Point(452, 69);
            this.cmbCari.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCari.Name = "cmbCari";
            this.cmbCari.Size = new System.Drawing.Size(200, 31);
            this.cmbCari.TabIndex = 0;
            // 
            // cmbStok
            // 
            this.cmbStok.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStok.FormattingEnabled = true;
            this.cmbStok.Location = new System.Drawing.Point(452, 133);
            this.cmbStok.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStok.Name = "cmbStok";
            this.cmbStok.Size = new System.Drawing.Size(200, 31);
            this.cmbStok.TabIndex = 1;
            // 
            // cmbBirim
            // 
            this.cmbBirim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBirim.FormattingEnabled = true;
            this.cmbBirim.Location = new System.Drawing.Point(452, 294);
            this.cmbBirim.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbBirim.Name = "cmbBirim";
            this.cmbBirim.Size = new System.Drawing.Size(200, 31);
            this.cmbBirim.TabIndex = 4;
            // 
            // dateSiparis
            // 
            this.dateSiparis.Location = new System.Drawing.Point(452, 495);
            this.dateSiparis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateSiparis.Name = "dateSiparis";
            this.dateSiparis.Size = new System.Drawing.Size(200, 30);
            this.dateSiparis.TabIndex = 8;
            // 
            // txtMiktar
            // 
            this.txtMiktar.Location = new System.Drawing.Point(452, 355);
            this.txtMiktar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMiktar.Name = "txtMiktar";
            this.txtMiktar.Size = new System.Drawing.Size(200, 30);
            this.txtMiktar.TabIndex = 5;
            this.txtMiktar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMiktar_KeyPress);
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.SteelBlue;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEkle.Location = new System.Drawing.Point(477, 540);
            this.btnEkle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(124, 43);
            this.btnEkle.TabIndex = 9;
            this.btnEkle.Text = "➕ Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // rbAlim
            // 
            this.rbAlim.AutoSize = true;
            this.rbAlim.Location = new System.Drawing.Point(12, 32);
            this.rbAlim.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbAlim.Name = "rbAlim";
            this.rbAlim.Size = new System.Drawing.Size(65, 27);
            this.rbAlim.TabIndex = 6;
            this.rbAlim.TabStop = true;
            this.rbAlim.Text = "Alım";
            this.rbAlim.UseVisualStyleBackColor = true;
            // 
            // rbVerim
            // 
            this.rbVerim.AutoSize = true;
            this.rbVerim.Location = new System.Drawing.Point(100, 32);
            this.rbVerim.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbVerim.Name = "rbVerim";
            this.rbVerim.Size = new System.Drawing.Size(75, 27);
            this.rbVerim.TabIndex = 7;
            this.rbVerim.TabStop = true;
            this.rbVerim.Text = "Verim";
            this.rbVerim.UseVisualStyleBackColor = true;
            // 
            // grpTur
            // 
            this.grpTur.Controls.Add(this.rbAlim);
            this.grpTur.Controls.Add(this.rbVerim);
            this.grpTur.Location = new System.Drawing.Point(444, 398);
            this.grpTur.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpTur.Name = "grpTur";
            this.grpTur.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpTur.Size = new System.Drawing.Size(200, 78);
            this.grpTur.TabIndex = 4;
            this.grpTur.TabStop = false;
            // 
            // btnSiparisCikis
            // 
            this.btnSiparisCikis.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSiparisCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiparisCikis.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSiparisCikis.Location = new System.Drawing.Point(881, 595);
            this.btnSiparisCikis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSiparisCikis.Name = "btnSiparisCikis";
            this.btnSiparisCikis.Size = new System.Drawing.Size(107, 39);
            this.btnSiparisCikis.TabIndex = 10;
            this.btnSiparisCikis.Text = "🔚 Çıkış";
            this.btnSiparisCikis.UseVisualStyleBackColor = false;
            this.btnSiparisCikis.Click += new System.EventHandler(this.btnSiparisCikis_Click);
            // 
            // lblYeniStokAdi
            // 
            this.lblYeniStokAdi.AutoSize = true;
            this.lblYeniStokAdi.Location = new System.Drawing.Point(333, 194);
            this.lblYeniStokAdi.Name = "lblYeniStokAdi";
            this.lblYeniStokAdi.Size = new System.Drawing.Size(114, 23);
            this.lblYeniStokAdi.TabIndex = 8;
            this.lblYeniStokAdi.Text = "Yeni Stok Adı:";
            this.lblYeniStokAdi.Visible = false;
            // 
            // txtYeniStokAdi
            // 
            this.txtYeniStokAdi.Location = new System.Drawing.Point(452, 194);
            this.txtYeniStokAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtYeniStokAdi.Name = "txtYeniStokAdi";
            this.txtYeniStokAdi.Size = new System.Drawing.Size(200, 30);
            this.txtYeniStokAdi.TabIndex = 2;
            this.txtYeniStokAdi.Visible = false;
            // 
            // txtOzelKod
            // 
            this.txtOzelKod.Location = new System.Drawing.Point(452, 248);
            this.txtOzelKod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOzelKod.Name = "txtOzelKod";
            this.txtOzelKod.Size = new System.Drawing.Size(200, 30);
            this.txtOzelKod.TabIndex = 3;
            this.txtOzelKod.Visible = false;
            // 
            // lblOzelKod
            // 
            this.lblOzelKod.AutoSize = true;
            this.lblOzelKod.Location = new System.Drawing.Point(333, 248);
            this.lblOzelKod.Name = "lblOzelKod";
            this.lblOzelKod.Size = new System.Drawing.Size(83, 23);
            this.lblOzelKod.TabIndex = 12;
            this.lblOzelKod.Text = "Özel Kod:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(334, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 38);
            this.label1.TabIndex = 13;
            this.label1.Text = "Yeni Sipariş Giriş Ekranı";
            // 
            // FrmSiparisEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1000, 647);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblOzelKod);
            this.Controls.Add(this.txtOzelKod);
            this.Controls.Add(this.txtYeniStokAdi);
            this.Controls.Add(this.lblYeniStokAdi);
            this.Controls.Add(this.btnSiparisCikis);
            this.Controls.Add(this.grpTur);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.txtMiktar);
            this.Controls.Add(this.dateSiparis);
            this.Controls.Add(this.cmbBirim);
            this.Controls.Add(this.cmbStok);
            this.Controls.Add(this.cmbCari);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.lblTur);
            this.Controls.Add(this.lblMiktar);
            this.Controls.Add(this.lblBirim);
            this.Controls.Add(this.lblStok);
            this.Controls.Add(this.lblCari);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSiparisEkle";
            this.Text = "Yeni Sipariş Oluştur";
            this.Load += new System.EventHandler(this.FrmSiparis_Load);
            this.grpTur.ResumeLayout(false);
            this.grpTur.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblCari;
        private Label lblStok;
        private Label lblBirim;
        private Label lblMiktar;
        private Label lblTur;
        private Label lblTarih;
        private ComboBox cmbCari;
        private ComboBox cmbStok;
        private ComboBox cmbBirim;
        private DateTimePicker dateSiparis;
        private TextBox txtMiktar;
        private Button btnEkle;
        private RadioButton rbAlim;
        private RadioButton rbVerim;
        private GroupBox grpTur;
        private Button btnSiparisCikis;
        private Label lblYeniStokAdi;
        private TextBox txtYeniStokAdi;
        private TextBox txtOzelKod;
        private Label lblOzelKod;
        private Label label1;
    }
}

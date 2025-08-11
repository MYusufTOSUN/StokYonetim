namespace Stok
{
    partial class FrmMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCikis = new System.Windows.Forms.Button();
            this.btnSiparisEkle = new System.Windows.Forms.Button();
            this.btnSiparisListele = new System.Windows.Forms.Button();
            this.btnSiparisSil = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHosgeldin = new System.Windows.Forms.Label();
            this.lblTarihSaat = new System.Windows.Forms.Label();
            this.timerSaat = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.ForeColor = System.Drawing.Color.White;
            this.btnCikis.Location = new System.Drawing.Point(780, 384);
            this.btnCikis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(107, 39);
            this.btnCikis.TabIndex = 4;
            this.btnCikis.Text = "🔚 Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // btnSiparisEkle
            // 
            this.btnSiparisEkle.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSiparisEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiparisEkle.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSiparisEkle.Location = new System.Drawing.Point(97, 231);
            this.btnSiparisEkle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSiparisEkle.Name = "btnSiparisEkle";
            this.btnSiparisEkle.Size = new System.Drawing.Size(208, 91);
            this.btnSiparisEkle.TabIndex = 1;
            this.btnSiparisEkle.Text = "➕ Sipariş Ekle";
            this.btnSiparisEkle.UseVisualStyleBackColor = false;
            this.btnSiparisEkle.Click += new System.EventHandler(this.btnSiparisEkle_Click);
            // 
            // btnSiparisListele
            // 
            this.btnSiparisListele.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSiparisListele.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiparisListele.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSiparisListele.Location = new System.Drawing.Point(333, 231);
            this.btnSiparisListele.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSiparisListele.Name = "btnSiparisListele";
            this.btnSiparisListele.Size = new System.Drawing.Size(208, 91);
            this.btnSiparisListele.TabIndex = 2;
            this.btnSiparisListele.Text = "📄 Sipariş Listele";
            this.btnSiparisListele.UseVisualStyleBackColor = false;
            this.btnSiparisListele.Click += new System.EventHandler(this.btnSiparisListele_Click);
            // 
            // btnSiparisSil
            // 
            this.btnSiparisSil.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSiparisSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiparisSil.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSiparisSil.Location = new System.Drawing.Point(572, 231);
            this.btnSiparisSil.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSiparisSil.Name = "btnSiparisSil";
            this.btnSiparisSil.Size = new System.Drawing.Size(208, 91);
            this.btnSiparisSil.TabIndex = 3;
            this.btnSiparisSil.Text = "❌ Sipariş Sil";
            this.btnSiparisSil.UseVisualStyleBackColor = false;
            this.btnSiparisSil.Click += new System.EventHandler(this.btnSiparisSil_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(291, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 38);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sipariş Yönetim Paneli";
            // 
            // lblHosgeldin
            // 
            this.lblHosgeldin.AutoSize = true;
            this.lblHosgeldin.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHosgeldin.ForeColor = System.Drawing.Color.Navy;
            this.lblHosgeldin.Location = new System.Drawing.Point(14, 13);
            this.lblHosgeldin.Name = "lblHosgeldin";
            this.lblHosgeldin.Size = new System.Drawing.Size(0, 23);
            this.lblHosgeldin.TabIndex = 6;
            // 
            // lblTarihSaat
            // 
            this.lblTarihSaat.AutoSize = true;
            this.lblTarihSaat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarihSaat.ForeColor = System.Drawing.Color.Navy;
            this.lblTarihSaat.Location = new System.Drawing.Point(722, 13);
            this.lblTarihSaat.Name = "lblTarihSaat";
            this.lblTarihSaat.Size = new System.Drawing.Size(0, 20);
            this.lblTarihSaat.TabIndex = 7;
            // 
            // timerSaat
            // 
            this.timerSaat.Enabled = true;
            this.timerSaat.Interval = 60000;
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(899, 436);
            this.Controls.Add(this.lblTarihSaat);
            this.Controls.Add(this.lblHosgeldin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSiparisSil);
            this.Controls.Add(this.btnSiparisListele);
            this.Controls.Add(this.btnSiparisEkle);
            this.Controls.Add(this.btnCikis);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMenu";
            this.Text = "Sipariş Yönetim Paneli";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Button btnSiparisEkle;
        private System.Windows.Forms.Button btnSiparisListele;
        private System.Windows.Forms.Button btnSiparisSil;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHosgeldin;
        private System.Windows.Forms.Label lblTarihSaat;
        private System.Windows.Forms.Timer timerSaat;
    }
}
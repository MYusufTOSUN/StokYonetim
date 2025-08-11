using System;

namespace Stok
{
    partial class FrmListe
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
            this.cmbTablolar.Enabled = false;
            this.cmbTablolar.Visible = false;
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
            this.dgvSonuc = new System.Windows.Forms.DataGridView();
            this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kopyalaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAra = new System.Windows.Forms.Button();
            this.lblSiparisId = new System.Windows.Forms.Label();
            this.txtSiparisId = new System.Windows.Forms.TextBox();
            this.btnAra2 = new System.Windows.Forms.Button();
            this.lblCariListele = new System.Windows.Forms.Label();
            this.txtCariListele = new System.Windows.Forms.TextBox();
            this.btnCikis = new System.Windows.Forms.Button();
            this.lblStokID = new System.Windows.Forms.Label();
            this.txtStokID = new System.Windows.Forms.TextBox();
            this.btnAra3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpExport = new System.Windows.Forms.GroupBox();
            this.btnScriptAl = new System.Windows.Forms.Button();
            this.cmbTablolar = new System.Windows.Forms.ComboBox();
            this.rdbVeritabani = new System.Windows.Forms.RadioButton();
            this.rdbTablo = new System.Windows.Forms.RadioButton();
            this.btnTxtAktar = new System.Windows.Forms.Button();
            this.lblSonuc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonuc)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.grpExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSonuc
            // 
            this.dgvSonuc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSonuc.ContextMenuStrip = this.contextMenuGrid;
            this.dgvSonuc.Location = new System.Drawing.Point(21, 215);
            this.dgvSonuc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvSonuc.Name = "dgvSonuc";
            this.dgvSonuc.RowHeadersWidth = 51;
            this.dgvSonuc.RowTemplate.Height = 24;
            this.dgvSonuc.Size = new System.Drawing.Size(959, 335);
            this.dgvSonuc.TabIndex = 0;
            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopyalaToolStripMenuItem});
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new System.Drawing.Size(133, 28);
            // 
            // kopyalaToolStripMenuItem
            // 
            this.kopyalaToolStripMenuItem.Name = "kopyalaToolStripMenuItem";
            this.kopyalaToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.kopyalaToolStripMenuItem.Text = "Kopyala";
            this.kopyalaToolStripMenuItem.Click += new System.EventHandler(this.kopyalaToolStripMenuItem_Click);
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAra.Location = new System.Drawing.Point(273, 72);
            this.btnAra.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(93, 33);
            this.btnAra.TabIndex = 1;
            this.btnAra.Text = "🔎 Ara";
            this.btnAra.UseVisualStyleBackColor = false;
            // 
            // lblSiparisId
            // 
            this.lblSiparisId.AutoSize = true;
            this.lblSiparisId.BackColor = System.Drawing.Color.Transparent;
            this.lblSiparisId.Location = new System.Drawing.Point(22, 77);
            this.lblSiparisId.Name = "lblSiparisId";
            this.lblSiparisId.Size = new System.Drawing.Size(85, 23);
            this.lblSiparisId.TabIndex = 2;
            this.lblSiparisId.Text = "Sipariş ID:";
            // 
            // txtSiparisId
            // 
            this.txtSiparisId.Location = new System.Drawing.Point(112, 72);
            this.txtSiparisId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSiparisId.Name = "txtSiparisId";
            this.txtSiparisId.Size = new System.Drawing.Size(124, 30);
            this.txtSiparisId.TabIndex = 1;
            // 
            // btnAra2
            // 
            this.btnAra2.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAra2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAra2.Location = new System.Drawing.Point(273, 117);
            this.btnAra2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAra2.Name = "btnAra2";
            this.btnAra2.Size = new System.Drawing.Size(93, 33);
            this.btnAra2.TabIndex = 4;
            this.btnAra2.Text = "🔎 Ara";
            this.btnAra2.UseVisualStyleBackColor = false;
            // 
            // lblCariListele
            // 
            this.lblCariListele.AutoSize = true;
            this.lblCariListele.BackColor = System.Drawing.Color.Transparent;
            this.lblCariListele.Location = new System.Drawing.Point(22, 127);
            this.lblCariListele.Name = "lblCariListele";
            this.lblCariListele.Size = new System.Drawing.Size(66, 23);
            this.lblCariListele.TabIndex = 5;
            this.lblCariListele.Text = "Cari ID:";
            // 
            // txtCariListele
            // 
            this.txtCariListele.Location = new System.Drawing.Point(112, 123);
            this.txtCariListele.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCariListele.Name = "txtCariListele";
            this.txtCariListele.Size = new System.Drawing.Size(124, 30);
            this.txtCariListele.TabIndex = 2;
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCikis.Location = new System.Drawing.Point(881, 595);
            this.btnCikis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(107, 39);
            this.btnCikis.TabIndex = 6;
            this.btnCikis.Text = "🔚 Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // lblStokID
            // 
            this.lblStokID.AutoSize = true;
            this.lblStokID.BackColor = System.Drawing.Color.Transparent;
            this.lblStokID.Location = new System.Drawing.Point(22, 173);
            this.lblStokID.Name = "lblStokID";
            this.lblStokID.Size = new System.Drawing.Size(70, 23);
            this.lblStokID.TabIndex = 7;
            this.lblStokID.Text = "Stok ID:";
            // 
            // txtStokID
            // 
            this.txtStokID.Location = new System.Drawing.Point(112, 169);
            this.txtStokID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStokID.Name = "txtStokID";
            this.txtStokID.Size = new System.Drawing.Size(124, 30);
            this.txtStokID.TabIndex = 8;
            // 
            // btnAra3
            // 
            this.btnAra3.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAra3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAra3.Location = new System.Drawing.Point(273, 167);
            this.btnAra3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAra3.Name = "btnAra3";
            this.btnAra3.Size = new System.Drawing.Size(93, 33);
            this.btnAra3.TabIndex = 9;
            this.btnAra3.Text = "🔎 Ara";
            this.btnAra3.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(364, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 38);
            this.label1.TabIndex = 10;
            this.label1.Text = "Listeleme Ekranı";
            // 
            // grpExport
            // 
            this.grpExport.Controls.Add(this.btnScriptAl);
            this.grpExport.Controls.Add(this.cmbTablolar);
            this.grpExport.Controls.Add(this.rdbVeritabani);
            this.grpExport.Controls.Add(this.rdbTablo);
            this.grpExport.Location = new System.Drawing.Point(583, 53);
            this.grpExport.Name = "grpExport";
            this.grpExport.Size = new System.Drawing.Size(397, 150);
            this.grpExport.TabIndex = 11;
            this.grpExport.TabStop = false;
            this.grpExport.Text = "Data Script Al";
            // 
            // btnScriptAl
            // 
            this.btnScriptAl.BackColor = System.Drawing.Color.SteelBlue;
            this.btnScriptAl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScriptAl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnScriptAl.Location = new System.Drawing.Point(70, 104);
            this.btnScriptAl.Name = "btnScriptAl";
            this.btnScriptAl.Size = new System.Drawing.Size(262, 39);
            this.btnScriptAl.TabIndex = 4;
            this.btnScriptAl.Text = "📁 Veri Scripti Al\t";
            this.btnScriptAl.UseVisualStyleBackColor = false;
            this.btnScriptAl.Click += new System.EventHandler(this.btnScriptAl_Click);
            // 
            // cmbTablolar
            // 
            this.cmbTablolar.Enabled = false;
            this.cmbTablolar.FormattingEnabled = true;
            this.cmbTablolar.Location = new System.Drawing.Point(198, 24);
            this.cmbTablolar.Name = "cmbTablolar";
            this.cmbTablolar.Size = new System.Drawing.Size(134, 31);
            this.cmbTablolar.TabIndex = 3;
            // 
            // rdbVeritabani
            // 
            this.rdbVeritabani.AutoSize = true;
            this.rdbVeritabani.Location = new System.Drawing.Point(7, 63);
            this.rdbVeritabani.Name = "rdbVeritabani";
            this.rdbVeritabani.Size = new System.Drawing.Size(152, 27);
            this.rdbVeritabani.TabIndex = 1;
            this.rdbVeritabani.TabStop = true;
            this.rdbVeritabani.Text = "Tüm Veri Tabanı";
            this.rdbVeritabani.UseVisualStyleBackColor = true;
            // 
            // rdbTablo
            // 
            this.rdbTablo.AutoSize = true;
            this.rdbTablo.Location = new System.Drawing.Point(7, 26);
            this.rdbTablo.Name = "rdbTablo";
            this.rdbTablo.Size = new System.Drawing.Size(102, 27);
            this.rdbTablo.TabIndex = 0;
            this.rdbTablo.TabStop = true;
            this.rdbTablo.Text = "Tablo Seç";
            this.rdbTablo.UseVisualStyleBackColor = true;
            // 
            // btnTxtAktar
            // 
            this.btnTxtAktar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnTxtAktar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTxtAktar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTxtAktar.Location = new System.Drawing.Point(872, 510);
            this.btnTxtAktar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTxtAktar.Name = "btnTxtAktar";
            this.btnTxtAktar.Size = new System.Drawing.Size(107, 39);
            this.btnTxtAktar.TabIndex = 12;
            this.btnTxtAktar.Text = "📄 Txt ";
            this.btnTxtAktar.UseVisualStyleBackColor = false;
            this.btnTxtAktar.Click += new System.EventHandler(this.btnTxtAktar_Click);
            // 
            // lblSonuc
            // 
            this.lblSonuc.AutoSize = true;
            this.lblSonuc.BackColor = System.Drawing.Color.Transparent;
            this.lblSonuc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSonuc.Location = new System.Drawing.Point(830, 554);
            this.lblSonuc.Name = "lblSonuc";
            this.lblSonuc.Size = new System.Drawing.Size(0, 23);
            this.lblSonuc.TabIndex = 13;
            // 
            // FrmListe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 647);
            this.Controls.Add(this.lblSonuc);
            this.Controls.Add(this.btnTxtAktar);
            this.Controls.Add(this.grpExport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAra3);
            this.Controls.Add(this.txtStokID);
            this.Controls.Add(this.lblStokID);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.txtCariListele);
            this.Controls.Add(this.lblCariListele);
            this.Controls.Add(this.btnAra2);
            this.Controls.Add(this.txtSiparisId);
            this.Controls.Add(this.lblSiparisId);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.dgvSonuc);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmListe";
            this.Text = "Listeleme";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonuc)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.grpExport.ResumeLayout(false);
            this.grpExport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSonuc;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Label lblSiparisId;
        private System.Windows.Forms.TextBox txtSiparisId;
        private System.Windows.Forms.Button btnAra2;
        private System.Windows.Forms.Label lblCariListele;
        private System.Windows.Forms.TextBox txtCariListele;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Label lblStokID;
        private System.Windows.Forms.TextBox txtStokID;
        private System.Windows.Forms.Button btnAra3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpExport;
        private System.Windows.Forms.RadioButton rdbVeritabani;
        private System.Windows.Forms.RadioButton rdbTablo;
        private System.Windows.Forms.ComboBox cmbTablolar;
        private System.Windows.Forms.Button btnScriptAl;
        private System.Windows.Forms.ContextMenuStrip contextMenuGrid;
        private System.Windows.Forms.ToolStripMenuItem kopyalaToolStripMenuItem;
        private System.Windows.Forms.Button btnTxtAktar;
        private System.Windows.Forms.Label lblSonuc;
    }
}
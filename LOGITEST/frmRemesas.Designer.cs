
namespace LOGITEST
{
    partial class frmRemesas
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkuuid = new System.Windows.Forms.CheckBox();
            this.btnbuscar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmborigen = new System.Windows.Forms.ComboBox();
            this.dtfinal = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtinicio = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.remesar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.serie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Folio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaFactura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UUIDFiscal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreRemitente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreDestinatario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdViaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkuuid);
            this.groupBox1.Controls.Add(this.btnbuscar);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmborigen);
            this.groupBox1.Controls.Add(this.dtfinal);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtinicio);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(893, 121);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtros de búsqueda:";
            // 
            // chkuuid
            // 
            this.chkuuid.AutoSize = true;
            this.chkuuid.Location = new System.Drawing.Point(20, 94);
            this.chkuuid.Name = "chkuuid";
            this.chkuuid.Size = new System.Drawing.Size(200, 17);
            this.chkuuid.TabIndex = 10;
            this.chkuuid.Text = "Incluir la descarga de los folios UUID";
            this.chkuuid.UseVisualStyleBackColor = true;
            // 
            // btnbuscar
            // 
            this.btnbuscar.Location = new System.Drawing.Point(734, 87);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(131, 28);
            this.btnbuscar.TabIndex = 9;
            this.btnbuscar.Text = "Generar búsqueda";
            this.btnbuscar.UseVisualStyleBackColor = true;
            this.btnbuscar.Click += new System.EventHandler(this.btnbuscar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bodega origen:";
            // 
            // cmborigen
            // 
            this.cmborigen.FormattingEnabled = true;
            this.cmborigen.Location = new System.Drawing.Point(96, 58);
            this.cmborigen.Name = "cmborigen";
            this.cmborigen.Size = new System.Drawing.Size(307, 21);
            this.cmborigen.TabIndex = 5;
            // 
            // dtfinal
            // 
            this.dtfinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtfinal.Location = new System.Drawing.Point(558, 25);
            this.dtfinal.Margin = new System.Windows.Forms.Padding(1);
            this.dtfinal.MaxDate = new System.DateTime(2030, 1, 5, 0, 0, 0, 0);
            this.dtfinal.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtfinal.Name = "dtfinal";
            this.dtfinal.Size = new System.Drawing.Size(307, 20);
            this.dtfinal.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(479, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fecha final:";
            // 
            // dtinicio
            // 
            this.dtinicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtinicio.Location = new System.Drawing.Point(96, 25);
            this.dtinicio.Margin = new System.Windows.Forms.Padding(1);
            this.dtinicio.MaxDate = new System.DateTime(2030, 1, 5, 0, 0, 0, 0);
            this.dtinicio.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtinicio.Name = "dtinicio";
            this.dtinicio.Size = new System.Drawing.Size(307, 20);
            this.dtinicio.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha inicial:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.remesar,
            this.serie,
            this.Folio,
            this.FechaFactura,
            this.subtotal,
            this.total,
            this.UUIDFiscal,
            this.NombreRemitente,
            this.NombreDestinatario,
            this.IdViaje});
            this.dataGridView1.Location = new System.Drawing.Point(12, 139);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(893, 328);
            this.dataGridView1.TabIndex = 1;
            // 
            // remesar
            // 
            this.remesar.HeaderText = ".";
            this.remesar.Name = "remesar";
            this.remesar.Width = 50;
            // 
            // serie
            // 
            this.serie.DataPropertyName = "Serie";
            this.serie.HeaderText = "SERIE";
            this.serie.Name = "serie";
            // 
            // Folio
            // 
            this.Folio.DataPropertyName = "Folio";
            this.Folio.HeaderText = "FOLIO";
            this.Folio.Name = "Folio";
            // 
            // FechaFactura
            // 
            this.FechaFactura.DataPropertyName = "FechaFactura";
            this.FechaFactura.HeaderText = "FECHA";
            this.FechaFactura.Name = "FechaFactura";
            // 
            // subtotal
            // 
            this.subtotal.DataPropertyName = "SubTotal";
            this.subtotal.HeaderText = "SUBTOTAL";
            this.subtotal.Name = "subtotal";
            // 
            // total
            // 
            this.total.DataPropertyName = "Total";
            this.total.HeaderText = "TOTAL";
            this.total.Name = "total";
            // 
            // UUIDFiscal
            // 
            this.UUIDFiscal.DataPropertyName = "UUIDFiscal";
            this.UUIDFiscal.HeaderText = "UUID";
            this.UUIDFiscal.Name = "UUIDFiscal";
            // 
            // NombreRemitente
            // 
            this.NombreRemitente.DataPropertyName = "NombreRemitente";
            this.NombreRemitente.HeaderText = "ORIGEN";
            this.NombreRemitente.Name = "NombreRemitente";
            this.NombreRemitente.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NombreRemitente.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NombreRemitente.Width = 250;
            // 
            // NombreDestinatario
            // 
            this.NombreDestinatario.DataPropertyName = "NombreDestinatario";
            this.NombreDestinatario.HeaderText = "DESTINO";
            this.NombreDestinatario.Name = "NombreDestinatario";
            this.NombreDestinatario.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NombreDestinatario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NombreDestinatario.Width = 250;
            // 
            // IdViaje
            // 
            this.IdViaje.DataPropertyName = "IdViaje";
            this.IdViaje.HeaderText = "IdViaje";
            this.IdViaje.Name = "IdViaje";
            this.IdViaje.ReadOnly = true;
            this.IdViaje.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(786, 473);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 33);
            this.button1.TabIndex = 2;
            this.button1.Text = "Imprimir remesa";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmRemesas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 512);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRemesas";
            this.Text = "Remesas Bepensa Bebidas - Logística del Mayab";
            this.Load += new System.EventHandler(this.frmRemesas_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtinicio;
        private System.Windows.Forms.DateTimePicker dtfinal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmborigen;
        private System.Windows.Forms.Button btnbuscar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkuuid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn remesar;
        private System.Windows.Forms.DataGridViewTextBoxColumn serie;
        private System.Windows.Forms.DataGridViewTextBoxColumn Folio;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaFactura;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn UUIDFiscal;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreRemitente;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreDestinatario;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdViaje;
    }
}

namespace LOGITEST
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rchConsole = new System.Windows.Forms.RichTextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkFile = new System.Windows.Forms.CheckBox();
            this.dtfechafin = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtfechainicio = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbOpciones = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(908, 640);
            this.panel2.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rchConsole);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(1, 95);
            this.panel4.Margin = new System.Windows.Forms.Padding(1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(907, 545);
            this.panel4.TabIndex = 5;
            // 
            // rchConsole
            // 
            this.rchConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rchConsole.Location = new System.Drawing.Point(0, 0);
            this.rchConsole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rchConsole.Name = "rchConsole";
            this.rchConsole.Size = new System.Drawing.Size(907, 545);
            this.rchConsole.TabIndex = 1;
            this.rchConsole.Text = "";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(1, 94);
            this.splitter2.Margin = new System.Windows.Forms.Padding(1);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(907, 1);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 94);
            this.splitter1.Margin = new System.Windows.Forms.Padding(1);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 546);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkFile);
            this.panel3.Controls.Add(this.dtfechafin);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.dtfechainicio);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(908, 94);
            this.panel3.TabIndex = 2;
            // 
            // chkFile
            // 
            this.chkFile.AutoSize = true;
            this.chkFile.Location = new System.Drawing.Point(21, 58);
            this.chkFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkFile.Name = "chkFile";
            this.chkFile.Size = new System.Drawing.Size(212, 24);
            this.chkFile.TabIndex = 4;
            this.chkFile.Text = "Leer archivo TXT periodos";
            this.chkFile.UseVisualStyleBackColor = true;
            // 
            // dtfechafin
            // 
            this.dtfechafin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtfechafin.Location = new System.Drawing.Point(608, 12);
            this.dtfechafin.Margin = new System.Windows.Forms.Padding(1);
            this.dtfechafin.MaxDate = new System.DateTime(2030, 1, 5, 0, 0, 0, 0);
            this.dtfechafin.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtfechafin.Name = "dtfechafin";
            this.dtfechafin.Size = new System.Drawing.Size(202, 26);
            this.dtfechafin.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(503, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha final:";
            // 
            // dtfechainicio
            // 
            this.dtfechainicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtfechainicio.Location = new System.Drawing.Point(112, 12);
            this.dtfechainicio.Margin = new System.Windows.Forms.Padding(1);
            this.dtfechainicio.MaxDate = new System.DateTime(2030, 1, 5, 0, 0, 0, 0);
            this.dtfechainicio.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtfechainicio.Name = "dtfechainicio";
            this.dtfechainicio.Size = new System.Drawing.Size(202, 26);
            this.dtfechainicio.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fecha inicio:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cmbOpciones);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 640);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(908, 55);
            this.panel1.TabIndex = 5;
            // 
            // cmbOpciones
            // 
            this.cmbOpciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbOpciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbOpciones.FormattingEnabled = true;
            this.cmbOpciones.Location = new System.Drawing.Point(0, 0);
            this.cmbOpciones.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbOpciones.Name = "cmbOpciones";
            this.cmbOpciones.Size = new System.Drawing.Size(674, 28);
            this.cmbOpciones.TabIndex = 4;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(674, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 55);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Limpiar";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStart.Location = new System.Drawing.Point(791, 0);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 55);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Procesar";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 695);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Interfaz de pruebas Logística del Mayab";
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rchConsole;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbOpciones;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtfechainicio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtfechafin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.CheckBox chkFile;
    }
}


namespace Continental.v2.MessageBoxForms
{
    partial class FormMessageBoxError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessageBoxError));
            this.lblOops = new System.Windows.Forms.Label();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.panelDerecho = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPrincipal.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOops
            // 
            this.lblOops.AutoSize = true;
            this.lblOops.Font = new System.Drawing.Font("Century Gothic", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOops.Location = new System.Drawing.Point(235, 237);
            this.lblOops.Name = "lblOops";
            this.lblOops.Size = new System.Drawing.Size(114, 36);
            this.lblOops.TabIndex = 1;
            this.lblOops.Text = "Oops...";
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblMensaje.Location = new System.Drawing.Point(60, 25);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(492, 33);
            this.lblMensaje.TabIndex = 2;
            this.lblMensaje.Text = "abcdefghijklmnñopqrs";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(225, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnContinuar
            // 
            this.btnContinuar.BackColor = System.Drawing.Color.Silver;
            this.btnContinuar.FlatAppearance.BorderSize = 0;
            this.btnContinuar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.btnContinuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinuar.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinuar.ForeColor = System.Drawing.Color.White;
            this.btnContinuar.Location = new System.Drawing.Point(-101, 404);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(200, 50);
            this.btnContinuar.TabIndex = 10;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = false;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPrincipal);
            this.panel1.Controls.Add(this.lblOops);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 500);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.lblMensaje);
            this.panel2.Location = new System.Drawing.Point(12, 276);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(576, 77);
            this.panel2.TabIndex = 11;
            // 
            // tableLayoutPrincipal
            // 
            this.tableLayoutPrincipal.ColumnCount = 2;
            this.tableLayoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPrincipal.Controls.Add(this.panelIzquierdo, 0, 0);
            this.tableLayoutPrincipal.Controls.Add(this.panelDerecho, 1, 0);
            this.tableLayoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPrincipal.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPrincipal.Name = "tableLayoutPrincipal";
            this.tableLayoutPrincipal.RowCount = 1;
            this.tableLayoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPrincipal.Size = new System.Drawing.Size(600, 500);
            this.tableLayoutPrincipal.TabIndex = 12;
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.Controls.Add(this.panel2);
            this.panelIzquierdo.Controls.Add(this.pictureBox1);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 0);
            this.panelIzquierdo.Margin = new System.Windows.Forms.Padding(0);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(300, 500);
            this.panelIzquierdo.TabIndex = 0;
            // 
            // panelDerecho
            // 
            this.panelDerecho.Controls.Add(this.btnContinuar);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(300, 0);
            this.panelDerecho.Margin = new System.Windows.Forms.Padding(0);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Size = new System.Drawing.Size(300, 500);
            this.panelDerecho.TabIndex = 1;
            // 
            // FormMessageBoxError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMessageBoxError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessageBoxError";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPrincipal.ResumeLayout(false);
            this.panelIzquierdo.ResumeLayout(false);
            this.panelDerecho.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblOops;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPrincipal;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Panel panelDerecho;
    }
}
namespace WS980
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.tBOut = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClearHistory = new System.Windows.Forms.Button();
            this.btnClearMaxMinDay = new System.Windows.Forms.Button();
            this.btnGetHistory = new System.Windows.Forms.Button();
            this.btnGetPara = new System.Windows.Forms.Button();
            this.btnCompareEprom = new System.Windows.Forms.Button();
            this.btnClr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tBOut
            // 
            this.tBOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBOut.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBOut.Location = new System.Drawing.Point(12, 42);
            this.tBOut.Multiline = true;
            this.tBOut.Name = "tBOut";
            this.tBOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tBOut.Size = new System.Drawing.Size(774, 423);
            this.tBOut.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(686, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "23";
            // 
            // btnClearHistory
            // 
            this.btnClearHistory.Location = new System.Drawing.Point(577, 13);
            this.btnClearHistory.Name = "btnClearHistory";
            this.btnClearHistory.Size = new System.Drawing.Size(75, 23);
            this.btnClearHistory.TabIndex = 3;
            this.btnClearHistory.Text = "ClearHistory";
            this.btnClearHistory.UseVisualStyleBackColor = true;
            this.btnClearHistory.Click += new System.EventHandler(this.btnClearHistory_Click);
            // 
            // btnClearMaxMinDay
            // 
            this.btnClearMaxMinDay.Location = new System.Drawing.Point(454, 13);
            this.btnClearMaxMinDay.Name = "btnClearMaxMinDay";
            this.btnClearMaxMinDay.Size = new System.Drawing.Size(105, 23);
            this.btnClearMaxMinDay.TabIndex = 4;
            this.btnClearMaxMinDay.Text = "ClearMaxMinDay";
            this.btnClearMaxMinDay.UseVisualStyleBackColor = true;
            this.btnClearMaxMinDay.Click += new System.EventHandler(this.btnClearMaxMinDay_Click);
            // 
            // btnGetHistory
            // 
            this.btnGetHistory.Location = new System.Drawing.Point(373, 13);
            this.btnGetHistory.Name = "btnGetHistory";
            this.btnGetHistory.Size = new System.Drawing.Size(75, 23);
            this.btnGetHistory.TabIndex = 5;
            this.btnGetHistory.Text = "Get History";
            this.btnGetHistory.UseVisualStyleBackColor = true;
            this.btnGetHistory.Click += new System.EventHandler(this.btnGetHistory_Click);
            // 
            // btnGetPara
            // 
            this.btnGetPara.Location = new System.Drawing.Point(273, 13);
            this.btnGetPara.Name = "btnGetPara";
            this.btnGetPara.Size = new System.Drawing.Size(94, 23);
            this.btnGetPara.TabIndex = 6;
            this.btnGetPara.Text = "Get Parameter";
            this.btnGetPara.UseVisualStyleBackColor = true;
            this.btnGetPara.Click += new System.EventHandler(this.btnGetPara_Click);
            // 
            // btnCompareEprom
            // 
            this.btnCompareEprom.Location = new System.Drawing.Point(173, 13);
            this.btnCompareEprom.Name = "btnCompareEprom";
            this.btnCompareEprom.Size = new System.Drawing.Size(94, 23);
            this.btnCompareEprom.TabIndex = 7;
            this.btnCompareEprom.Text = "Verg. EPROM";
            this.btnCompareEprom.UseVisualStyleBackColor = true;
            this.btnCompareEprom.Click += new System.EventHandler(this.btnCompareEprom_Click);
            // 
            // btnClr
            // 
            this.btnClr.Location = new System.Drawing.Point(94, 13);
            this.btnClr.Name = "btnClr";
            this.btnClr.Size = new System.Drawing.Size(73, 23);
            this.btnClr.TabIndex = 8;
            this.btnClr.Text = "CLR";
            this.btnClr.UseVisualStyleBackColor = true;
            this.btnClr.Click += new System.EventHandler(this.btnClr_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 477);
            this.Controls.Add(this.btnClr);
            this.Controls.Add(this.btnCompareEprom);
            this.Controls.Add(this.btnGetPara);
            this.Controls.Add(this.btnGetHistory);
            this.Controls.Add(this.btnClearMaxMinDay);
            this.Controls.Add(this.btnClearHistory);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tBOut);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "WS980 Test App";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tBOut;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnClearHistory;
        private System.Windows.Forms.Button btnClearMaxMinDay;
        private System.Windows.Forms.Button btnGetHistory;
        private System.Windows.Forms.Button btnGetPara;
        private System.Windows.Forms.Button btnCompareEprom;
        private System.Windows.Forms.Button btnClr;
    }
}


namespace WS980Test
{
    partial class MainForm
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
            this.btnClearHistory = new System.Windows.Forms.Button();
            this.btnClearMaxMinDay = new System.Windows.Forms.Button();
            this.btnGetHistory = new System.Windows.Forms.Button();
            this.btnReadPara = new System.Windows.Forms.Button();
            this.btnCompareEprom = new System.Windows.Forms.Button();
            this.btnClr = new System.Windows.Forms.Button();
            this.btnWritePara = new System.Windows.Forms.Button();
            this.cBKeyBeep = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tBWert = new System.Windows.Forms.TextBox();
            this.tBAdr = new System.Windows.Forms.TextBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnChangeinfo = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSetParameter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
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
            this.tBOut.Location = new System.Drawing.Point(12, 81);
            this.tBOut.Multiline = true;
            this.tBOut.Name = "tBOut";
            this.tBOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tBOut.Size = new System.Drawing.Size(774, 384);
            this.tBOut.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnClearHistory
            // 
            this.btnClearHistory.Location = new System.Drawing.Point(390, 39);
            this.btnClearHistory.Name = "btnClearHistory";
            this.btnClearHistory.Size = new System.Drawing.Size(105, 23);
            this.btnClearHistory.TabIndex = 3;
            this.btnClearHistory.Text = "ClearHistory";
            this.btnClearHistory.UseVisualStyleBackColor = true;
            this.btnClearHistory.Click += new System.EventHandler(this.btnClearHistory_Click);
            // 
            // btnClearMaxMinDay
            // 
            this.btnClearMaxMinDay.Location = new System.Drawing.Point(390, 13);
            this.btnClearMaxMinDay.Name = "btnClearMaxMinDay";
            this.btnClearMaxMinDay.Size = new System.Drawing.Size(105, 23);
            this.btnClearMaxMinDay.TabIndex = 4;
            this.btnClearMaxMinDay.Text = "ClearMaxMinDay";
            this.btnClearMaxMinDay.UseVisualStyleBackColor = true;
            this.btnClearMaxMinDay.Click += new System.EventHandler(this.btnClearMaxMinDay_Click);
            // 
            // btnGetHistory
            // 
            this.btnGetHistory.Location = new System.Drawing.Point(309, 13);
            this.btnGetHistory.Name = "btnGetHistory";
            this.btnGetHistory.Size = new System.Drawing.Size(75, 23);
            this.btnGetHistory.TabIndex = 5;
            this.btnGetHistory.Text = "Get History";
            this.btnGetHistory.UseVisualStyleBackColor = true;
            this.btnGetHistory.Click += new System.EventHandler(this.btnGetHistory_Click);
            // 
            // btnReadPara
            // 
            this.btnReadPara.Location = new System.Drawing.Point(209, 13);
            this.btnReadPara.Name = "btnReadPara";
            this.btnReadPara.Size = new System.Drawing.Size(94, 23);
            this.btnReadPara.TabIndex = 6;
            this.btnReadPara.Text = "Read Parameter";
            this.btnReadPara.UseVisualStyleBackColor = true;
            this.btnReadPara.Click += new System.EventHandler(this.btnReadPara_Click);
            // 
            // btnCompareEprom
            // 
            this.btnCompareEprom.Location = new System.Drawing.Point(94, 13);
            this.btnCompareEprom.Name = "btnCompareEprom";
            this.btnCompareEprom.Size = new System.Drawing.Size(94, 23);
            this.btnCompareEprom.TabIndex = 7;
            this.btnCompareEprom.Text = "Vergl. EPROM";
            this.btnCompareEprom.UseVisualStyleBackColor = true;
            this.btnCompareEprom.Click += new System.EventHandler(this.btnCompareEprom_Click);
            // 
            // btnClr
            // 
            this.btnClr.Location = new System.Drawing.Point(12, 43);
            this.btnClr.Name = "btnClr";
            this.btnClr.Size = new System.Drawing.Size(76, 23);
            this.btnClr.TabIndex = 8;
            this.btnClr.Text = "CLR";
            this.btnClr.UseVisualStyleBackColor = true;
            this.btnClr.Click += new System.EventHandler(this.btnClr_Click);
            // 
            // btnWritePara
            // 
            this.btnWritePara.Location = new System.Drawing.Point(209, 42);
            this.btnWritePara.Name = "btnWritePara";
            this.btnWritePara.Size = new System.Drawing.Size(94, 23);
            this.btnWritePara.TabIndex = 9;
            this.btnWritePara.Text = "Write Parameter";
            this.btnWritePara.UseVisualStyleBackColor = true;
            this.btnWritePara.Click += new System.EventHandler(this.btnWritePara_Click);
            // 
            // cBKeyBeep
            // 
            this.cBKeyBeep.AutoSize = true;
            this.cBKeyBeep.Location = new System.Drawing.Point(314, 45);
            this.cBKeyBeep.Name = "cBKeyBeep";
            this.cBKeyBeep.Size = new System.Drawing.Size(40, 17);
            this.cBKeyBeep.TabIndex = 10;
            this.cBKeyBeep.Text = " °C";
            this.cBKeyBeep.UseVisualStyleBackColor = true;
            this.cBKeyBeep.CheckedChanged += new System.EventHandler(this.cBKeyBeep_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tBWert);
            this.groupBox1.Controls.Add(this.tBAdr);
            this.groupBox1.Controls.Add(this.btnWrite);
            this.groupBox1.Controls.Add(this.btnRead);
            this.groupBox1.Location = new System.Drawing.Point(592, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 63);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EPROM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Wert";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Adr";
            // 
            // tBWert
            // 
            this.tBWert.Location = new System.Drawing.Point(76, 32);
            this.tBWert.Name = "tBWert";
            this.tBWert.Size = new System.Drawing.Size(54, 20);
            this.tBWert.TabIndex = 3;
            this.tBWert.Text = "0";
            // 
            // tBAdr
            // 
            this.tBAdr.Location = new System.Drawing.Point(16, 32);
            this.tBAdr.Name = "tBAdr";
            this.tBAdr.Size = new System.Drawing.Size(54, 20);
            this.tBAdr.TabIndex = 2;
            this.tBAdr.Text = "0x10";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(142, 35);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(41, 23);
            this.btnWrite.TabIndex = 1;
            this.btnWrite.Text = "write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(142, 6);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(41, 23);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnChangeinfo
            // 
            this.btnChangeinfo.Location = new System.Drawing.Point(511, 13);
            this.btnChangeinfo.Name = "btnChangeinfo";
            this.btnChangeinfo.Size = new System.Drawing.Size(75, 23);
            this.btnChangeinfo.TabIndex = 12;
            this.btnChangeinfo.Text = "Changeinfo";
            this.btnChangeinfo.UseVisualStyleBackColor = true;
            this.btnChangeinfo.Click += new System.EventHandler(this.btnChangeinfo_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(511, 39);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSetParameter
            // 
            this.btnSetParameter.Location = new System.Drawing.Point(94, 43);
            this.btnSetParameter.Name = "btnSetParameter";
            this.btnSetParameter.Size = new System.Drawing.Size(94, 23);
            this.btnSetParameter.TabIndex = 14;
            this.btnSetParameter.Text = "Set Parameter";
            this.btnSetParameter.UseVisualStyleBackColor = true;
            this.btnSetParameter.Click += new System.EventHandler(this.btnSetParameter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 477);
            this.Controls.Add(this.btnSetParameter);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnChangeinfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cBKeyBeep);
            this.Controls.Add(this.btnWritePara);
            this.Controls.Add(this.btnClr);
            this.Controls.Add(this.btnCompareEprom);
            this.Controls.Add(this.btnReadPara);
            this.Controls.Add(this.btnGetHistory);
            this.Controls.Add(this.btnClearMaxMinDay);
            this.Controls.Add(this.btnClearHistory);
            this.Controls.Add(this.tBOut);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "WS980 Test App";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tBOut;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnClearHistory;
        private System.Windows.Forms.Button btnClearMaxMinDay;
        private System.Windows.Forms.Button btnGetHistory;
        private System.Windows.Forms.Button btnReadPara;
        private System.Windows.Forms.Button btnCompareEprom;
        private System.Windows.Forms.Button btnClr;
        private System.Windows.Forms.Button btnWritePara;
        private System.Windows.Forms.CheckBox cBKeyBeep;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tBWert;
        private System.Windows.Forms.TextBox tBAdr;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnChangeinfo;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSetParameter;
    }
}


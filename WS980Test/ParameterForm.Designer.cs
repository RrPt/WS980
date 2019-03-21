namespace WS980Test
{
    partial class ParameterForm
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
            this.comboBoxRainUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxWindUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxLightUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPressureUnit = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTempUnit = new System.Windows.Forms.ComboBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxRainSeason = new System.Windows.Forms.ComboBox();
            this.checkBoxBeep = new System.Windows.Forms.CheckBox();
            this.checkBoxDST = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDownSekunden = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMinuten = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSekunden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinuten)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxRainUnit);
            this.groupBox1.Controls.Add(this.comboBoxWindUnit);
            this.groupBox1.Controls.Add(this.comboBoxLightUnit);
            this.groupBox1.Controls.Add(this.comboBoxPressureUnit);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxTempUnit);
            this.groupBox1.Location = new System.Drawing.Point(23, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Einheiten";
            // 
            // comboBoxRainUnit
            // 
            this.comboBoxRainUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRainUnit.FormattingEnabled = true;
            this.comboBoxRainUnit.Location = new System.Drawing.Point(348, 45);
            this.comboBoxRainUnit.Name = "comboBoxRainUnit";
            this.comboBoxRainUnit.Size = new System.Drawing.Size(58, 21);
            this.comboBoxRainUnit.TabIndex = 14;
            this.comboBoxRainUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxRainUnit_SelectedIndexChanged);
            // 
            // comboBoxWindUnit
            // 
            this.comboBoxWindUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWindUnit.FormattingEnabled = true;
            this.comboBoxWindUnit.Location = new System.Drawing.Point(267, 45);
            this.comboBoxWindUnit.Name = "comboBoxWindUnit";
            this.comboBoxWindUnit.Size = new System.Drawing.Size(58, 21);
            this.comboBoxWindUnit.TabIndex = 13;
            this.comboBoxWindUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxWindUnit_SelectedIndexChanged);
            // 
            // comboBoxLightUnit
            // 
            this.comboBoxLightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLightUnit.FormattingEnabled = true;
            this.comboBoxLightUnit.Location = new System.Drawing.Point(177, 45);
            this.comboBoxLightUnit.Name = "comboBoxLightUnit";
            this.comboBoxLightUnit.Size = new System.Drawing.Size(58, 21);
            this.comboBoxLightUnit.TabIndex = 12;
            this.comboBoxLightUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxLightUnit_SelectedIndexChanged);
            // 
            // comboBoxPressureUnit
            // 
            this.comboBoxPressureUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPressureUnit.FormattingEnabled = true;
            this.comboBoxPressureUnit.Location = new System.Drawing.Point(99, 45);
            this.comboBoxPressureUnit.Name = "comboBoxPressureUnit";
            this.comboBoxPressureUnit.Size = new System.Drawing.Size(58, 21);
            this.comboBoxPressureUnit.TabIndex = 11;
            this.comboBoxPressureUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxPressureUnit_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Niederschlag";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(268, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Wind";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Beleuchtung";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Luftdruck";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Temperatur";
            // 
            // comboBoxTempUnit
            // 
            this.comboBoxTempUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTempUnit.FormattingEnabled = true;
            this.comboBoxTempUnit.Location = new System.Drawing.Point(18, 45);
            this.comboBoxTempUnit.Name = "comboBoxTempUnit";
            this.comboBoxTempUnit.Size = new System.Drawing.Size(58, 21);
            this.comboBoxTempUnit.TabIndex = 0;
            this.comboBoxTempUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxTempUnit_SelectedIndexChanged);
            this.comboBoxTempUnit.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(290, 308);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(156, 57);
            this.btnWrite.TabIndex = 4;
            this.btnWrite.Text = "Schreiben";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(23, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 100);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Anzeige";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Regenzeitbegin";
            // 
            // comboBoxRainSeason
            // 
            this.comboBoxRainSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRainSeason.FormattingEnabled = true;
            this.comboBoxRainSeason.Items.AddRange(new object[] {
            "Januar",
            "Februar",
            "März",
            "April",
            "Mai",
            "Juni",
            "Juli",
            "August",
            "Spetemper",
            "Oktober",
            "November",
            "Dezember"});
            this.comboBoxRainSeason.Location = new System.Drawing.Point(41, 255);
            this.comboBoxRainSeason.Name = "comboBoxRainSeason";
            this.comboBoxRainSeason.Size = new System.Drawing.Size(94, 21);
            this.comboBoxRainSeason.TabIndex = 6;
            // 
            // checkBoxBeep
            // 
            this.checkBoxBeep.AutoSize = true;
            this.checkBoxBeep.Location = new System.Drawing.Point(391, 257);
            this.checkBoxBeep.Name = "checkBoxBeep";
            this.checkBoxBeep.Size = new System.Drawing.Size(79, 17);
            this.checkBoxBeep.TabIndex = 8;
            this.checkBoxBeep.Text = "Tastenpiep";
            this.checkBoxBeep.UseVisualStyleBackColor = true;
            // 
            // checkBoxDST
            // 
            this.checkBoxDST.AutoSize = true;
            this.checkBoxDST.Location = new System.Drawing.Point(294, 259);
            this.checkBoxDST.Name = "checkBoxDST";
            this.checkBoxDST.Size = new System.Drawing.Size(80, 17);
            this.checkBoxDST.TabIndex = 9;
            this.checkBoxDST.Text = "Sommerzeit";
            this.checkBoxDST.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(213, 256);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            12,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown1.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 258);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Zeitzone";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.numericUpDownMinuten);
            this.groupBox3.Controls.Add(this.numericUpDownSekunden);
            this.groupBox3.Location = new System.Drawing.Point(34, 299);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(237, 66);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Intervall";
            // 
            // numericUpDownSekunden
            // 
            this.numericUpDownSekunden.Enabled = false;
            this.numericUpDownSekunden.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDownSekunden.Location = new System.Drawing.Point(128, 19);
            this.numericUpDownSekunden.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.numericUpDownSekunden.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDownSekunden.Name = "numericUpDownSekunden";
            this.numericUpDownSekunden.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownSekunden.TabIndex = 13;
            this.numericUpDownSekunden.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDownSekunden.ValueChanged += new System.EventHandler(this.numericUpDownSekunden_ValueChanged);
            // 
            // numericUpDownMinuten
            // 
            this.numericUpDownMinuten.Location = new System.Drawing.Point(17, 19);
            this.numericUpDownMinuten.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numericUpDownMinuten.Name = "numericUpDownMinuten";
            this.numericUpDownMinuten.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownMinuten.TabIndex = 14;
            this.numericUpDownMinuten.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownMinuten.ValueChanged += new System.EventHandler(this.numericUpDownMinuten_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(193, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "s";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Min";
            // 
            // ParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 377);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBoxDST);
            this.Controls.Add(this.checkBoxBeep);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxRainSeason);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.groupBox1);
            this.Name = "ParameterForm";
            this.Text = "ParameterForm";
            this.Load += new System.EventHandler(this.ParameterForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSekunden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinuten)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox comboBoxTempUnit;
        internal System.Windows.Forms.ComboBox comboBoxPressureUnit;
        internal System.Windows.Forms.ComboBox comboBoxRainUnit;
        internal System.Windows.Forms.ComboBox comboBoxWindUnit;
        internal System.Windows.Forms.ComboBox comboBoxLightUnit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.ComboBox comboBoxRainSeason;
        private System.Windows.Forms.CheckBox checkBoxBeep;
        private System.Windows.Forms.CheckBox checkBoxDST;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownMinuten;
        private System.Windows.Forms.NumericUpDown numericUpDownSekunden;
    }
}
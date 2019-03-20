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
            this.groupBox1.SuspendLayout();
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
            this.btnWrite.Location = new System.Drawing.Point(490, 195);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 4;
            this.btnWrite.Text = "Schreiben";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // ParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 230);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.groupBox1);
            this.Name = "ParameterForm";
            this.Text = "ParameterForm";
            this.Load += new System.EventHandler(this.ParameterForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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
    }
}
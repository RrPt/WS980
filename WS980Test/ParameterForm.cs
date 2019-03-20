using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WS980_NS;

// todo  werte speichern und übertragen

namespace WS980Test
{
    public partial class ParameterForm : Form
    {
        WS980 ws980;

        public ParameterForm(WS980 ws980)
        {
            InitializeComponent();
            this.ws980 = ws980;
            comboBoxTempUnit.DataSource = Enum.GetValues(typeof(TemperatureUnitE));
            comboBoxPressureUnit.DataSource = Enum.GetValues(typeof(PressureUnitE));
            comboBoxLightUnit.DataSource = Enum.GetValues(typeof(LightUnitE));
            comboBoxWindUnit.DataSource = Enum.GetValues(typeof(WindUnitE));
            comboBoxRainUnit.DataSource = Enum.GetValues(typeof(RainUnitE));
        }



        private void ParameterForm_Load(object sender, EventArgs e)
        {
            // todo werte laden
            comboBoxTempUnit.SelectedItem = ws980.Para.TemperatureUnit;
            comboBoxPressureUnit.SelectedItem = ws980.Para.PressureUnit;
            comboBoxLightUnit.SelectedItem = ws980.Para.LightUnit;
            comboBoxWindUnit.SelectedItem = ws980.Para.WindUnit;
            comboBoxRainUnit.SelectedItem = ws980.Para.RainUnit;

        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            ws980.Para.WriteParameter();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTempUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // events erst nach der Initialisierung auswerten
            if (comboBoxTempUnit.IsHandleCreated) ws980.Para.TemperatureUnit =(TemperatureUnitE) comboBoxTempUnit.SelectedItem;
        }

        private void comboBoxPressureUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // events erst nach der Initialisierung auswerten
            if (comboBoxPressureUnit.IsHandleCreated) ws980.Para.PressureUnit = (PressureUnitE)comboBoxPressureUnit.SelectedItem;
        }

        private void comboBoxLightUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // events erst nach der Initialisierung auswerten
            if (comboBoxLightUnit.IsHandleCreated) ws980.Para.LightUnit = (LightUnitE)comboBoxLightUnit.SelectedItem;
        }

        private void comboBoxWindUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // events erst nach der Initialisierung auswerten
            if (comboBoxWindUnit.IsHandleCreated) ws980.Para.WindUnit = (WindUnitE)comboBoxWindUnit.SelectedItem;
        }

        private void comboBoxRainUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // events erst nach der Initialisierung auswerten
            if (comboBoxRainUnit.IsHandleCreated) ws980.Para.RainUnit = (RainUnitE)comboBoxRainUnit.SelectedItem;
        }
    }
}

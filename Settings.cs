using System;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;
using System.Text.RegularExpressions;

namespace MakoprBrowser
{
    public partial class Settings : Form
    {
        public class SettingPar
        {
            public string searchSys { get; set; }
            public string startPage { get; set; }
            public bool saveHist { get; set; }
            public string saveType { get; set; }
            public bool saveDate { get; set; }
        }
        public Settings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SettingPar setp = new SettingPar
            {
                searchSys = comboBox1.Text,
                startPage = comboBox2.Text,
                saveHist = checkBox1.Checked,
                saveType = comboBox3.Text,
                saveDate = checkBox2.Checked
            };
            string json = JsonSerializer.Serialize(setp);
            File.WriteAllText("browser/settings.json", json);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            string[] hist = File.ReadAllLines("browser/history.txt");
            for (int i = hist.Length-1; i > 0; i--)
                listBox1.Items.Add(hist[i]);
            try
            {
                SettingPar setp = JsonSerializer.Deserialize<SettingPar>(File.ReadAllText("browser/settings.json"));
                comboBox1.Text = setp.searchSys;
                comboBox2.Text = setp.startPage;
                checkBox1.Checked = setp.saveHist;
                comboBox3.Text = setp.saveType;
                checkBox2.Checked = setp.saveDate;
            }
            catch (Exception ex) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText("browser/history.txt", "");
            listBox1.Items.Clear();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            { 
                return;
            }
            string s = listBox1.SelectedItem.ToString();
            string change = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\t')
                    break;
                change += s[i];
            }
            if (s != "")
                Clipboard.SetData(DataFormats.StringFormat, change);
        }

        private void listBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            string s = listBox1.SelectedItem.ToString();
            string change = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\t')
                    break;
                change += s[i];
            }
            if (e.Control == true && e.KeyCode == Keys.C && s != "")
                Clipboard.SetData(DataFormats.StringFormat, change);
        }
    }
}
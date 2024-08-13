using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static MakoprBrowser.Settings;
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    public partial class Bookmarks : Form
    {
        private List<string> bookmarks = new List<string>();
        private string stylePath = "browser/bookmarks.txt";
        string[] book;
        public Bookmarks()
        {
            InitializeComponent();
            book = File.ReadAllLines(stylePath);
            listBox1.Items.AddRange(book);
        }

        private void Bookmarks_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text.ToString() + '\n');
            File.AppendAllText(stylePath, textBox1.Text.ToString() + '\n');
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem.ToString());
                File.WriteAllText(stylePath, "");
                for (int i = 0; i < listBox1.Items.Count; i++)
                    File.AppendAllText(stylePath, listBox1.Items[i].ToString() + '\n');
            }
            else
            {
                listBox1.Items.Remove(textBox2.Text.ToString());
                File.WriteAllText(stylePath, "");
                for (int i = 0; i < listBox1.Items.Count; i++)
                    File.AppendAllText(stylePath, listBox1.Items[i].ToString() + '\n');
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText("browser/bookmarks.txt", "");
            listBox1.Items.Clear();
        }


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string s = listBox1.SelectedItem.ToString();
            if (s != "")
                Clipboard.SetData(DataFormats.StringFormat, s);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string s = listBox1.SelectedItem.ToString();
            if (e.Control == true && e.KeyCode == Keys.C && s != "")
                Clipboard.SetData(DataFormats.StringFormat, s);
        }
    }
}

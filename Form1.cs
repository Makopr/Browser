using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using static MakoprBrowser.Settings;
using System.Text.RegularExpressions;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.ConstrainedExecution;
using WindowsFormsApp2.Properties;
using WindowsFormsApp2;
using CefSharp.DevTools.Debugger;
using System.Diagnostics;

namespace MakoprBrowser
{
    public partial class MakoprBrowser : Form
    {
        int i = 0;
        int SelectTab = 0;
        ChromiumWebBrowser chrome;
        Settings.SettingPar setp;
        string adress;
        public MakoprBrowser()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        public void AddHistory(string site)
        {
            if (setp.saveHist)
            {
                if (setp.saveDate)
                {
                    DateTime dt = DateTime.UtcNow;
                    File.AppendAllText("browser/history.txt", "\n" + site + "\t" + dt.ToString("HH:mm dd.MM.yy"));
                }
                else
                    File.AppendAllText("browser/history.txt", "\n" + site);
            }
        }
        private void MakoprBrowser_Load(object sender, EventArgs e)
        {
            try
            {
                setp = JsonSerializer.Deserialize<SettingPar>(File.ReadAllText("browser/settings.json"));

            }
            catch (Exception ex)
            {
                setp = new SettingPar
                {
                    searchSys = "Яндекс",
                    startPage = "ya.ru",
                    saveHist = true,
                    saveType = "Адрес",
                    saveDate = false
                };
            }
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            chrome = new ChromiumWebBrowser("https://" + setp.startPage);
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.TitleChanged += Chrome_TitleChanged;
            tabControl1.SelectedTab.Controls.Add(chrome);
        }

        private void Chrome_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tabControl1.SelectedTab.Text = e.Title;
                if (setp.saveType == "Адрес")
                {
                    AddHistory(adress);
                }
                else
                {
                    AddHistory(e.Title);
                }
            }));
        }

        private void Chrome_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                tabControl1.SelectedTab.Text = e.Address;
                textBox1.Text = e.Address;
                adress = e.Address;
            }));
        }

        private void MakoprBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TabPage tb = new TabPage();
            tb.Text = "Новая вкладка";
            ChromiumWebBrowser chrome = new ChromiumWebBrowser("https://" + setp.startPage);
            chrome.AddressChanged += Chrome_AddressChanged;
            chrome.TitleChanged += Chrome_TitleChanged;
            tb.Controls.Add(chrome);
            tabControl1.TabPages.Add(tb);
            tabControl1.SelectTab(tabControl1.TabCount - 1);
            i++;
            SelectTab++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (Regex.IsMatch(textBox1.Text, @"^http\w*"))
            {
                if (chrome != null)
                    chrome.Load(textBox1.Text);
            }
            else
            {
                if (setp.searchSys == "Яндекс")
                {
                    chrome.Load("https://yandex.ru/search/?text=" + textBox1.Text);
                }
                else if (setp.searchSys == "Google")
                {
                    chrome.Load("https://www.google.ru/search?q=" + textBox1.Text);
                }
                else if (setp.searchSys == "Mail.ru")
                {
                    chrome.Load("https://mail.ru/search?search_source=mailru_desktop_safe&text=" + textBox1.Text);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null && chrome.CanGoForward)
                chrome.Forward();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null && chrome.CanGoBack)
                chrome.Back();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            if (chrome != null)
                chrome.Reload();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bookmarks bookmarks = new Bookmarks();
            bookmarks.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                i--;
                SelectTab--;
            }
            else
            {
                Application.Exit();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(sender, e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MakoprBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void tabControl1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.H)
            {
                button6_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.T)
            {
                button5_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.W)
            {
                button8_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.J)
            {
                Process.Start("C:\\Users\\macim\\Downloads");
            }
            else if (e.Control == true && e.KeyCode == Keys.Left)
            {
                if (tabControl1.SelectedIndex != 0)
                {
                    tabControl1.SelectedTab.Controls[0].Focus();
                }
            }
            else if (e.Control == true && e.KeyCode == Keys.Right)
            {
                if (tabControl1.TabPages.Count != tabControl1.SelectedIndex && tabControl1.SelectedIndex != 0)
                {
                    tabControl1.SelectedTab.Controls[0].Focus();
                }
            }
        }

        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.H)
            {
                button6_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.T)
            {
                button5_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.W)
            {
                button8_Click(sender, e);
            }
            else if (e.Control == true && e.KeyCode == Keys.J)
            {
                Process.Start("C:\\Users\\macim\\Downloads");
            }
            else if (e.Control == true && e.KeyCode == Keys.Left)
            {
                if (tabControl1.SelectedIndex != 0)
                {
                    tabControl1.SelectedTab.Controls[0].Focus();
                }
            }
            else if (e.Control == true && e.KeyCode == Keys.Right)
            {
                if (tabControl1.SelectedIndex != i)
                {
                    tabControl1.SelectedTab.Controls[0].Focus();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\Users\\macim\\Downloads");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser chrome = tabControl1.SelectedTab.Controls[0] as ChromiumWebBrowser;
            chrome.Load("https://" + setp.startPage);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BilDestek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DB db;
        private void Form1_Load(object sender, EventArgs e)
        {
            hideForm();
            textBox1.ReadOnly = true;
            db = new DB();
            //db.register("deneme", "deneme");
            db.disconnected += db_disconnected;
            AddApplicationToStartup();
        }

        void db_disconnected()
        {
            i = 5;
        }


        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = i.ToString();
            if (i > 0)
            { textBox1.BackColor = Color.Red; i--; return; }

            Computer comp = new Computer();

            try
            {
                comp.id = db.register(comp.mac, comp.ipAddress,comp.username);

                Task t = db.getTask(comp);
                if (t.isReal())
                {
                    t.run();
                    db.endTask(comp, t);
                }
                
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            if(i==0)
                textBox1.BackColor = Color.Green;

            label1.Text = comp.mac;
            label2.Text = comp.ipAddress;
            label3.Text = comp.id.ToString();
            label8.Text = comp.username;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showForm();
        }

        private void hideForm()
        {
            Visible = false;
            ShowInTaskbar = false;
        }
        private void showForm()
        {
            Visible = true;
            ShowInTaskbar = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            hideForm();
        }

        public static void AddApplicationToStartup()
        {
            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("My Program", "\"" + Application.ExecutablePath + "\"");
            }
        }

    }
}

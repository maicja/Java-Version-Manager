using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Java_Version_Manager
{
    public partial class jarlauncher : Form
    {
        public jarlauncher()
        {
            InitializeComponent();
        }
        public static string jarpath = "";

        public static string homedir = "C:\\maicjadir\\javamanager";
        public static string settingsfile = "C:\\maicjadir\\javamanager\\settings.mks";
        public static string javadir = "";
        private void jarlauncher_Load(object sender, EventArgs e)
        {
            label2.Text = jarpath;
            if (!Directory.Exists(homedir))
            {
                Directory.CreateDirectory(homedir);
            }
            if (!File.Exists(settingsfile))
            {
                using (FileStream fs = File.Create(settingsfile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("javadir|C:\\JavaVersions");

                    fs.Write(info, 0, info.Length);
                }
            }
            foreach (string line in File.ReadAllLines(settingsfile))
            {
                string[] setting = line.Split('|');
                switch (setting[0])
                {
                    case "javadir":
                        javadir = setting[1];
                        break;
                }
            }
            if (!Directory.Exists(javadir))
            {
                Directory.CreateDirectory(javadir);
            }
            refreshversions();
        }

        public void refreshversions()
        {
            string saved = "";

            try
            {
                saved = comboBox1.SelectedItem.ToString();
            }
            catch (Exception) { }
            comboBox1.Items.Clear();
            foreach (string dir in Directory.GetDirectories(javadir))
            {
                comboBox1.Items.Add(Path.GetFileName(dir));
            }
            try
            {
                comboBox1.SelectedItem = saved;
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                Process.Start(javadir + "\\" + comboBox1.SelectedItem.ToString() + "\\bin\\java.exe", $"-jar \"{jarpath}\"");
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Launch error");
            }
        }
    }
}

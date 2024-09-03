using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Java_Version_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string homedir = "C:\\maicjadir\\javamanager";
        public static string settingsfile = "C:\\maicjadir\\javamanager\\settings.mks";
        public static string javadir = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            if (!File.Exists(settingsfile))
            {
                using (FileStream fs = File.Create(settingsfile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("javadir|C:\\Java\\Versions");

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
            foreach (string dir in Directory.GetDirectories(javadir))
            {
                comboBox1.Items.Add(Path.GetFileName(dir));
            }
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            foreach (string var in currentPath.Split(';'))
            {
                foreach (var item in comboBox1.Items)
                {
                    if (var.Contains($"\\{item}\\"))
                    {
                        selectedjava.Text = item.ToString();
                    }
                }
                
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
          
            settings.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem.ToString().Length > 0)
                {
                    


                    string currentPath1 = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

                    string newpath1 = javadir + "\\" + comboBox1.SelectedItem.ToString() + "\\bin";
                    foreach (string var in currentPath1.Split(';'))
                    {
                        if (!var.Contains("\\Java\\"))
                        {
                            newpath1 = newpath1 + ";" + var;
                        }
                    }
                    Environment.SetEnvironmentVariable("PATH", newpath1, EnvironmentVariableTarget.User);

                    
             
                    selectedjava.Text = comboBox1.SelectedItem.ToString();
                }
                else
                {
                    MessageBox.Show("Select java version from list, if you do not see your java version import it from settings");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select java version from list, if you do not see your java version import it from settings");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refreshversions();
        }
        public  void refreshversions()
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
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            foreach (string var in currentPath.Split(';'))
            {
                foreach (var item in comboBox1.Items)
                {
                    if (var.Contains($"\\{item}\\"))
                    {
                        selectedjava.Text = item.ToString();
                    }
                }


            }
            try
            {
                comboBox1.SelectedItem = saved;
            }
            catch (Exception) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem.ToString().Length > 0)
                {
                    foreach (var item in Directory.GetFiles(javadir + "\\" + comboBox1.SelectedItem.ToString(), ".", SearchOption.AllDirectories))
                    {
                        FileInfo file = new FileInfo(item);

                        
                            file.IsReadOnly = false;
                            File.Delete(item);
                        
                    }
                    Directory.Delete(javadir+"\\"+comboBox1.SelectedItem.ToString(), true);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error while deleting java version"); }
        }

    }
}

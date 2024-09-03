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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Path where all your java versions are located", "Java directory help");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Import folder with your java version. Example:\r\n\r\nC:\\user\\downloads\\java17\\bin\\java.exe\r\n\r\nYou need to select folder that contains \"bin\" directory. In this example it will be \"java17\".", "Import java version help");
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            javadir.Text = Form1.javadir;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!javadir.Text.Contains("\\Java\\"))
            {
                MessageBox.Show("Java path MUST contain \"\\Java\\\"");
                return;
            }
            string newsettings = "";
            foreach (string line in File.ReadAllLines(Form1.settingsfile))
            {
                string[] setting = line.Split('|');
                switch (setting[0])
                {
                    case "javadir":
                        newsettings = newsettings + setting[0] + "|" + javadir.Text;
                        break;
                }
            }
            File.WriteAllText(newsettings, Form1.settingsfile);
            MessageBox.Show("Restart required", "Settings updated");
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select your java folder";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog.ShowNewFolderButton = false;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    if (File.Exists(selectedPath + "\\bin\\java.exe"))
                    {
          
                        string targerdir = Form1.javadir + "\\" + Path.GetFileName(selectedPath);
                        while (Directory.Exists(targerdir))
                        {
                            targerdir = targerdir + "-0";
                        }
         
                        CopyDirectory(selectedPath, targerdir);
                        MessageBox.Show("Imported!");
                    }
                    else
                    {
                        MessageBox.Show(selectedPath + " Is not valid java version! There is no /bin/java.exe");
                    }
                   
                }
                else
                {
                    MessageBox.Show("There was an error while selecting folder");
                }
            }
        }

        static void CopyDirectory(string sourceDir, string destinationDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDir}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(tempPath, false);
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destinationDir, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }
    }
}

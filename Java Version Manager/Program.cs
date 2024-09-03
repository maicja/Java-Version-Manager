using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Java_Version_Manager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [DllImport("shell32.dll")]
        internal static extern bool IsUserAnAdmin();


        static bool IsFileExtensionAssociated(string fileExtension, string progID)
        {
            // Open the registry key for the file extension
            RegistryKey extKey = Registry.ClassesRoot.OpenSubKey(fileExtension);
            if (extKey != null)
            {
                // Get the default value of the key
                string associatedProgID = extKey.GetValue("") as string;
                extKey.Close();

                // Check if the associated ProgID matches the expected ProgID
                return associatedProgID == progID;
            }
            return false;
        }

        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                if (!Directory.Exists("C:\\maicjadir\\javamanager"))
                {
                    Directory.CreateDirectory("C:\\maicjadir\\javamanager");
                }
                File.Copy(Assembly.GetExecutingAssembly().Location, "C:\\maicjadir\\javamanager\\JavaVersionManager.exe", true);

                if (!IsFileExtensionAssociated(".jar", "mks.maicja.jvm"))
                {
                    if (IsUserAnAdmin())
                    {
                        string fileExtension = ".jar";
                        string progID = "mks.maicja.jvm";
                        string applicationPath = "C:\\maicjadir\\javamanager\\JavaVersionManager.exe";
                        RegistryKey extKey = Registry.ClassesRoot.CreateSubKey(fileExtension);
                        extKey.SetValue("", progID);
                        extKey.Close();
                        RegistryKey progIDKey = Registry.ClassesRoot.CreateSubKey(progID);
                        progIDKey.SetValue("", "My Application File");
                        RegistryKey commandKey = progIDKey.CreateSubKey(@"shell\open\command");
                        commandKey.SetValue("", $"\"{applicationPath}\" \"%1\"");
                        commandKey.Close();
                    }
                    else
                    {
                        MessageBox.Show("If you want to associate .jar file extension with this program, run it as administrator");

                    }
                }
            }
            catch (Exception)
            {
            }
            

    

            string illegalfiles = "appletviewer.exe|extcheck.exe|idlj.exe|jabswitch.exe|jar.exe|jarsigner.exe|java-rmi.exe|java.exe|javac.exe|javadoc.exe|javafxpackager.exe|javah.exe|javap.exe|javapackager.exe|javaw.exe|javaws.exe|jcmd.exe|jconsole.exe|jdb.exe|jdeps.exe|jhat.exe|jinfo.exe|jjs.exe|jli.dll|jmap.exe|jmc.exe|jmc.ini|jps.exe|jrunscript.exe|jsadebugd.exe|jstack.exe|jstat.exe|jstatd.exe|jvisualvm.exe|keytool.exe|kinit.exe|ktab.exe|msvcr100.dll|native2ascii.exe|orbd.exe|pack200.exe|policytool.exe|rmic.exe|rmid.exe|rmiregistry.exe|schemagen.exe|serialver.exe|servertool.exe|tnameserv.exe|unpack200.exe|wsgen.exe|wsimport.exe|xjc.exe";
            string vars = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            if (vars.Contains("\\JAVA\\"))
            {
                if (!IsUserAnAdmin())
                {
                    MessageBox.Show("Java Environment Variables on administrator level detected. To run, you must first run this program with administrator privileges.\r\n\r\nRemember that this will disable your previous java installations", "Environment Variable Error");
                    Application.Exit();
                    return;
                }
                string newpath1 = "C:\\exedir";
                foreach (string var in vars.Split(';'))
                {
                    if (!var.Contains("\\Java\\"))
                    {
                        newpath1 = newpath1 + ";" + var;
                    }
                }
                Environment.SetEnvironmentVariable("PATH", newpath1, EnvironmentVariableTarget.Machine);
                MessageBox.Show("Cleaned Java Environment Variables on administrator level!");
            }
            foreach (var file in illegalfiles.Split('|'))
            {
                if (File.Exists("C:\\Windows\\System32\\" + file))
                {
                    if (!IsUserAnAdmin())
                    {
                        MessageBox.Show("Java files in windows detected. To run, you must first run this program with administrator privileges.\r\n\r\nRemember that this will remove your previous java installations", "Java System32 Error");
                        Application.Exit();
                        return;
                    }

                    ProcessStartInfo dsa = new ProcessStartInfo();
                    dsa.CreateNoWindow = true;
    
                    
                    dsa.WorkingDirectory = "C:\\Windows\\System32";
                    dsa.FileName = "cmd.exe";
                    dsa.Arguments = $"/C del {file}";
                    Process.Start(dsa);

                }
            }
            try
            {
                jarlauncher.jarpath = args[0];
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new jarlauncher());
            }
            catch (Exception)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
           
        }
    }
}

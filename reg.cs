using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;
namespace ExampleCsPlugin
{
    [RunInstaller(true)]
    public class CustomInstaller : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    string regFilePath = $"{path}\\Default Company Name\\plugin_setup\\plugin.reg";
                    Console.WriteLine(regFilePath);

                    Process process = new Process();
                    process.StartInfo.FileName = "regedit.exe";
                    process.StartInfo.Arguments = $"\"{regFilePath}\""; // 使用 /s 参数以静默模式运行
                    process.Start();
                    process.WaitForExit();
                    /*RegistryKey regHKCR = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

                    RegistryKey regPlugin = regHKCR.CreateSubKey("ExampleCsPlugin.MyPlugin");
                    regPlugin.SetValue("", "ExampleCsPlugin.MyPlugin");

                    RegistryKey regCLSID = regHKCR.CreateSubKey("ExampleCsPlugin.MyPlugin\\CLSID");
                    regCLSID.SetValue("", "{D765C6EE-477A-4819-9809-BBF1C16F675D}");

                    RegistryKey regClsid = regHKCR.CreateSubKey("CLSID\\{D765C6EE-477A-4819-9809-BBF1C16F675D}");
                    regClsid.SetValue("", "ExampleCsPlugin.MyPlugin");
                    RegistryKey regImplement1 = regClsid.CreateSubKey("Implemented Categories\\{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}");
                    RegistryKey regImplement2 = regClsid.CreateSubKey("Implemented Categories\\{3494FA92-B139-4730-9591-01135D5E7831}");

                    RegistryKey regProgid = regClsid.CreateSubKey("ProgId");
                    regProgid.SetValue("", "ExampleCsPlugin.MyPlugin");
                    RegistryKey regInproc = regClsid.CreateSubKey("InprocServer32");
                    regInproc.SetValue("ThreadingModel", "Both");
                    regInproc.SetValue("Class", "ExampleCsPlugin.MyPlugin");
                    regInproc.SetValue("Assembly", "ExampleCsPlugin, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null");
                    regInproc.SetValue("RuntimeVersion", "v4.0.30319");
                    regInproc.SetValue("CodeBase", "file:///C:\\Program Files (x86)\\xg\\svn_plugin\\ExampleCsPlugin.dll");
                    regInproc.SetValue("", "mscoree.dll");

                    RegistryKey regInproc_1 = regClsid.CreateSubKey("1.1.0.0");
                    regInproc_1.SetValue("Class", "ExampleCsPlugin.MyPlugin");
                    regInproc_1.SetValue("Assembly", "ExampleCsPlugin, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null");
                    regInproc_1.SetValue("RuntimeVersion", "v4.0.30319");
                    regInproc_1.SetValue("CodeBase", "file:///C:\\Program Files (x86)\\xg\\svn_plugin\\ExampleCsPlugin.dll");*/

                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }
            }



        }


    }
}

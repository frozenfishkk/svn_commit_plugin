using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Diagnostics;
using System.Web;
using System.Data;
using System.Security.Policy;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace ExampleCsPlugin
{
    [ComVisible(true),
        Guid("D765C6EE-477A-4819-9809-BBF1C16F675D"),
        ClassInterface(ClassInterfaceType.None)]
    public class NestedListComparer : IComparer<List<string>>
    {
        public int Compare(List<string> x, List<string> y)
        {
            var order = new Dictionary<string, int>
        {
            { "开始", 0 },
            { "开发中", 1 },
            { "策划验收中", 2 },
            { "测试中", 3 },
                { "缺陷修复中",4},
                {"缺陷测试中",5 }

        };

            int indexX = order[x[2].ToString()];
            int indexY = order[y[2].ToString()];

            return indexX.CompareTo(indexY);
        }
    }
    public class MyPlugin : Interop.BugTraqProvider.IBugTraqProvider, Interop.BugTraqProvider.IBugTraqProvider2
    {
        public string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\SvnPluginConfig.json";
        public string current_svn_dir;
        public string root_svn_url;
        public bool check;
        private List<TicketItem> selectedTickets = new List<TicketItem>();
        public string wronglist;
        public string commonRootDir;

        public List<string> parseSvnStatus(string svnPath)
        {
            List<string> filePathList = new List<string>();
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c svn status {svnPath}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
            string line;
            string path;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                if (line.StartsWith("M"))
                {
                    path = line.Replace("M", "").Trim();
                    filePathList.Add(path);


                }
            }
            return filePathList;
        }
        public string getCommonUrl(string commonroot)
        {
            string decodedString;
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c svn info {commonroot}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,

            };
            process.Start();
            string line;
            string url;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                if (line.StartsWith("URL:"))
                {
                    url = line.Replace("URL:", "").Trim();
                    decodedString = Mono.Web.HttpUtility.UrlDecode(url, System.Text.Encoding.UTF8);
                    return decodedString;
                }
            }
            return "";
        }
        public Dictionary<string, string> getFileUrlList(List<string> filePathList)
        {
            Dictionary<string, string> fileWithUrl = new Dictionary<string, string>();
            foreach (string filePath in filePathList)
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c svn info {filePath}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,

                };
                process.Start();
                string line;
                string url;
                while ((line = process.StandardOutput.ReadLine()) != null)
                {
                    if (line.StartsWith("URL:"))
                    {
                        url = line.Replace("URL:", "").Trim();
                        string decodedString = Mono.Web.HttpUtility.UrlDecode(url, System.Text.Encoding.UTF8);
                        fileWithUrl.Add(filePath, decodedString);
                    }
                    if (line.StartsWith("工作副本根目录:"))
                    {
                        commonRootDir = line.Replace("工作副本根目录:", "").Trim();

                    }
                    else if (line.StartsWith("Working Copy Root Path:"))
                    {
                        commonRootDir = line.Replace("Working Copy Root Path:", "").Trim();
                    }
                }
            }

            return fileWithUrl;
        }
        public string[] splitUrl(string url)
        {
            string[] fotmattedUrl;
            if (url.Contains("\\")) { fotmattedUrl = url.Split('\\'); }
            else { fotmattedUrl = url.Split('/'); }
            return fotmattedUrl;
        }
        public bool checkSvnUrl(string commonurl, Dictionary<string, string> filewithurl)
        {
            bool Except = false;
            string subUrl1 = commonurl.Substring(0, commonurl.IndexOf("/svn/Aquaman") + "/svn/Aquaman".Length);//http://svn.xgjoy.org/svn/Aquaman

            foreach (var pair in filewithurl)
            {
                string rooturl = subUrl1 + "/" + splitUrl(pair.Value)[5];

                if (!pair.Value.Contains(commonurl))
                {
                    wronglist += ($"文件名:{pair.Key}  url:{pair.Value} \n");

                    Except = true;

                }

            }
            return Except;
        }

        public bool ValidateParameters(IntPtr hParentWnd, string parameters)
        {
            return true;
        }

        public string GetLinkText(IntPtr hParentWnd, string parameters)
        {
            return "选择提交需求";
        }

        public string GetCommitMessage(IntPtr hParentWnd, string parameters, string commonRoot, string[] pathList,
                                       string originalMessage)
        {
            string[] revPropNames = new string[0];
            string[] revPropValues = new string[0];
            string dummystring = "";
            return GetCommitMessage2(hParentWnd, parameters, "", commonRoot, pathList, originalMessage, "", out dummystring, out revPropNames, out revPropValues);
        }


        public string GetCommitMessage2(IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList,
                               string originalMessage, string bugID, out string bugIDOut, out string[] revPropNames, out string[] revPropValues)
        {


            List<TicketItem> tickets = new List<TicketItem>();
            List<List<string>> workItemList = new List<List<string>>();
            workItemList = request.Exec().Result;
            workItemList.Sort(new NestedListComparer());

            foreach (var item in workItemList)
            {
                tickets.Add(new TicketItem(item[1], item[2], item[0], item[3]));
                string info = string.Join(",", item);
                listener.info($"请求响应体{info}");
                listener.FlushLogs();
            }


            /*
                            tickets.Add(new TicketItem(88, commonRoot));
                            foreach (string path in pathList)
                                tickets.Add(new TicketItem(99, path));
             */
            revPropNames = new string[0];
            revPropValues = new string[0];


            bugIDOut = bugID;

            MyIssuesForm form = new MyIssuesForm(tickets);
            if (form.ShowDialog() != DialogResult.OK)
                return originalMessage;

            StringBuilder result = new StringBuilder(originalMessage);
            if (originalMessage.Length != 0 && !originalMessage.EndsWith("\n"))
                result.AppendLine();

            foreach (TicketItem ticket in form.TicketsFixed)
            {
                result.AppendFormat("#{0}: {1} https://project.feishu.cn/flowaquaman/story/detail/{2}", ticket.Number, ticket.Summary, ticket.Number);
                selectedTickets.Add(ticket);
            }


            return result.ToString();
        }


        private bool loadConfig()
        {
            string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\SvnPluginConfig.json";
            try { File.ReadAllText(ConfigPath); }
            catch (Exception ex) { listener.error($"读取配置文件失败{ex.Message}"); listener.FlushLogs(); }
            string jsonContent = File.ReadAllText(ConfigPath);
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            string check = data["check"];
            if (check == "1") { return true; }
            else { return false; }
        }
        private string cleanWronglist()
        {
            wronglist = "";
            return "";
        }
        public void CreateConfig()
        {
            if (File.Exists(ConfigPath)) { return; }
            try { File.Create(ConfigPath).Close(); }
            catch (Exception e)
            {
                listener.info(e.Message);
                listener.FlushLogs();
            }
            var data = new
            {
                version = "1.0.0",
                check = "1"
            };
            string jsonString = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConfigPath, jsonString);
        }
        public bool checkUpdate()
        {

            using (StreamReader file = File.OpenText(ConfigPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, string> data = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
                string current_version = data["version"];
                string remote_version = MyIssuesForm.getRemoteVersion();
                if (current_version != remote_version)
                {

                    return true;
                }
            }
            return false;
        }
        public string CheckCommit(IntPtr hParentWnd, string parameters, string commonURL, string commonRoot, string[] pathList, string commitMessage)
        {

            CreateConfig();
            if (checkUpdate())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {

                    FileName = "C:\\Program Files (x86)\\Default Company Name\\svn_plugin\\AutoUpdate.exe",
                    Arguments = "--p \"http://effectplatform.xgjoy.org/aquaman/file/download?filename=svn_plugin.zip\" --d \"C:\\Program Files (x86)\\Default Company Name\\svn_plugin\""
                };
                try { Process process = Process.Start(startInfo); process.WaitForExit(); }
                catch (Exception ex)
                {
                    listener.error($"自动更新失败{ex.Message}");
                    listener.FlushLogs(); ;
                }
            }
        

            if (loadConfig() == false) { return ""; }
            List<string> filepathlist = parseSvnStatus(commonRoot);
            Dictionary<string, string> fileurllist = getFileUrlList(filepathlist);//空的
            string url = getCommonUrl(commonRootDir);
            listener.info($"分支检查{wronglist},commonurl {url},commonroot {commonRoot},  fplist {filepathlist}, fulist  {fileurllist} commonRootDir{commonRootDir} " );
            listener.FlushLogs();
            if (checkSvnUrl(url, fileurllist))
            {

                return $"发现文件的svn url不符合当前分支根url：{url}\n不符合的文件:\n{wronglist}{cleanWronglist()}";

            }
            else
            {
                return ""; }

        }

        public string OnCommitFinished( IntPtr hParentWnd, string commonRoot, string[] pathList, string logMessage, int revision )
        {
            // we now could use the selectedTickets member to find out which tickets
            // were assigned to this commit.
/*            AutoUpdateForm form = new AutoUpdateForm( selectedTickets );
            if ( form.ShowDialog( ) != DialogResult.OK )
                return "";
            // just for testing, we return an error string*/
            return "";
        }

        public bool HasOptions()
        {
            return true;
        }

        public  string ShowOptionsDialog( IntPtr hParentWnd, string parameters )
        {

            return "";
        }



    }
}

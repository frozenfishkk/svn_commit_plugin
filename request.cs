using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ExampleCsPlugin
    
{
    
    public class request
    {

        public static string GetUserName()
        {
            listener.checkLogB();
            string name = "";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\Subversion\\auth\\svn.simple";
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                int num =0;
                bool i = false;
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    num++;
                    if (i == true)
                    {
                        
                        break;
                    }

                    if (line.StartsWith("username"))
                    {
                        num += 1;
                        break;
                    }
                }
                name = lines[num];
                listener.info($"本地用户名{name}");
                listener.FlushLogs();
                return name;
            }
            
            return "没找到用户名";
        }
        public static async Task<List<List<string>>> Exec()
        {   
            using (HttpClient client = new HttpClient())
            {
                Stopwatch stopwatch = new Stopwatch();
                string name = GetUserName();
                
                try { HttpResponseMessage r = await client.GetAsync($"http://robot-dev.xgjoy.org:10086/robot/dev/aquaman/svn/SVNCommitMessage?name={name}"); }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("连接失败,请检查网络(vpn)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listener.info($"请求机器人失败,请检查网络(联网是否正常,vpn)");
                    listener.FlushLogs();
                }
                HttpResponseMessage response = await client.GetAsync($"http://robot-dev.xgjoy.org:10086/robot/dev/aquaman/svn/SVNCommitMessage?name={name}");
                string responseBody = await response.Content.ReadAsStringAsync();
                
                // 输出响应内容
                List<List<string>> itemList = new List<List<string>>();
                itemList = JsonConvert.DeserializeObject<List<List<string>>>(responseBody);

                return itemList;
                

                
            }

        }
    }
}


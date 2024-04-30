using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using AutoUpdate;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Security.Policy;
using System.IO;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.AxHost;

namespace ExampleCsPlugin
{
    public struct Item
    {
        public string WorkID;
        public string State;
        public string Name;
        public string Planning;

        public Item(string workid,string state,string name ,string planning)
           
        {
            WorkID = workid;
            State = state;
            Name = name;
            Planning = planning;
        }
    }
    public static class listener
    {
        public static TraceSource myLogger = new TraceSource("MyLogger");
        public static TextWriterTraceListener myListener;
        static string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SvnPluginLog.txt");
        static listener()
        {   if (!File.Exists(logFilePath)){ File.Create(logFilePath).Close(); }
            
            myListener = new TextWriterTraceListener(logFilePath);
            myLogger.Listeners.Add(myListener);
            myLogger.Switch.Level = SourceLevels.All;
        }
        public static void checkLogB()
        {
            long maxLogFileSizeInBytes = 2 * 1024 * 1024;
            FileInfo logFileInfo = new FileInfo(logFilePath);
            if (logFileInfo.Length > maxLogFileSizeInBytes)
            {
                File.WriteAllText(logFilePath, string.Empty);
            }
            }
        public static void info(string message)
        {
            listener.myLogger.TraceEvent(System.Diagnostics.TraceEventType.Information, 1,$"{DateTime.Now}{message}" );
        }
        public static void error(string message)
        {
            listener.myLogger.TraceEvent(System.Diagnostics.TraceEventType.Error, 2,$"{DateTime.Now}{message}");
        }
        public static void FlushLogs()
        {
            myLogger.Flush();
            myListener.Flush();
        }
    }
    partial class MyIssuesForm : Form
    {
        private readonly IEnumerable<TicketItem> _tickets;
        private readonly List<TicketItem> _ticketsAffected = new List<TicketItem>();
        private string selectedStatus = "";
        private string text = "";
        private string plan = "";
        public string current_version;
        private int column0_width = 50;
        static string install_path = "C:\\Program Files (x86)\\Default Company Name\\svn_plugin";
        public string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\SvnPluginConfig.json";
        public string jsonContent = "";


        public Autoupdate updater = new Autoupdate();
        public MyIssuesForm(IEnumerable<TicketItem> tickets)
        {
            CreateConfig();
            InitializeComponent();
            _tickets = tickets;
        }

        public IEnumerable<TicketItem> TicketsFixed
        {
            get { return _ticketsAffected; }
        }
        private System.Windows.Forms.Button getCopyButton(ListViewItem lvi)
        {
            System.Windows.Forms.Button copyButton = new System.Windows.Forms.Button();
            copyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            copyButton.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            copyButton.Text = "选择";
            copyButton.Size = new Size(column0_width, lvi.SubItems[1].Bounds.Height);
            copyButton.Location = new Point(lvi.SubItems[0].Bounds.Left, lvi.SubItems[1].Bounds.Top);
            return copyButton;

        }
        public List<Item> getStructList()
        {   
            List<Item> itemlist = new List<Item>();
            foreach (TicketItem ticketItem in _tickets)
            {
                Item item = new Item(ticketItem.Number, ticketItem.State, ticketItem.Summary, ticketItem.Planning);
                itemlist.Add(item);
            }
            return itemlist;
        }
        public List<Item> FilterState(List<Item> itemlist,string state)

        {
            List<Item> items = new List<Item>();
            foreach (Item item in itemlist)
            {   
                if(item.State == state) { items.Add(item); }
                
            }
                return items;
        }
        public List<Item> FilterName(List<Item> itemlist, string text)

        {
            List<Item> items = new List<Item>();
            foreach (Item item in itemlist)
            {
                if (item.Name.Contains(text) ) { items.Add(item); }

            }
            return items;
        }
        public List<Item> FilterPlanning(List<Item> itemlist, string planning)

        {
            List<Item> items = new List<Item>();
            foreach (Item item in itemlist)
            {
                if (item.Planning==planning) { items.Add(item); }

            }
            return items;
        }
        public void addItem(List<Item> itemlist)
        {
            foreach (Item item in itemlist)
            {
                ListViewItem lvi = new ListViewItem();
                /*lvi.ImageList.Images.Add("key1",Image.FromFile("btn.png"));*/
                lvi.SubItems.Add(item.WorkID);
                lvi.SubItems.Add(item.State);
                lvi.SubItems.Add(item.Name);
                lvi.SubItems.Add(item.Planning);
                listView1.Items.Add(lvi);
                System.Windows.Forms.Button copyButton = getCopyButton(lvi);
                copyButton.Click += (sender, e) =>
                {

                    lvi.Selected = true;
                    TicketItem ticketItem = new TicketItem(item.WorkID, item.State, item.Name, item.Planning);
                    if (ticketItem != null && lvi.Selected)
                        _ticketsAffected.Add(ticketItem);

                };
                // 将按钮添加到窗体
                listView1.Controls.Add(copyButton);

            }
        }
       
        
        private void loadComboboxPlanning(List<Item> itemlist)
        {   
            List<string> planningList = new List<string>();
            foreach (Item item in itemlist)
            {
                if (!planningList.Contains(item.Planning)){
                    planningList.Add(item.Planning);
                }
            }
            comboBoxPlanning.Items.Add("全部计划");
            foreach(string planning in planningList)
            {
                comboBoxPlanning.Items.Add(planning);
            }
            

        }
        public void loadListView(string status = "", string text = "", string planning = "")
        {
            List<Item> itemlist = getStructList();
            
            if (status == "全部") { }
            else if (status != "") { itemlist = FilterState(itemlist, status); }
            if (text != "") { itemlist = FilterName(itemlist, text); }
            if (planning == "全部计划") { }
            else if (planning != "") { itemlist =FilterPlanning(itemlist, planning); }
            if(itemlist != null) { addItem(itemlist); }
            listView_font_change();
            listView1.Columns[0].Width = column0_width;
            listView1.Columns[1].Width = 100;
            listView1.Columns[2].Width = -2;
            listView1.Columns[3].Width = 500;
            listView1.Columns[4].Width = 300;

        }

        public static string getRemoteVersion()
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://effectplatform.xgjoy.org/aquaman/file/fileList";
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string content = response.Content.ReadAsStringAsync().Result;
                        var data = JObject.Parse(content);
                        foreach (var item in data["data"]["items"])
                        {
                            if (item["filename"].ToString() == "svn_plugin.zip")
                            {
                                listener.info($"远程版本为{item["version"].ToString()}");
                                listener.FlushLogs();
                                return item["version"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e) { MessageBox.Show("连接失败,请检查网络(vpn)", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                return "";
            }
        }
        public bool checkUpdate()
        {
            
            using (StreamReader file = File.OpenText(ConfigPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, string> data = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
                current_version = data["version"];
                string remote_version = getRemoteVersion();
                if (current_version != remote_version)
                {

                    return true;
                }
            }
            return false;
        }
        public void CreateConfig()
        {
            if (File.Exists(ConfigPath)){ return; }
            try { File.Create(ConfigPath).Close(); }
            catch(Exception e) { listener.info(e.Message);
                listener.FlushLogs();
            }
            var data = new {
                version = "1.0.0",
                check = "1"
            };
            string jsonString = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConfigPath, jsonString);
        }
        private void MyIssuesForm_Load(object sender, EventArgs e)
        {

            listView1.Columns.Add("");
            listView1.Columns.Add("workID");
            listView1.Columns.Add("任务状态");
            listView1.Columns.Add("工作项名称");
            listView1.Columns.Add("规划迭代");
            List<Item> itemlist = getStructList();
            loadComboboxPlanning(itemlist);
            comboBoxSearchStatus.SelectedIndexChanged += comboBoxSearchStatus_SelectedIndexChanged;
            comboBoxPlanning.SelectedIndexChanged += comboboxPlanning_SelectedIndexChanged;
            searchButton.Click += searchButton_Click;
            listView1.MouseClick += listView_MouseDown;
            listView1.ItemMouseHover += listView1_ItemMouseHover;

            loadListView();
            loadConfig();
            comboBoxPlanning.SelectedIndex = 0;
            listView_font_change();
            if (!checkUpdate()) { return; }
            
            ProcessStartInfo startInfo = new ProcessStartInfo {

                FileName = "C:\\Program Files (x86)\\Default Company Name\\svn_plugin\\AutoUpdate.exe",
                Arguments = "--p \"http://effectplatform.xgjoy.org/aquaman/file/download?filename=svn_plugin.zip\" --d \"C:\\Program Files (x86)\\Default Company Name\\svn_plugin\""
            };
            try { Process process = Process.Start(startInfo); process.WaitForExit(); }
            catch (Exception ex) {
                listener.error($"自动更新失败{ex.Message}");
                listener.FlushLogs(); ; }
        }
        private void comboBoxSearchStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当用户选择新的项时，这个事件会被触发
            // 可以在这里编写处理选中项变化的逻辑
            int selectedIndex = comboBoxSearchStatus.SelectedIndex;
            string selectedText = comboBoxSearchStatus.SelectedItem.ToString();
            selectedStatus = selectedText;
            listView1.Items.Clear();
            listView1.Controls.Clear();
            loadListView(status:selectedStatus, text:text,planning:plan); 

        }
        private void comboboxPlanning_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当用户选择新的项时，这个事件会被触发
            // 可以在这里编写处理选中项变化的逻辑
            string selectedText = comboBoxPlanning.SelectedItem.ToString();
            plan = selectedText;
            listView1.Items.Clear();
            listView1.Controls.Clear();
            loadListView(status: selectedStatus, text: text, planning: plan);
        }

        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();


            string itemInfor =
            e.Item.SubItems[3].Text;

            toolTip.SetToolTip((e.Item).ListView, itemInfor);

        }
        private void okButton_Click(object sender, EventArgs e)
        {  
            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.Selected = true;
                TicketItem ticketItem = lvi.Tag as TicketItem;
                if (ticketItem != null && lvi.Selected)
                    _ticketsAffected.Add(ticketItem);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            text = textBoxSearch.Text;
            listView1.Items.Clear();
            listView1.Controls.Clear();
            loadListView(status: selectedStatus, text: text, planning: plan);
        }

            private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            var info = listView1.HitTest(e.X, e.Y);
            var row = info.Item.Index;
            var col = info.Item.SubItems.IndexOf(info.SubItem);
            var value = info.Item.SubItems[col].Text;
/*            MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));*/
            if (col == 1)
            {
                string url = $"https://project.feishu.cn/flowaquaman/story/detail/{value}";
                Process.Start(url);
            }
        }

        private void listViewItemDoubelClick(object sender, MouseEventArgs e)
        {
            var info = listView1.HitTest(e.X, e.Y);
            var row = info.Item.Index;
            var col = info.Item.SubItems.IndexOf(info.SubItem);
            var value = info.Item.SubItems[1].Text;
            if (col != 1)
            {

                    TicketItem ticketItem = new TicketItem(info.Item.SubItems[1].Text, info.Item.SubItems[2].Text, info.Item.SubItems[3].Text, info.Item.SubItems[4].Text);
                    if (ticketItem != null )
                        _ticketsAffected.Add(ticketItem);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                // 关闭当前窗体（假设当前窗体是对话框）
                /*this.Close();*/

            }
        }
        private void textBoxSearchKeyDown(object sender,KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void cancel(object sender, EventArgs e)
        {
            Close();
        }
        private void clearControl()
        {
            foreach (Control control in Controls)
            {
                if (control.Name == "listView1" || control.Name == "panel1") { Controls.Remove(control); }
            }
        }
        private void configButtonClicked(object sender, EventArgs  e)
        {
            clearControl();
            Controls.Add(panel1);
        }

        private void listViewButtonClicked(object sender, EventArgs e)
        {
            clearControl();
            Controls.Add(listView1);
        }
        public void optionsChecked(object sender, EventArgs e)
        {
            bool optionCheck = false;
            optionCheck = this.checkboxCheck.Checked;
            changeConfig(optionCheck);
            
        }
        private void loadConfig()
        {
            jsonContent = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) + "\\SvnPluginConfig.json");
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            string check = data["check"];
            if (check == "1") { checkboxCheck.Checked = true; }
            else { checkboxCheck.Checked = false; }
        }

        public void changeConfig(bool check)
        {
            string str;
            if (check == false) { str = "0"; }
            else { str = "1"; }
            string jsonString = "";
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            data["check"] = str;
            jsonString = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConfigPath, jsonString);
        }
        private void listView_font_change()
        {
            Font customFont = new Font("Arial", 9, FontStyle.Underline);
/*            listView1.Items[0].Font = customFont;*/
            foreach (ListViewItem lvi in listView1.Items)
            {   
                lvi.UseItemStyleForSubItems = false;
                lvi.SubItems[1].ForeColor = Color.CadetBlue;
                lvi.SubItems[1].Font = customFont;
            }
        }
        }

    }


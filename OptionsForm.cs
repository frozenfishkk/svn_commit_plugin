using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ExampleCsPlugin
{
    public partial class OptionsForm : Form
    {
        string  filepath = $"C:\\Program Files (x86)\\Default Company Name\\svn_plugin\\config.json";
        public OptionsForm( )
        {
            InitializeComponent( );
            loadConfig();
        }

        public void optionsChecked( object sender, EventArgs e)
        {
            bool optionCheck = false;
            optionCheck = this.checkboxCheck.Checked;
            changeConfig(optionCheck);
            this.Close();
        }
        private void loadConfig() {
            string jsonContent = File.ReadAllText(filepath);
            dynamic data = JsonConvert.DeserializeObject(jsonContent);
            string check = data["check"];
            if (check == "1") { checkboxCheck.Checked = true; }
            else {  checkboxCheck.Checked = false; }
        }
        
        public void changeConfig(bool check)
        {
            string str;
            if (check == false) {str="0"; }
            else { str="1"; }
            
            string jsonString = "";
            using (StreamReader file = File.OpenText(filepath))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, string> data = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
                data["check"] = str;
                jsonString = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

            }
            File.WriteAllText(filepath, jsonString);
        }
    }
}

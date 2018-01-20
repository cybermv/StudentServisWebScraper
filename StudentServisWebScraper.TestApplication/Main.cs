using Newtonsoft.Json;
using StudentServisWebScraper.TestApplication.Models;
using StudentServisWebScraper.TestApplication.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace StudentServisWebScraper.TestApplication
{
    public partial class Main : Form
    {
        public string ApiRootUrl { get; set; }

        public Main()
        {
            InitializeComponent();
            this.ApiRootUrl = Settings.Default.apiRootUrl;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            cbxCategory.Items.Add(new KeyValuePair<int, string>(-1, "Any category"));
            cbxCategory.Items.Add(new KeyValuePair<int, string>(0, "Unspecified"));

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };

            string categoriesJson = client.DownloadString(this.ApiRootUrl + "/api/jobs/categories");

            object[] categories = JsonConvert
                .DeserializeObject<List<CategoryModel>>(categoriesJson)
                .Select(c => new KeyValuePair<int, string>(c.Id, c.FriendlyName))
                .Cast<object>()
                .ToArray();

            cbxCategory.Items.AddRange(categories);
            
            cbxCategory.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int category = ((KeyValuePair<int, string>)cbxCategory.SelectedItem).Key;

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };

            if (category >= 0)
            {
                client.QueryString["categoryId"] = category.ToString();
            }

            if (!string.IsNullOrWhiteSpace(tbxFreeText.Text))
            {
                client.QueryString["contains"] = tbxFreeText.Text;
            }

            client.QueryString["minHourlyPay"] = numMinHourlyPay.Value.ToString();
            client.QueryString["excludeNonParsed"] = chkExcludeNonParsed.Checked.ToString();

            string jobsJson = client.DownloadString(this.ApiRootUrl + "/api/jobs/filter");

            List<JobModel> jobs = JsonConvert
                .DeserializeObject<List<JobModel>>(jobsJson);

            dgrResults.AutoGenerateColumns = false;
            dgrResults.DataSource = jobs;
        }
    }
}

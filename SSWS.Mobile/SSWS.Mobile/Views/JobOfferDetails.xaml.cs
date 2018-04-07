using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JobOfferDetails : ContentPage
	{
        public JobModel Job { get; set; }

        public JobOfferDetails(JobModel job)
		{
			InitializeComponent();
            Job = job;
            this.BindingContext = this;
		}

        private void ContactPhone_Tapped(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Job.ContactPhone)) return;
            string filteredPhone = new string(Job.ContactPhone.Where(char.IsDigit).ToArray());
            string uri = $"tel:{filteredPhone}";
            Device.OpenUri(new Uri(uri));
        }

        private void ContactEmail_Tapped(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Job.ContactEmail)) return;
            string uri = $"mailto:{Job.ContactEmail}";
            Device.OpenUri(new Uri(uri));
        }
    }
}
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

        public string BackgroundColorString => this.BackgroundColor.ToString();

        public JobOfferDetails(JobModel job)
		{
			InitializeComponent();
            Job = job;
            this.BindingContext = this;
		}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SSWS.Mobile
{
    public class TodoItem
    {
        public string Name { get; set; }
        public bool Done { get; set; }
    }

    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var listView = new ListView
            {
                RowHeight = 40
            };


            listView.ItemsSource = new TodoItem[] {
    new TodoItem { Name = "Buy pears" },
    new TodoItem { Name = "Buy oranges", Done=true} ,
    new TodoItem { Name = "Buy mangos" },
    new TodoItem { Name = "Buy apples", Done=true },
    new TodoItem { Name = "Buy bananas", Done=true }
};

            listView.ItemTemplate = new DataTemplate(typeof(TextCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");

            listView.ItemSelected += async (sender, e) => {
                var todoItem = (TodoItem)e.SelectedItem;
                //var todoPage = new TodoItemPage(todoItem); // so the new page shows correct data
                //await Navigation.PushAsync(todoPage);
            };
                
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { listView }
            };
        }


    }
}

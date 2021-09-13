using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarineTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        protected override void OnAppearing()
        {
            showImage(App.Current.Properties["id"], App.Current.Properties["name"]);
            base.OnAppearing();
        }
        public Page2()
        {
            InitializeComponent();
        }
        public async void showImage(object id, object name)
        {
            Label label = new Label
            {
                Text = name.ToString(),
                FontSize = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(label);
            if (id.ToString() != "")
            {
                Image image = new Image();
                image.HeightRequest = 500;
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://demo.macroscop.com/mobile?channelid=" + id + "&oneframeonly=true&login=root&password="),
                    Method = HttpMethod.Get,
                };
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    string[] content = str.Split(new[] { "\r\n\r\n" }, StringSplitOptions.None);
                    byte[] bytes = Encoding.ASCII.GetBytes(content[1]);
                 //   string base64 = Convert.ToBase64String(bytes);
                    MemoryStream stream = new MemoryStream(bytes);
                    image.Source = ImageSource.FromStream(() => stream);
                }
                stackLayout.Children.Add(image);
            }
            Content = stackLayout;
        }
    }
}
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarineTask
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public class Cameras
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sound { get; set; }
            public string parent { get; set; }

            public Cameras() { }
            public Cameras(string id, string name, string sound, string parent)
            {
                this.id = id;
                this.name = name;
                this.sound = sound;
                this.parent = parent;
            }
        }

        public ObservableCollection<Cameras> camera {get;set;}
        async void Connection()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://demo.macroscop.com/configex?login=root&password="),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Accept", "application/xml");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                camera = new ObservableCollection<Cameras>();
                HttpContent responseContent = response.Content;
                var xmlstring = await responseContent.ReadAsStringAsync();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlstring);
                XmlElement xmlRoot = xml.DocumentElement;
                XmlNode Channels = xmlRoot.SelectSingleNode("Channels");
                XmlNode RootSecurityObject = xmlRoot.SelectSingleNode("RootSecurityObject");
                XmlNode securityObject = RootSecurityObject.SelectSingleNode("ChildSecurityObjects");
                foreach (XmlNode child in Channels.ChildNodes)
                {
                    string parent = "";
                    foreach (XmlNode securityChild in securityObject.ChildNodes)
                    {
                        bool goAway = false;
                        XmlNode ChildChannels = securityChild.SelectSingleNode("ChildChannels");
                        foreach (XmlNode id in ChildChannels)
                        {
                            if (id.InnerText == child.Attributes.GetNamedItem("Id").Value)
                            {
                                parent = securityChild.Attributes.GetNamedItem("Name").Value;
                                goAway = true;
                                break;
                            }
                        }
                        if (goAway) break;
                    }
                    Cameras cam = new Cameras(child.Attributes.GetNamedItem("Id").Value,child.Attributes.GetNamedItem("Name").Value,child.Attributes.GetNamedItem("IsSoundOn").Value,parent );
                    camera.Add(cam);
                }
                camerasList.ItemsSource = camera;
            }
        }
        protected override void OnAppearing()
        {
            Connection();
            base.OnAppearing();
        }
        public Page1()
        {
            InitializeComponent();
        }

        private void camerasList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            App.Current.Properties["id"] = camera[e.SelectedItemIndex].id;
            App.Current.Properties["name"] = camera[e.SelectedItemIndex].name;
        }
    }
}
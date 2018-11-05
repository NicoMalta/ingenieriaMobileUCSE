using Newtonsoft.Json;
using SarasApp.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SarasApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }



        public async void OpenContenido()
        {
            Usuario user = new Usuario()
            {
                username = EntUsername.Text,
                password = EntPassword.Text,
            };

            var json = JsonConvert.SerializeObject(user);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();

            var result = await client.PostAsync("http://sarasa-ucse.herokuapp.com/ApiToken_auth/", content);
            
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content2 = await result.Content.ReadAsStringAsync();

                var resultoJson = JsonConvert.DeserializeObject<Token>(content2);
                string accessToken = resultoJson.token;
                Application.Current.MainPage = new NavigationPage(new MisGrupos(accessToken));

               

               
            }
            else
            {
                await DisplayAlert("Error", "Contraseña o Usuario incorrectos" , "Volver a intentar");
            }
        }


    }
}

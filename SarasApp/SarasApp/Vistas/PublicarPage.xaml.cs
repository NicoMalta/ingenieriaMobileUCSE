using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net;
using System.Net.Http.Headers;

namespace SarasApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PublicarPage : ContentPage
	{
        public string token { get; set; }
        public int? idGrupo { get; set; }
        public PublicarPage (string tokenUser, int? idGrupo)
		{
			InitializeComponent ();
            this.token = tokenUser;
            this.idGrupo = idGrupo;
		}


        private async void POST()
        {

            var headerToken = new AuthenticationHeaderValue("Token", this.token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = headerToken;

            var response = await client.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/tokens/");
            var tokens = JsonConvert.DeserializeObject<List<UserToken>>(response);
            var userFiltrado = tokens.Where(x => x.key == this.token).Single();


            Publicacion publicaciones = new Publicacion()
            {
                Estado = 1,
                idUserPublico = userFiltrado.user,
                Titulo = EntTitulo.Text,
                Contenido = EntContenido.Text,
                idGrupoPu = this.idGrupo
                
            };

            var json = JsonConvert.SerializeObject(publicaciones);


            var content = new StringContent(json, Encoding.UTF8, "application/json");
           


            var result = await client.PostAsync("http://sarasa-ucse.herokuapp.com/api_v1/publicacion/", content);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Publicar", "Tu publicacion ha sido creada con Exito!", "Seguir");
                Application.Current.MainPage = new NavigationPage(new Contenido(this.token, this.idGrupo));
            }
            else
            {
                await DisplayAlert("Publicar", "No pudiste publicar", "Volver a intentar");

            }
        }

        void Volver_Publicaciones()
        {
            Application.Current.MainPage = new NavigationPage(new Contenido(this.token, this.idGrupo));
        }

    }
}
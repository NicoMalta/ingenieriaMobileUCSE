using Newtonsoft.Json;
using SarasApp.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SarasApp.Vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ComentarPage : ContentPage
	{
        public string token { get; set; }
        Publicacion publicacion { get; set; }
        public ComentarPage (string Token, Publicacion publicacion )
		{
			InitializeComponent ();
            this.token = Token;
            this.publicacion = publicacion;
		}

        private async void POST()
        {

            var headerToken = new AuthenticationHeaderValue("Token", this.token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = headerToken;

            var response = await client.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/tokens/");
            var tokens = JsonConvert.DeserializeObject<List<UserToken>>(response);
            var userFiltrado = tokens.Where(x => x.key == this.token).Single();


            Comentario comentario = new Comentario()
            {
                idPublicacionC = this.publicacion.idPublicacion,
                idUserComento = userFiltrado.user,
                ContenidoComentario = EntContenido.Text
            };

            var json = JsonConvert.SerializeObject(comentario);


            var content = new StringContent(json, Encoding.UTF8, "application/json");



            var result = await client.PostAsync("http://sarasa-ucse.herokuapp.com/api_v1/comentarios/", content);
            if (result.StatusCode == HttpStatusCode.Created)
            {
                Application.Current.MainPage = new NavigationPage(new PublicacionView(this.token, this.publicacion.idPublicacion));
            }
            else
            {
                await DisplayAlert("Error", "No pudiste Comentar", "Volver a intentar");

            }
        }

        void Volver_Publicacion()
        {
            Application.Current.MainPage = new NavigationPage(new PublicacionView(this.token, this.publicacion.idPublicacion));
        }
    }
}
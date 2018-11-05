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
	public partial class PublicacionView : ContentPage
	{
        public int idPublicacion { get; set; }
        public int? idGrupo { get; set; }
        public string token { get; set; }
        Publicacion publicacion { get; set; }
        public PublicacionView (string Token, int id)
		{
			InitializeComponent ();
            this.idPublicacion = id;
            this.token = Token;

            MostrarPublicacion();
		}

        public async void MostrarPublicacion()
        {
            var headerToken = new AuthenticationHeaderValue("Token", this.token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = headerToken;

            var responsePublicaciones = await client.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/publicacion/");
            var Publicacions = JsonConvert.DeserializeObject<List<Publicacion>>(responsePublicaciones).ToList();
            this.publicacion = Publicacions.Where(x => x.idPublicacion == this.idPublicacion).Single();
            this.idGrupo = publicacion.idGrupoPu;
            VTitulo.Text = publicacion.Titulo;
            VidUserPublico.Text = Convert.ToString(publicacion.idUserPublico);
            VContenido.Html = publicacion.Contenido;

            var responseComentarios = await client.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/comentarios/");
            var Comentarios = JsonConvert.DeserializeObject<List<Comentario>>(responseComentarios).ToList();
            var ListaComentarios = Comentarios.Where(x => x.idPublicacionC == this.idPublicacion);

            ComentariosListView.ItemsSource = ListaComentarios;
            
        }

        public void Publicacion_Volver()
        {
            Application.Current.MainPage = new NavigationPage(new Contenido(this.token,this.idGrupo));
        }

        public void Comentar_Publicacion()
        {
            Application.Current.MainPage = new NavigationPage(new ComentarPage(this.token, this.publicacion));
        }

        async public void Destacar_Publicacion()
        {
            var headerToken = new AuthenticationHeaderValue("Token", this.token);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = headerToken;


            Publicacion publicaciones = new Publicacion()
            {
                Estado = 1,
                Destacar = 2,
                idPublicacion = this.publicacion.idPublicacion,
                idUserPublico = this.publicacion.idUserPublico,
                Titulo = this.publicacion.Titulo,
                Contenido = this.publicacion.Contenido,
                idGrupoPu = this.publicacion.idGrupoPu

            };

            var json = JsonConvert.SerializeObject(publicaciones);


            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PutAsync(string.Concat("http://sarasa-ucse.herokuapp.com/api_v1/publicacion/", publicacion.idPublicacion,"/"), content);

            if (result.IsSuccessStatusCode)
            {
                await DisplayAlert("Destacar", "Publicacion Destacada con Exito", "Seguir");
            }
            else
            {
                await DisplayAlert("Destacar", "No pudiste destacar esta publicacion", "Volver a intentar");

            }
        }

    }
}
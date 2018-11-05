using Newtonsoft.Json;
using SarasApp.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SarasApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contenido : ContentPage
    {
        public string token { get; set; }
        public int? idGrupo { get; set; }
        public Contenido(string tokenUser, int? id)
        {
            InitializeComponent();
            this.token = tokenUser;
            this.idGrupo = id;
            ObtenerPublicaciones();

        }


        private async void ObtenerPublicaciones()
        {

            var header = new AuthenticationHeaderValue("Token", token);
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = header;
            var response = await cliente.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/publicacion/");
            var Publicacions = JsonConvert.DeserializeObject<List<Publicacion>>(response).ToList();
            var publicaciones = Publicacions.Where(x => x.idGrupoPu == this.idGrupo).ToList();
            var ListaDestacados = new List<Publicacion>();
            var ListaPublicaciones = new List<Publicacion>();
            foreach (var item in publicaciones)
            {
                if (item.Destacar == 2)
                {
                    ListaDestacados.Add(item);
                }
            }
            
            foreach (var item in publicaciones)
            {
                if (item.Destacar == 1)
                {

                    ListaPublicaciones.Add(item);
                }
            }

            DestacadosListView.ItemsSource = ListaDestacados;
            PublicacionesListView.ItemsSource = ListaPublicaciones;

            PublicacionesListView.ItemTapped += PublicacionesListView_TappedAsync;
            DestacadosListView.ItemTapped += DestacadosListView_TappedAsync;


        }

        void DestacadosListView_TappedAsync(object sender, ItemTappedEventArgs e)
        {
            var details = e.Item as Publicacion;
            Application.Current.MainPage = new NavigationPage(new PublicacionView(this.token, details.idPublicacion));
        }

        void PublicacionesListView_TappedAsync(object sender, ItemTappedEventArgs e)
        { 
            var details = e.Item as Publicacion;
            Application.Current.MainPage = new NavigationPage(new PublicacionView(this.token,details.idPublicacion));
        }

        public Command ClickCommand
        {
            get { return new Command(() => MetodoDestacar()); }
        }

        void MetodoDestacar()
        {

        }

        void Publicaciones_Volver()
        {
            Application.Current.MainPage = new NavigationPage(new MisGrupos(this.token));
        }

        void Crear_Publicacion()
        {
            Application.Current.MainPage = new NavigationPage(new PublicarPage(this.token, this.idGrupo));
        }

    }



}
using Newtonsoft.Json;
using SarasApp.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SarasApp.Vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MisGrupos : ContentPage
	{
        public string token { get; set; }
        public MisGrupos (string tokenUser)
		{
			InitializeComponent ();
            this.token = tokenUser;
            ListarMisGrupos();
        }

        public async void ListarMisGrupos()
        {

            var header = new AuthenticationHeaderValue("Token", token);
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = header;
            var response = await cliente.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/grupos/");
            var Grups = JsonConvert.DeserializeObject<List<Grupo>>(response);

            var responseUser = await cliente.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/tokens/");
            var tokens = JsonConvert.DeserializeObject<List<UserToken>>(responseUser);
            var userFiltrado = tokens.Where(x => x.key == this.token).Single();

            var responseGrups = await cliente.GetStringAsync("http://sarasa-ucse.herokuapp.com/api_v1/usergrupos/");
            var usergrupos = JsonConvert.DeserializeObject<List<UserGrupos>>(responseGrups);
            var grupos = usergrupos.Where(x => x.idUser == userFiltrado.user).ToList();

            List<Grupo> ListaGrupos = new List<Grupo>();
            foreach (var item in grupos)
            {
                foreach (var item2 in Grups)
                {
                    if (item.idGrupoUsuario == item2.idGrupo)
                    {
                        ListaGrupos.Add(item2);
                    }
                }
            }
            MisGruposListView.ItemsSource = ListaGrupos;

            MisGruposListView.ItemTapped += GruposListView_TappedAsync;
            
        }

        void GruposListView_TappedAsync(object sender, ItemTappedEventArgs e )
        {
            var details = e.Item as Grupo;
            Application.Current.MainPage = new NavigationPage(new Contenido(this.token, details.idGrupo));
        }

    }
}
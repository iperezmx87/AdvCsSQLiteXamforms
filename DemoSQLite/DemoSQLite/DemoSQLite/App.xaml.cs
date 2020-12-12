using DemoSQLite.Administrador;
using Xamarin.Forms;

namespace DemoSQLite
{
    public partial class App : Application
    {
        private static AdminBaseDatos _adminBd;
        public static AdminBaseDatos adminBd
        {
            get
            {
                if (_adminBd == null)
                {
                    _adminBd = new AdminBaseDatos();
                }

                return _adminBd;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

using DemoSQLite.Modelo;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoSQLite
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaDetalleArticulo : ContentPage
    {
        private bool _esNuevo;
        private ArticuloDespensa _articulo;

        public VistaDetalleArticulo()
        {
            InitializeComponent();
        }

        public VistaDetalleArticulo(bool esNuevo, ArticuloDespensa articulo)
        {
            InitializeComponent();

            if (esNuevo)
            {
                _articulo = new ArticuloDespensa();

                // cargar formulario vacio
                txtCant.Text = "";
                txtDesc.Text = "";
                txtPr.Text = "";
                txtTotal.Text = "";
            }
            else
            {
                _articulo = articulo;

                // cargar formulario con datos
                txtCant.Text = articulo.Cantidad.ToString();
                txtDesc.Text = articulo.Descripcion;
                txtPr.Text = articulo.PrecioUnitario.ToString();
                txtTotal.Text = articulo.Total.ToString();
            }

            _esNuevo = esNuevo;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (_esNuevo)
            {
                // calcular precio total = precio unitario * cantidad
                double total = double.Parse(txtCant.Text) * double.Parse(txtPr.Text);
                txtTotal.Text = total.ToString();

                // guardar elemento
                _articulo.Cantidad = int.Parse(txtCant.Text);
                _articulo.Descripcion = txtDesc.Text;

                // obtener el siguiente id de articulo para agregarle 1
                int ultimoId = await App.adminBd.ObtenerUltimoId();
                _articulo.Id = ultimoId + 1;
                _articulo.PrecioUnitario = double.Parse(txtPr.Text);
                _articulo.Total = total;

                await App.adminBd.InsertarElemento(_articulo);
            }
            else
            {
                // actualizar
                double total = double.Parse(txtCant.Text) * double.Parse(txtPr.Text);
                txtTotal.Text = total.ToString();

                // guardar elemento
                _articulo.Cantidad = int.Parse(txtCant.Text);
                _articulo.Descripcion = txtDesc.Text;
                _articulo.PrecioUnitario = double.Parse(txtPr.Text);
                _articulo.Total = total;

                await App.adminBd.ActualizarArticulo(_articulo);
            }

            await DisplayAlert("Mi despensa", "Articulo guardado", "Ok");

            await Navigation.PopAsync();
        }
    }
}
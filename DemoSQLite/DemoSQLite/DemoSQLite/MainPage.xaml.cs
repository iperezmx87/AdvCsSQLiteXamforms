using DemoSQLite.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DemoSQLite
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            CargarListaDespensa();
        }

        public async void CargarListaDespensa()
        {
            List<ArticuloDespensa> despensa = await App.adminBd.ObtenerListaDespensa();
            cllDespensa.ItemsSource = despensa;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // nuevo elemento
            Navigation.PushAsync(new VistaDetalleArticulo(true, null));
        }

        private void cllDespensa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // actualizar articulo
            ArticuloDespensa art = e.CurrentSelection.First() as ArticuloDespensa;
            Navigation.PushAsync(new VistaDetalleArticulo(false, art));
        }

        private async void SwipeItem_Clicked(object sender, EventArgs e)
        {
            SwipeItem itemEliminar = sender as SwipeItem;

            ArticuloDespensa artEliminar = itemEliminar.CommandParameter as ArticuloDespensa;

            await App.adminBd.EliminarArticulo(artEliminar.Id);

            await DisplayAlert("Mi despensa", "Articulo eliminado", "ok");

            CargarListaDespensa();
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using futboleando.Service;

namespace futboleando.Pages.Contacto;

public partial class ContactoPage : ContentPage
{
    private const string SitioWeb = "https://futboleando.com.mx";
    private const string Correo = "admin@futboleando.com.mx";
    private readonly AvisoFutboleandoService avisoFutboleandoService;

    private string telefono = "Cargando...";
    public string Telefono
    {
        get => telefono;
        set
        {
            telefono = value;
            OnPropertyChanged(nameof(Telefono));
        }
    }

    public ContactoPage(AvisoFutboleandoService avisoFutboleandoService)
    {
        InitializeComponent();
        this.avisoFutboleandoService = avisoFutboleandoService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarTelefonoAsync();
    }

    private async Task CargarTelefonoAsync()
    {
        var telefonoDb = await avisoFutboleandoService.ObtenerTelefonoAsync();
        Telefono = string.IsNullOrWhiteSpace(telefonoDb) ? "No disponible" : telefonoDb;
    }

    private async void OnWebClicked(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync(new Uri(SitioWeb));
    }

    private async void OnEmailClicked(object sender, EventArgs e)
    {
        var subject = Uri.EscapeDataString("Información sobre administración de torneos");
        var body = Uri.EscapeDataString("Hola, me interesa administrar un torneo con Futboleando.");
        await Launcher.Default.OpenAsync(new Uri($"mailto:{Correo}?subject={subject}&body={body}"));
    }

    private async void OnCallClicked(object sender, EventArgs e)
    {
        var telefonoLlamada = LimpiarTelefono(Telefono);
        if (string.IsNullOrWhiteSpace(telefonoLlamada) || Telefono == "No disponible")
        {
            await DisplayAlert("Contacto", "No hay un número disponible para llamar.", "OK");
            return;
        }

        await Launcher.Default.OpenAsync(new Uri($"tel:{telefonoLlamada}"));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private static string LimpiarTelefono(string telefonoActual)
    {
        if (string.IsNullOrWhiteSpace(telefonoActual))
        {
            return string.Empty;
        }

        var caracteres = telefonoActual.Where(char.IsDigit).ToArray();
        return new string(caracteres);
    }
}
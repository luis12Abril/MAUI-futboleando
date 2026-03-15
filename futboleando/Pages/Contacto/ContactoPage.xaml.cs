using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using futboleando.Service;

namespace futboleando.Pages.Contacto;

public partial class ContactoPage : ContentPage
{
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

    private async void OnWhatsAppClicked(object sender, EventArgs e)
    {
        var telefonoWhatsApp = LimpiarTelefono(Telefono);
        if (string.IsNullOrWhiteSpace(telefonoWhatsApp) || Telefono == "No disponible")
        {
            await DisplayAlert("Contacto", "No hay un número disponible para WhatsApp.", "OK");
            return;
        }

        var mensaje = Uri.EscapeDataString("Hola, me interesa administrar un torneo con Futboleando.");
        await Launcher.Default.OpenAsync(new Uri($"https://wa.me/{telefonoWhatsApp}?text={mensaje}"));
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
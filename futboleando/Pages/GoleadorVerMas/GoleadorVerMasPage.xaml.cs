namespace futboleando.Pages.GoleadorVerMas;

public partial class GoleadorVerMasPage : ContentPage
{
    private int idJugador;

    public GoleadorVerMasPage(int _idJugador)
    {
        InitializeComponent();
        idJugador = _idJugador;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Mostrar el ID del jugador (temporal hasta implementar los detalles)
        lblIdJugador.Text = $"ID del Jugador: {idJugador}";
    }
}

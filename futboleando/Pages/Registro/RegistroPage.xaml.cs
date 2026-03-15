using futboleandoEntities.Login;
using futboleando.Service;

namespace futboleando.Pages.Registro;

public partial class RegistroPage : ContentPage
{
    public RegistroRequestCLS oRegistroRequestCLS { get; set; }
    private LoginService loginService;

    public RegistroPage(LoginService _loginService)
    {
        InitializeComponent();
        oRegistroRequestCLS = new RegistroRequestCLS();
        loginService = _loginService;
        BindingContext = this;
    }

    private async void btnRegistrarse_Clicked(object sender, EventArgs e)
    {
        // ? Obtener referencias a los controles
        var txtConfirmPassword = this.FindByName<Entry>("txtConfirmPassword");
        var btnRegistrar = this.FindByName<Button>("btnRegistrar");

        // ? Validar campos vacíos
        if (string.IsNullOrWhiteSpace(oRegistroRequestCLS.nombreusuario) || 
            string.IsNullOrWhiteSpace(oRegistroRequestCLS.contra) ||
            string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
        {
            await DisplayAlert("Error", "Por favor complete todos los campos", "OK");
            return;
        }

        // ? Validar que las contraseñas coincidan
        if (oRegistroRequestCLS.contra != txtConfirmPassword.Text)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        // ? Deshabilitar botón mientras se procesa
        btnRegistrar.IsEnabled = false;
        btnRegistrar.Text = "Registrando...";

        // ? Llamar al servicio de registro
        var registroResponse = await loginService.Registrar(oRegistroRequestCLS);

        if (registroResponse != null && registroResponse.exito == true)
        {
            // ? Mostrar mensaje de bienvenida
            await DisplayAlert("¡Registro Exitoso!", 
                $"¡Bienvenido a Futboleando, {registroResponse.nombre}!\n\nTu cuenta ha sido creada correctamente.", 
                "Continuar");

            // ? NUEVO: Limpiar preferencias de torneo para usuario nuevo
            LimpiarPreferenciasTorneo();

            // ? Iniciar sesión automáticamente
            await IniciarSesionAutomaticamente(oRegistroRequestCLS.nombreusuario, oRegistroRequestCLS.contra);
        }
        else
        {
            // ? Mostrar mensaje de error
            string mensajeError = registroResponse?.mensaje ?? "Error de conexión con el servidor";
            await DisplayAlert("Error de Registro", mensajeError, "OK");
            
            // ? Rehabilitar botón
            btnRegistrar.IsEnabled = true;
            btnRegistrar.Text = "REGISTRARSE";
        }
    }

    private void LimpiarPreferenciasTorneo()
    {
        // ? Limpiar todas las preferencias de torneo previas
        Preferences.Remove("UltimoEstado");
        Preferences.Remove("UltimoMunicipio");
        Preferences.Remove("UltimaLiga");
        Preferences.Remove("UltimoTorneo");
        Preferences.Remove("NombreEstado");
        Preferences.Remove("NombreMunicipio");
        Preferences.Remove("NombreLiga");
        Preferences.Remove("NombreTorneo");
    }

    private async Task IniciarSesionAutomaticamente(string usuario, string contraseña)
    {
        try
        {
            // ? Crear objeto de login
            var loginRequest = new LoginCLS
            {
                nombreusuario = usuario,
                contra = contraseña
            };

            // ? Intentar hacer login
            var loginResponse = await loginService.login(loginRequest);

            if (loginResponse != null && loginResponse.exito == true)
            {
                // ? Guardar sesión con datos del usuario
                Preferences.Set("usuario", "ok");
                Preferences.Set("IdUsuario", loginResponse.idusuario);
                Preferences.Set("NombreUsuario", loginResponse.nombre);
                Preferences.Set("IdTipoUsuario", loginResponse.idtipousuario);
                Preferences.Set("NombreTipoUsuario", loginResponse.nombretipousuario);

                // ? Obtener servicios necesarios
                var estadoService = MauiProgram.ServiceProvider.GetService<EstadoService>();
                var municipioService = MauiProgram.ServiceProvider.GetService<MunicipioService>();
                var ligaService = MauiProgram.ServiceProvider.GetService<LigaService>();
                var torneoService = MauiProgram.ServiceProvider.GetService<TorneoService>();
                var menuService = MauiProgram.ServiceProvider.GetService<MenuService>();
                var jugadorService = MauiProgram.ServiceProvider.GetService<JugadorService>();
                var ciudadService = MauiProgram.ServiceProvider.GetService<CiudadService>();
                var colaboradorService = MauiProgram.ServiceProvider.GetService<ColaboradorService>();
                var equipoService = MauiProgram.ServiceProvider.GetService<EquipoService>();
                var comunicadoService = MauiProgram.ServiceProvider.GetService<ComunicadoService>();

                // ? Ir directamente al selector de torneo (con pickers vacíos)
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    App.Current.MainPage = new NavigationPage(
                        new TorneoSelectorPage(estadoService, municipioService, ligaService, torneoService,
                            menuService, loginService, jugadorService, ciudadService, colaboradorService,
                            equipoService, comunicadoService)
                    );
                });
            }
            else
            {
                // ? Si el login automático falla (raro), cerrar modal y volver al login
                await DisplayAlert("Aviso", 
                    "Tu cuenta fue creada exitosamente. Por favor inicia sesión.", 
                    "OK");
                await Navigation.PopModalAsync();
            }
        }
        catch (Exception ex)
        {
            // ? Si hay error, cerrar modal y volver al login
            await DisplayAlert("Aviso", 
                "Tu cuenta fue creada exitosamente. Por favor inicia sesión.", 
                "OK");
            await Navigation.PopModalAsync();
        }
    }

    private async void OnVolverLoginTapped(object sender, EventArgs e)
    {
        // ? Cerrar modal y volver al login
        await Navigation.PopModalAsync();
    }
}
using System;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tally.ViewModels;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Threading.Tasks;

namespace Tally.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty]
    private string _status = " ";
    
    [ObservableProperty]
    private string _usernameInput;
    
    [ObservableProperty]
    private string _passwordInput;

    public LoginViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    // ZMĚNA 1: Metoda musí být asynchronní (async Task)
    [RelayCommand]
    private async Task Prihlasit()
    {
        System.Diagnostics.Debug.WriteLine($"Zkouším přihlásit: {UsernameInput} / {PasswordInput}");
        Console.WriteLine($"Zkouším přihlásit: {UsernameInput} / {PasswordInput}");
        
        Status = "Přihlašuji..."; // Zpětná vazba pro uživatele (používáme velké písmeno!)

        // ZMĚNA 2: Přidáno slůvko await, aby tlačítko počkalo na výsledek
        await ZkusitPrihlaseni(UsernameInput, PasswordInput);
    }

    public async Task ZkusitPrihlaseni(string zadaneJmeno, string zadaneHeslo)
    {
        var dataProServer = new LoginData 
        { 
            Jmeno = zadaneJmeno, 
            Heslo = zadaneHeslo 
        };

        using var klient = new HttpClient();

        try
        {
            // Zkontroluj, jestli ti backend běží opravdu na portu 5211
            string adresa = "http://localhost:5211/prihlaseni"; 

            var odpoved = await klient.PostAsJsonAsync(adresa, dataProServer);

            if (odpoved.IsSuccessStatusCode)
            {
                string zpravaZeServeru = await odpoved.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("ÚSPĚCH: " + zpravaZeServeru);
                
                // ZMĚNA 3: Všude upraveno _status na Status
                Status = "Přihlášení bylo úspěšné!"; 
                _mainViewModel.PrepnoutNaAplikaci(); // Přepni na hlavní aplikaci
            }
            else if (odpoved.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                System.Diagnostics.Debug.WriteLine("CHYBA: Špatné jméno nebo heslo.");
                Status = "Špatné jméno nebo heslo.";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Něco se pokazilo: {odpoved.StatusCode}");
                Status = $"Něco se pokazilo: {odpoved.StatusCode}";
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("CHYBA: Server neodpovídá. Podrobnosti: " + ex.Message);
            Status = "Server neodpovídá.";
        }
    }
    
    public class LoginData
    {
        public string Jmeno { get; set; } = string.Empty;
        public string Heslo { get; set; } = string.Empty;
    }
}
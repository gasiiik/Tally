using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tally.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainViewModel()
    {
        // Při spuštění aplikace chceme ukázat Login.
        // "this" znamená, že předáváme tomuto loginu přístup sem, aby nás mohl přepnout.
        CurrentPage = new LoginViewModel(this);
    }
   
   public void PrepnoutNaAplikaci()
    {
        // Tady přepneš na ViewModel tvé hlavní aplikace.
        // Zde si vytvoříš nějaký svůj "DashboardViewModel" nebo něco podobného,
        // co nahradí přihlašovací obrazovku.
        // Prozatím tam můžeme dát null, nebo pokud ho už máš:
        CurrentPage = new DashboardViewModel();
    }
    


}

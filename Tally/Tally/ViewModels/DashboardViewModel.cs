using System;

// 1. Musíme mu říct, kde najde ViewModelBase
using Tally.ViewModels; 

// 2. Patří to do složky ViewModels
namespace Tally.ViewModels; 

// 3. Přidáme dědičnost z ViewModelBase (a 'partial', abys tam pak mohl dávat [ObservableProperty])
public partial class DashboardViewModel : ViewModelBase 
{

}
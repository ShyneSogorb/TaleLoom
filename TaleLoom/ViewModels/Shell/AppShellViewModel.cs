using System.Windows.Input;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs.Home;
using TaleLoom.ViewModels.Prefabs.Prefabs;
using TaleLoom.Views.Home;

namespace TaleLoom.ViewModels.Prefabs.Shell;

public class AppShellViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly PrefabRepository _prefabRepository;

    public INavigationService Navigation => _navigationService;

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigatePrefabsCommand { get; }
    
    public AppShellViewModel(
        INavigationService navigationService,
        PrefabRepository prefabRepository)
    {
        _navigationService = navigationService;
        _prefabRepository = prefabRepository;

        NavigateHomeCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new HomeViewModel()));

        NavigatePrefabsCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new PrefabListViewModel(
                    _navigationService,
                    _prefabRepository)));
    }
}
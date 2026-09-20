using System.Windows.Input;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Entities;
using TaleLoom.ViewModels.Home;
using TaleLoom.ViewModels.Prefabs;

namespace TaleLoom.ViewModels.Shell;

public class AppShellViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly PrefabRepository _prefabRepository;
    private readonly EntityRepository _entityRepository;

    public INavigationService Navigation => _navigationService;

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigatePrefabsCommand { get; }
    
    public ICommand NavigateEntityCommand { get; }
    
    public AppShellViewModel(
        INavigationService navigationService,
        PrefabRepository prefabRepository,
        EntityRepository entityRepository)
    {
        _navigationService = navigationService;
        _prefabRepository = prefabRepository;
        _entityRepository = entityRepository;

        NavigateHomeCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new HomeViewModel()));

        NavigatePrefabsCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new PrefabListViewModel(
                    _navigationService,
                    _prefabRepository)));
        
        NavigateEntityCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new EntitiesListViewModel(
                    _navigationService,
                    _entityRepository,
                    _prefabRepository)));
        
    }
}
using System.Windows.Input;
using TaleLoom.Infrastructure.Files;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Entities;
using TaleLoom.ViewModels.Home;
using TaleLoom.ViewModels.Images;
using TaleLoom.ViewModels.Prefabs;
using TaleLoom.ViewModels.Timeline;

namespace TaleLoom.ViewModels.Shell;

public class AppShellViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly TaleLoomRepository _taleLoomRepository;
    private readonly ImageLibrary _imageLibrary;

    public INavigationService Navigation => _navigationService;

    public ICommand NavigateHomeCommand { get; }
    public ICommand NavigatePrefabsCommand { get; }
    
    public ICommand NavigateEntityCommand { get; }
    public ICommand NavigateGalleryCommand { get; }
    public ICommand NavigateTimelineCommand { get; }
    
    
    public AppShellViewModel(
        INavigationService navigationService,
        TaleLoomRepository taleLoomRepository)
    {
        _navigationService = navigationService;
        _taleLoomRepository = taleLoomRepository;

        _imageLibrary = new ImageLibrary();

        NavigateHomeCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new HomeViewModel()));

        NavigatePrefabsCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new PrefabListViewModel(
                    _navigationService,
                    _taleLoomRepository)));
        
        NavigateEntityCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new EntitiesListViewModel(
                    _navigationService,
                    _taleLoomRepository)));

        NavigateGalleryCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new ImageGalleryViewModel(_imageLibrary))
        );

        NavigateTimelineCommand = new RelayCommand(
            _ => _navigationService.Navigate(
                new TimelineViewModel())
        );

    }
}
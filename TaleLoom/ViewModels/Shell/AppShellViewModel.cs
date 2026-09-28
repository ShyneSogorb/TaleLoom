using System.Collections.Generic;
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
    
    public ICommand NavigateToPreviousCommand { get; }
    public ICommand NavigateToNextCommand { get; }

    private bool _canGoBack;
    private bool _canGoForward;
    
    public bool CanGoBack
    {
        get => _canGoBack;
        set 
        {
            if (_canGoBack == value) return;

            _canGoBack = value;
            OnPropertyChanged();
        }
    }
    
    public bool CanGoForward
    {
        get => _canGoForward;
        set 
        {
            if (_canGoForward == value) return;

            _canGoForward = value;
            OnPropertyChanged();
        }
    }

    private void GoTo(object view)
    {
        _navigationService.Navigate(view);
        CanGoForward = _navigationService.HasNextPage;
        CanGoBack = _navigationService.HasPreviousPage;
    }
    
    public AppShellViewModel(
        INavigationService navigationService,
        TaleLoomRepository taleLoomRepository)
    {
        _navigationService = navigationService;
        _taleLoomRepository = taleLoomRepository;

        _imageLibrary = new ImageLibrary();

        NavigateHomeCommand = new RelayCommand(
            _ => GoTo(new HomeViewModel()));

        NavigatePrefabsCommand = new RelayCommand(
            _ => GoTo(
                new PrefabListViewModel(
                    _navigationService,
                    _taleLoomRepository)));
        
        NavigateEntityCommand = new RelayCommand(
            _ => GoTo(
                new EntitiesListViewModel(
                    _navigationService,
                    _taleLoomRepository)));

        NavigateGalleryCommand = new RelayCommand(
            _ => GoTo(
                new ImageGalleryViewModel(_imageLibrary))
        );

        NavigateTimelineCommand = new RelayCommand(
            _ => GoTo(
                new TimelineViewModel())
        );
        
        NavigateToPreviousCommand = new RelayCommand(
            _ =>
            {
                _navigationService.GoToPrevious();
                CanGoForward = _navigationService.HasNextPage;
                CanGoBack = _navigationService.HasPreviousPage;
            }
            
        );

        NavigateToNextCommand = new RelayCommand(_ =>
            {
                _navigationService.GoToNext();
                CanGoForward = _navigationService.HasNextPage;
                CanGoBack = _navigationService.HasPreviousPage;
            }
        );

    }
}
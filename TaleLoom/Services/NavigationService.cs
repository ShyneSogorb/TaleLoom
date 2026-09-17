
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaleLoom.Services;

public class NavigationService : INavigationService, INotifyPropertyChanged
{
    private object? _currentView;

    public object? CurrentView
    {
        get => _currentView;
        private set
        {
            if (_currentView == value)return;

            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void Navigate(object viewModel)
    {
        CurrentView = viewModel;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
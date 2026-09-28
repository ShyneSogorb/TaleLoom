
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaleLoom.Services;

public class NavigationService : INavigationService, INotifyPropertyChanged
{
    private object? _currentView;
    private LinkedListNode<object> _currentNode;
    private LinkedList<object> _viewStack;

    private bool _hasPreviousPage;
    private bool _hasNextPage;

    public bool HasPreviousPage
    {
        get => _hasPreviousPage;
        set
        {
            if (_hasPreviousPage == value)return;

            _hasPreviousPage = value;
            OnPropertyChanged();
        }
    }
    
    public bool HasNextPage
    {
        get => _hasNextPage;
        set
        {
            if (_hasNextPage == value)return;

            _hasNextPage = value;
            OnPropertyChanged();
        }
    }


    public NavigationService()
    {
        _viewStack = new LinkedList<object>();
    }

    public object? CurrentView
    {
        get => _currentView;
        private set
        {
            if (_currentView == value)return;

            
            _currentView = value;
            HasNextPage = _currentNode.Next is not null;
            HasPreviousPage = _currentNode.Previous is not null;
            OnPropertyChanged();
        }
    }
    
    
    public void NavigateInternal()
    {
        CurrentView = _currentNode.Value;
    }

    public void Navigate(object viewModel)
    {
        while (_viewStack.Last != _currentNode)
        {
            _viewStack.RemoveLast();
        }
        _currentNode = _viewStack.AddLast(viewModel);
        NavigateInternal();
    }

    public void GoToPrevious()
    {
        _currentNode = _currentNode.Previous!;
        NavigateInternal();
    }



    public void GoToNext()
    {
        _currentNode = _currentNode.Next!;
        NavigateInternal();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
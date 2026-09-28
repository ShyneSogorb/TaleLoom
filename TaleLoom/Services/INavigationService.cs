namespace TaleLoom.Services;

public interface INavigationService
{
    object? CurrentView { get; }
    void Navigate(object viewModel);
    
    void GoToPrevious();
    bool HasPreviousPage { get; set; }
    
    void GoToNext();
    bool HasNextPage  { get; set; }
}
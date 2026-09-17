namespace TaleLoom.Services;

public interface INavigationService
{
    object? CurrentView { get; }
    void Navigate(object viewModel);
}
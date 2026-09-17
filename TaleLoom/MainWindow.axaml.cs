using Avalonia.Controls;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;
using TaleLoom.ViewModels.Prefabs;
using TaleLoom.ViewModels.Prefabs.Home;
using TaleLoom.ViewModels.Prefabs.Shell;

namespace TaleLoom;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        
        InitializeComponent();

        var navigationService = new NavigationService();
        navigationService.Navigate(new HomeViewModel());

        var database = new SqliteDatabase("TaleLoom.metadata.db");
        var repo = new PrefabRepository(database);
        var initializer = new DatabaseInitializer(database);
        initializer.Initialize();
        DataContext = new AppShellViewModel(navigationService, repo);

    }
}
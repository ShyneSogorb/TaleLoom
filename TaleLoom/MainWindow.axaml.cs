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

        var prefabDatabase = new SqliteDatabase("TaleLoom.metadata.db");
        var prefabRepository = new PrefabRepository(prefabDatabase);
        var prefabInitializer = new PrefabDatabaseInitializer(prefabDatabase);
        prefabInitializer.Initialize();
        prefabInitializer.Populate();
        
        var entityDatabase = new SqliteDatabase("TaleLoom.db");
        var entityRepository = new EntityRepository(entityDatabase);
        var entityInitializer = new EntityDatabaseInitializer(entityDatabase);
        //entityInitializer.Initialize();
        
        
        DataContext = new AppShellViewModel(navigationService, prefabRepository, entityRepository);

    }
}
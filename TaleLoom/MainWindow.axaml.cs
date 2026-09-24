using System;
using System.IO;
using Avalonia.Controls;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Infrastructure.Files;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;

using TaleLoom.ViewModels.Home;
using TaleLoom.ViewModels.Shell;

namespace TaleLoom;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        
        InitializeComponent();

        var navigationService = new NavigationService();
        navigationService.Navigate(new HomeViewModel());

        var prefabDatabase = new SqliteDatabase("TaleLoom.db");
        var prefabRepository = new PrefabRepository(prefabDatabase);
        var prefabInitializer = new PrefabDatabaseInitializer(prefabDatabase);
        prefabInitializer.Initialize();
        var prefabs = prefabInitializer.Populate();
        
        var entityDatabase = new SqliteDatabase("TaleLoom.db");
        var entityRepository = new EntityRepository(entityDatabase);
        var entityInitializer = new EntityDatabaseInitializer(entityDatabase);
        
        entityInitializer.Initialize(prefabs[0]);
        //entityInitializer.Populate(new Entity(prefabs[0], "Ythia"));

        Console.Write("Tale loom location is " + TaleLoomDataDirectory.Root);
        
        DataContext = new AppShellViewModel(navigationService, prefabRepository, entityRepository);

    }
}
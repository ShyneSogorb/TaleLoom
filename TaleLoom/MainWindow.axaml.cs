using System;
using System.IO;
using Avalonia.Controls;
using TaleLoom.Core.Model.Entities;
using TaleLoom.Infrastructure.Files;
using TaleLoom.Infrastructure.Persistence;
using TaleLoom.Services;

using TaleLoom.ViewModels.Home;
using TaleLoom.ViewModels.Shell;
using TaleLoom.ViewModels.Timeline;
using TaleLoom.Views.Timeline;

namespace TaleLoom;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        
        InitializeComponent();

        var navigationService = new NavigationService();
        //navigationService.Navigate(new HomeViewModel());
        navigationService.Navigate(new TimelineViewModel());

        var sqliteDatabase = new SqliteDatabase("TaleLoom.db");
        var repository = new TaleLoomRepository(sqliteDatabase);
        var prefabInitializer = new PrefabDatabaseInitializer(sqliteDatabase);
        prefabInitializer.Initialize();
        var prefabs = prefabInitializer.Populate();
        
        var entityInitializer = new EntityDatabaseInitializer(sqliteDatabase);
        
        entityInitializer.Initialize(prefabs[0]);

        Console.Write("Tale loom location is " + TaleLoomDataDirectory.Root);
        
        DataContext = new AppShellViewModel(navigationService, repository);

    }
}
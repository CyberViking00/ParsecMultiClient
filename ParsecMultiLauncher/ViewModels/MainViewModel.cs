using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ParsecMultiLauncher.Models;

namespace ParsecMultiLauncher.ViewModels;

public sealed class MainViewModel
{
    private readonly InstanceManager _manager = new();

    public ObservableCollection<ParsecInstance> Instances => _manager.Instances;
    public ICommand LaunchCommand { get; }
    public ICommand KillCommand { get; }
    public ICommand KillAllCommand { get; }

    public MainViewModel()
    {
        if (!_manager.MasterEmbedded)
        {
            MessageBox.Show(
                "Embedded master.zip not found.\nRebuild with the master folder present.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        LaunchCommand = new RelayCommand(_ => Launch(), _ => _manager.MasterEmbedded);
        KillCommand = new RelayCommand(async p => await KillAsync(p as ParsecInstance));
        KillAllCommand = new RelayCommand(async _ => await _manager.KillAllAsync(), _ => Instances.Count > 0);
    }

    private void Launch()
    {
        try
        {
            _manager.Launch();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to launch instance:\n{ex.Message}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task KillAsync(ParsecInstance? instance)
    {
        if (instance != null)
            await _manager.KillAsync(instance);
    }

    public void Shutdown() => _manager.KillAllSync();
}

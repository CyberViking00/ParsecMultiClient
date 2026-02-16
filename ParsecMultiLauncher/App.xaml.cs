using System.Windows;
using ParsecMultiLauncher.ViewModels;

namespace ParsecMultiLauncher;

public partial class App : Application
{
    protected override void OnExit(ExitEventArgs e)
    {
        if (MainWindow?.DataContext is MainViewModel vm)
            vm.Shutdown();
        base.OnExit(e);
    }
}

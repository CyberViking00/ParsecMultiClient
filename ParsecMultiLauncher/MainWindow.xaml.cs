using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using ParsecMultiLauncher.ViewModels;

namespace ParsecMultiLauncher;

public partial class MainWindow : Window
{
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(nint hwnd, int attr, ref int value, int size);

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => EnableDarkTitleBar();
        Closing += (_, _) =>
        {
            if (DataContext is MainViewModel vm)
                vm.Shutdown();
        };
    }

    private void EnableDarkTitleBar()
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var value = 1;
        // DWMWA_USE_IMMERSIVE_DARK_MODE = 20
        DwmSetWindowAttribute(hwnd, 20, ref value, sizeof(int));
    }
}

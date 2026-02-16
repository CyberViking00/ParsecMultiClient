using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ParsecMultiLauncher.Models;

public sealed class ParsecInstance : INotifyPropertyChanged
{
    private string _status = "Starting";

    public int Number { get; init; }
    public Process? Process { get; init; }
    public string TempPath { get; init; } = string.Empty;

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public int? Pid => Process?.Id;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

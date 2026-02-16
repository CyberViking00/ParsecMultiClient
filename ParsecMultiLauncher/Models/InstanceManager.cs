using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows;

namespace ParsecMultiLauncher.Models;

public sealed class InstanceManager
{
    private static readonly string[] LockFiles = ["lock", "lock_client"];

    public ObservableCollection<ParsecInstance> Instances { get; } = [];

    public bool MasterEmbedded => GetMasterStream() != null;

    public ParsecInstance Launch()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"Parsec_{Guid.NewGuid():N}");
        ExtractMaster(tempDir);
        DeleteLockFiles(tempDir);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(tempDir, "parsecd.exe"),
                WorkingDirectory = tempDir,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var instance = new ParsecInstance
        {
            Number = GetNextNumber(),
            TempPath = tempDir,
            Process = process,
            Status = "Running"
        };

        process.Exited += (_, _) =>
        {
            CleanupTemp(instance.TempPath);
            Application.Current?.Dispatcher.BeginInvoke(() => Instances.Remove(instance));
        };

        process.Start();
        Instances.Add(instance);
        return instance;
    }

    public async Task KillAsync(ParsecInstance instance)
    {
        instance.Status = "Killing";

        await Task.Run(() =>
        {
            try
            {
                if (instance.Process is { HasExited: false })
                {
                    instance.Process.Kill(entireProcessTree: true);
                    instance.Process.WaitForExit(3000);
                }
            }
            catch { }

            CleanupTemp(instance.TempPath);
        });

        Instances.Remove(instance);
    }

    public async Task KillAllAsync()
    {
        foreach (var instance in Instances.ToList())
            await KillAsync(instance);
    }

    public void KillAllSync()
    {
        foreach (var instance in Instances.ToList())
        {
            try
            {
                if (instance.Process is { HasExited: false })
                {
                    instance.Process.Kill(entireProcessTree: true);
                    instance.Process.WaitForExit(3000);
                }
            }
            catch { }

            CleanupTemp(instance.TempPath);
        }
    }

    private int GetNextNumber()
    {
        var used = new HashSet<int>(Instances.Select(i => i.Number));
        var n = 1;
        while (used.Contains(n)) n++;
        return n;
    }

    private static Stream? GetMasterStream()
        => Assembly.GetExecutingAssembly().GetManifestResourceStream("master.zip");

    private static void ExtractMaster(string destination)
    {
        using var stream = GetMasterStream()
            ?? throw new InvalidOperationException("Embedded master.zip not found.");
        ZipFile.ExtractToDirectory(stream, destination);
    }

    private static void DeleteLockFiles(string directory)
    {
        foreach (var name in LockFiles)
        {
            var path = Path.Combine(directory, name);
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    private static void CleanupTemp(string path)
    {
        try
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive: true);
        }
        catch { }
    }
}

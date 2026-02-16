# Parsec Multi Client

A Lightweight windows application that allows you to run multiple instances of Parsec simultaneously.

## Features

- Launch multiple Parsec instances independently
- Each instance runs with its own configuration
- Simple WPF interface for managing instances
- Automatically packages required Parsec files

## Requirements

- Windows 10/11
- .NET 8.0 Runtime (or SDK for building)
- Visual Studio 2022 or later (for building from source)

## Building

1. Open `ParsecMultiLauncher.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. The executable will be in `ParsecMultiLauncher/bin/Debug/net8.0-windows/` or `ParsecMultiLauncher/bin/Release/net8.0-windows/`

## How It Works

The application embeds the Parsec client files in the `master` folder and extracts them to temporary directories for each instance. Each instance runs independently with its own configuration.

## Project Structure

```
ParsecMultiLauncher/
├── Models/               # Data models and instance management
├── ViewModels/          # MVVM view models
├── Properties/          # Assembly information
├── master/              # Parsec client files (embedded at build time)
├── App.xaml            # Application definition
├── MainWindow.xaml     # Main UI
└── app.ico             # Application icon
```

## License

This project is provided as-is for educational purposes.


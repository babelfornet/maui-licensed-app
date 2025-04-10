# Licensed App

A cross-platform MAUI application with built-in licensing functionality for Android, iOS, and MacCatalyst platforms.

## Overview

This application is built using .NET MAUI (Multi-platform App UI) framework, allowing it to run on multiple platforms from a single codebase. The app includes a licensing system that helps manage software licensing across different platforms.

## Features

- Cross-platform support (Android, iOS, MacCatalyst)
- Integrated licensing system
- Platform-specific implementations for system information

## Requirements

- .NET 9.0 SDK or later
- Visual Studio 2022 or Visual Studio for Mac with MAUI workload installed
- XCode (for iOS/MacCatalyst development)
- Android SDK (for Android development)

## Project Structure

```
MauiLicensedApp/
├── Licensing/                 # Licensing system implementation
│   ├── BabelLicensingExtensions.cs
│   ├── ISystemInformation.cs
│   └── LicensingService.cs
├── Platforms/                 # Platform-specific code
│   ├── Android/               # Android implementation
│   ├── iOS/                   # iOS implementation
│   ├── MacCatalyst/           # MacCatalyst implementation
│   ├── Tizen/                 # Tizen implementation
│   └── Windows/               # Windows implementation
├── Resources/                 # Application resources
│   ├── AppIcon/               # App icons
│   ├── Fonts/                 # Custom fonts
│   ├── Images/                # Images used in the app
│   ├── Raw/                   # Raw resource files
│   ├── Splash/                # Splash screen resources
│   └── Styles/                # XAML styles
├── App.xaml                   # Application definition
├── AppShell.xaml              # App shell for navigation
├── MainPage.xaml              # Main page of the application
└── MauiProgram.cs             # MAUI program initialization
```

## Setup and Installation

1. Clone this repository to your local machine
2. Open the solution file (`MauiLicensedApp.sln`) in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Select your target platform (Android, iOS, MacCatalyst) in the run configurations
6. Run the application

## Development

### Adding a New Platform

To add support for a new platform:

1. Implement the `ISystemInformation` interface for the new platform
2. Add the implementation to the appropriate platform-specific folder
3. Register the implementation in the platform's initialization code

### Licensing System

The application includes a custom licensing system that:
- Validates license keys
- Handles license activation and deactivation
- Uses platform-specific implementations to gather system information for licensing
- Generates an exception report for simulated unhandled exceptions

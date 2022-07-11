
# uepme

CLI Unreal Engine Project Manager tool.

![Nuget](https://img.shields.io/nuget/v/Uepme?color=g&label=nuget&logo=nuget&style=plastic)

## Features

- Project Management without Epic Games Launcher
- Creating a С++ Unreal project without the participation of Visual Studio IDE
- Building a project without Visual Studio IDE
- Creates .bat files for compilation already in the created sln project.

### !!! YOU NEED THE VISUAL STUDIO C++ PACKAGE FOR UNREAL, .NET 6 SDK AND WINDOWS SDK TO CREATE AN UNREAL C++ PROJECT

## Deployment

To deploy uepme run

```bash
  # clone repository
  git clone https://github.com/jejikeh/Uepme.git
  
  # build project
  dotnet pack

  # install package
  dotnet tool install --global --add-source ./nupkg uepme
```


## Installation

Install uepme as [nuget package](https://www.nuget.org/packages/Uepme)

```bash
  dotnet tool install --global Uepme --version 1.9.9
```
    
## Commands

#### Example

```bash
  # Configure uepme
  # on fist launch
  uepme
  # or later
  uepme config
```

| Parameter | Type |
| :-------- |:------------------------- |
| `create ExampleName` |  Creates an Unreal c++ project and launches the editor |
| `build ExampleName` | Build Unreal project  |
| `editor ExampleName` | Launch editor  |
| `cook ExampleName` | Cook content of project  |
| `compile ExampleName` | Build a standalone version of project  |
| `run ExampleName` | Run .exe standalone file   |
| `open ExampleName` | Open project folder in the Explorer   |
| `link C:\Documents\Projects\ExampleProject` | Linking an existing unreal project with uepme |
| `delete ExampleName` | Delete the uepme project configuration file |
| `list ExampleName` | Print all uepme projects |

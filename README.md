# uepme

```
 __  __     __    _____     ___ ___       __
/\ \/\ \  /'__`\ /\ '__`\ /' __` __`\   /'__`\
\ \ \_\ \/\  __/ \ \ \L\ \/\ \/\ \/\ \ /\  __/
 \ \____/\ \____\ \ \ ,__/\ \_\ \_\ \_\\ \____\
  \/___/  \/____/  \ \ \/  \/_/\/_/\/_/ \/____/
                    \ \_\
                     \/_/
```

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

1. Install uepme as [nuget package](https://www.nuget.org/packages/Uepme)

```bash
  dotnet tool install --global Uepme --version 2.0.9
```

2. Download the zip archive from the releases tab and add the uepme folder to the PATH

## Commands

#### Example

```bash
  # Configure uepme
  # on fist launch
  uepme
  # or later
  uepme config
  uepme new --name ExampleUepmeProject
```

| Parameter                                      | Type                                                  |
| :--------------------------------------------- | :---------------------------------------------------- |
| `new -n ExampleName`                           | Creates an Unreal c++ project and launches the editor |
| `build -n ExampleName`                         | Build Unreal project                                  |
| `editor -n ExampleName`                        | Launch editor                                         |
| `cook -n ExampleName`                          | Cook content of project                               |
| `compile -n ExampleName`                       | Build a standalone version of project                 |
| `run -n ExampleName`                           | Run .exe standalone file                              |
| `open -n ExampleName`                          | Open project folder in the Explorer                   |
| `link -p C:\Documents\Projects\ExampleProject` | Linking an existing unreal project with uepme         |
| `delete -n ExampleName`                        | Delete the uepme project configuration file           |
| `list ExampleName`                             | Print all uepme projects                              |

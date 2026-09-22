# Secret Project

Proyecto inicial de Godot 4.7 con C# para el repositorio [josvent90/secretProject](https://github.com/josvent90/secretProject).

## Requisitos

- Godot **4.7.2 .NET** en `C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe`.
- .NET SDK 8 o posterior. El proyecto apunta a `net8.0` y usa `Godot.NET.Sdk/4.7.2`.

`D:\Godot\Godot.exe` es el editor estándar y no carga scripts `.cs`. Este proyecto se abre con el ejecutable de `C:\GodotMono`.

## Abrir y ejecutar

1. Abre `project.godot` con el editor .NET de Godot 4.7.
2. Si el editor pide crear la solución C#, usa la que ya está en la raíz (`SecretProject.sln`).
3. Pulsa Play. La escena principal es `scenes/main/Main.tscn`.

Desde la terminal, en la raíz del repositorio:

```powershell
dotnet build SecretProject.csproj
& "C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe" --path .
& "C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe" --headless --path . --build-solutions --quit
```

## Estructura

```
scenes/main/Main.tscn    escena principal
scripts/main/Main.cs     script C# de esa escena
assets/                  arte, audio y otras fuentes importadas
```

Las instrucciones para agentes de código están en `AGENT.md`.

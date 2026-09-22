# AGENT.md

Instrucciones para agentes que trabajen en este repositorio. `AGENTS.md` es una copia de este archivo: si cambias uno, actualiza el otro.

## Proyecto

Secret Project es un juego en Godot 4.7 con C#. El código de juego vive en este repositorio y el lenguaje de scripting es C#, no GDScript.

El usuario escribe en español. Responde en español.

## Mapa de la cuenta y del remoto

Cuenta de GitHub: [josvent90](https://github.com/josvent90) (id 36771455, creada el 2018-02-23).

| Repositorio | Visibilidad | Rama | Qué es |
| --- | --- | --- | --- |
| [secretProject](https://github.com/josvent90/secretProject) | Público. Descripción en GitHub: "proyecto secreto de trabajo y casa". | `main` | Este proyecto. Remoto `origin`: `https://github.com/josvent90/secretProject.git`. |
| [netToUDP](https://github.com/josvent90/netToUDP) | Público | `main` | Clase C# para enviar un mensaje UDP a un microservicio que publica en Kafka. No forma parte de este juego. No lo mezcles aquí. |

El commit inicial del remoto (`9ee20ed`, 2026-09-22, autor `josvent90`) solo añadía `.gitignore`. El proyecto Godot se creó encima de ese clon en `D:\SecretProyect`.

No hagas push, ni abras pull requests, ni cambies la visibilidad del remoto salvo que el usuario lo pida.

## Entorno local

- Carpeta de trabajo: `D:\SecretProyect`.
- .NET SDK instalados: 6.0.428, 9.0.302, 10.0.103 y 10.0.301. El proyecto compila con el SDK 10 apuntando a `net8.0`.
- Editor del proyecto: `C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe`. Es el build .NET (el nombre del paquete dice `mono`). `.vscode/launch.json` apunta a ese ejecutable.
- `D:\Godot\Godot.exe` es Godot 4.7.2 estándar y no carga scripts `.cs`. No lo uses para este proyecto.

`dotnet build SecretProject.csproj` restaura `Godot.NET.Sdk/4.7.2` y genera `SecretProject.dll` en `.godot/mono/temp/bin/Debug/`. Esa carpeta no se versiona.

## Mapa del repositorio

```
project.godot                 nombre SecretProject, escena principal, features 4.7 / C# / Forward Plus
SecretProject.csproj          Sdk Godot.NET.Sdk/4.7.2, net8.0, net9.0 solo en Android
SecretProject.sln             configuraciones Debug, ExportDebug y ExportRelease
icon.svg                      icono de proyecto (icono por defecto de Godot, licencia MIT)
icon.svg.import               metadatos de importación; se versiona
scenes/main/Main.tscn         escena principal, 1280x720
scripts/main/Main.cs          script de esa escena, clase Main
assets/                       arte, audio y demás fuentes. Vacío salvo .gitkeep
.vscode/                      build y depuración. .gdignore evita que Godot importe esos JSON
```

`run/main_scene` es `res://scenes/main/Main.tscn`. La ventana es 1280x720, stretch `canvas_items` / `expand`, renderer Forward+.

El ensamblado se llama `SecretProject` (`dotnet/project/assembly_name`). No lo renombres sin renombrar también el `.csproj`, el `.sln` y la carpeta de salida.

## Convenciones

- Cada script C# es `public partial class`. El nombre de la clase es el nombre del archivo, sin namespace. Godot busca esa clase por el nombre del archivo; un namespace o un nombre distinto produce `Cannot find class`.
- La API de Godot en C# usa PascalCase. `GD.Print` sustituye a `print`. Los structs (`Vector2`, `Color`) se copian al leerlos: modifica una copia y vuelve a asignar la propiedad.
- Tras añadir `[Export]`, señales o scripts `tool`, hay que recompilar para que el editor los vea.
- `Call`, `CallDeferred`, `Get`, `Set` y `Connect` esperan el nombre `snake_case` original de Godot. Para código propio usa los `StringName` de `PropertyName`, `MethodName` y `SignalName`.
- Escenas en `scenes/`, scripts en `scripts/`, fuentes importadas en `assets/`. Empareja la ruta: `scenes/main/Main.tscn` con `scripts/main/Main.cs`.
- No conviertas el proyecto a GDScript ni quites `C#` de `config/features`.
- No cambies a mano la versión de `Godot.NET.Sdk` salvo que el editor .NET instalado sea otra versión 4.7.x. El SDK tiene que coincidir con el editor.
- Conserva las dos líneas de `TargetFramework`: `net8.0` y la condición `net9.0` para Android. No actives `ImplicitUsings` ni cambies el formato del `.sln`; Godot genera esas tres configuraciones y nada más.
- `project.godot` se edita desde el editor. Si hay que tocarlo a mano, conserva `config_version=5` y las features `PackedStringArray("4.7", "C#", "Forward Plus")`.
- Los scripts usan tabuladores, igual que la plantilla de Godot.

## Qué no versionar

`.gitignore` ya excluye `.godot/`, `.import/`, `export.cfg`, `export_credentials.cfg`, `.mono/`, `bin/`, `obj/`, `.vs/` y archivos de usuario de MSBuild. No reviertas esas exclusiones. `export_credentials.cfg` puede contener secretos de tiendas.

Se versionan `.csproj`, `.sln` y los `.import` de los assets. No se versiona nada dentro de `.godot/`, incluido `.godot/mono`.

## Comandos

Compilar:

```powershell
dotnet build SecretProject.csproj
```

Abrir o jugar:

```powershell
& "C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe" --editor --path .
& "C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe" --path .
& "C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe" --headless --path . --build-solutions --quit
```

Un arranque correcto imprime `SecretProject listo.` desde `Main._Ready`.

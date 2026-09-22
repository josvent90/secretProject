# Secret Project

Proyecto inicial de Godot 4.7 con C# para el repositorio [josvent90/secretProject](https://github.com/josvent90/secretProject).

## Requisitos

- Godot **4.7.2 .NET** en `C:\GodotMono\Godot_v4.7.2-stable_mono_win64.exe`.
- .NET SDK 8 o posterior. El proyecto apunta a `net8.0` y usa `Godot.NET.Sdk/4.7.2`.

`D:\Godot\Godot.exe` es el editor estándar y no carga scripts `.cs`. Este proyecto se abre con el ejecutable de `C:\GodotMono`.

## Jugar

En la pantalla de título, pulsa Enter. En el nivel:

- A o flecha izquierda, y D o flecha derecha, para moverte.
- Espacio, W o flecha arriba para saltar.
- Recoge las 3 monedas y entra en la puerta.

Los bloques, las monedas y la puerta salen del Roguelike/RPG pack de [Kenney](https://kenney.nl/assets/roguelike-rpg-pack) (CC0). Esa hoja es de vista cenital, así que el personaje de perfil está en `assets/player/player.png`.

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
scenes/main/Main.tscn       pantalla de título
scenes/level/Level.tscn     nivel de plataformas
scenes/player/Player.tscn   jugador
scenes/pickup/Coin.tscn     moneda
scenes/goal/Goal.tscn       puerta
assets/Spritesheet/         hoja Kenney, 16×16 con 1 px de separación
assets/Map/                 mapas de ejemplo; el nivel no los usa
assets/player/player.png    personaje de perfil, dos frames
```

Las instrucciones para agentes de código están en `AGENT.md`.

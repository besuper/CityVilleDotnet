# CityVille .NET
> A .NET 11 backend recreation of the classic CityVille (2013 version) flash game

**Beta status**: the game is playable and most core systems work, but many features are still being implemented.

## Features

-   **Four worlds**: Main city, Downtown, Lakefront and Alps
-   **Building & farming**: construction sites, businesses, residences, plots, greenhouse, remodel, upgrades
-   **City expansion**: expansion licenses and incentivized expansions
-   **Quests**: hundreds of quests and task actions
-   **Franchises**: headquarters, supply, daily bonus, Samantha's city
-   **Trains & ships**, zoo animals, factory workers, crew members and gates, mastery gates
-   **Cops & bandits**, weather effects
-   **Collections & wishlist**, inventory, consumables, bundles
-   **Friends**: requests, visits, helping neighbors, energy
-   **Multi languages**: english, spanish, french, italian, portuguese
-   **Embedded [ruffle](https://ruffle.rs/) flash emulator**: no need to install Flash

<!-- progress:start -->
## Implementation progress
_Last update: 2026-11-13 · 🟡 = partial_

| | Progress | |
|---|---|---|
| Client transactions | <img src="docs/progress/transactions.svg" alt="36%"> 36% | 91 ✅ · 20 🟡 · 145 ❌ |
| World actions | <img src="docs/progress/world_actions.svg" alt="53%"> 53% | 20 ✅ · 4 🟡 · 14 ❌ |
| Quest task actions | <img src="docs/progress/quest_actions.svg" alt="94%"> 94% | 95 ✅ · 3 🟡 · 83 ❌ (% weighted by use) |
| Completable quests | <img src="docs/progress/quests.svg" alt="86%"> 86% | 1091 / 1276 |
<!-- progress:end -->

## Versions

This server only supports one version of CityVille at a time. If you want to play other versions, switch between branches.

| Version |                              Branch                              |   Last update   |
|:-------:|:----------------------------------------------------------------:|:---------------:|
|  2011   | [**2011**](https://github.com/besuper/CityVilleDotnet/tree/main) |   2026-01-30    |
|  2012   | [**2012**](https://github.com/besuper/CityVilleDotnet/tree/2012) |   2026-11-11    |
|  2013   | [**2013**](https://github.com/besuper/CityVilleDotnet/tree/2013) | Current version |

## Getting Started

### Prerequisites
-   Docker & Docker Compose, **or**
-   .NET 11 SDK and MS SQL Server
-   The game assets (see [Assets](#assets))

### Run with Docker

1.  **Clone the repository**
    ```bash
    git clone https://github.com/besuper/CityVilleDotnet
    cd CityVilleDotnet
    ```

2.  **Create the `wwwroot/` and `logs/` folders** at the repository root, and put the game assets in `wwwroot/`.

3.  **Start the stack**
    ```bash
    docker-compose up --build
    ```

4.  **Play** at `http://localhost:5221/Game`

### Run locally

The default connection string targets a local SQL Server with Windows authentication (`appsettings.json`). The database is created and migrated on startup.

```bash
dotnet run --project CityVilleDotnet.Api
```

Then open `http://localhost:5221`. Assets go in `CityVilleDotnet.Api/wwwroot/`.

### Prebuilt releases

Self-contained builds for Windows, Linux and macOS are published weekly as pre-releases on the [releases page](https://github.com/besuper/CityVilleDotnet/releases).

## Assets

**Important notice**: game assets (images, sounds, sprites, SWF, settings) are not included in this repository as they are copyrighted material owned by Zynga Inc.

1.  **Get the assets** from the [Raise the Empires Discord](https://discord.gg/xrNE6Hg)
2.  **Place them** in the `wwwroot` folder:

    ```
    wwwroot/
    ├── assets/
    ├── lang/
    ├── Game.2013.swf
    ├── Preloader.swf
    ├── EmbeddedArt.swf
    ├── FontMapper.swf
    ├── gameSettings.xml
    ├── questSettings.xml
    ├── effectsConfig.xml
    ├── bootstrap.xml
    └── settings.amf.z
    ```

**Note**: this project is for educational purposes only. All game assets remain the property of Zynga Inc.

## Tests

```bash
dotnet test
```

-   `test/CityVilleDotnet.Test.Domain`: domain entity unit tests
-   `test/CityVilleDotnet.Test.Integration`: transaction tests against a real SQL Server (**requires Docker**, via Testcontainers)
-   `test/CityVilleDotnet.Factory`: bogus factories shared by both

## Project Structure

```
CityVilleDotnet/
├── CityVilleDotnet.Api/          # AMF gateway, transaction handlers (Services/), Razor Pages
├── CityVilleDotnet.Common/       # Game & quest settings (XML), enums, utilities
├── CityVilleDotnet.Domain/       # Entities and business logic, DTOs
├── CityVilleDotnet.Persistence/  # EF Core DbContext, configurations, migrations
├── FluorineFX/                   # AMF serialization library
├── test/                         # Unit & integration tests, factories
├── docker-compose.yml
└── Dockerfile
```

## Credits

Thanks to everyone who helped bring CityVille back:

-   **xmohamedX1** huge contribution on the assets: recovery, fixes
-   **Captain Rusty** helped build the first prototype in 2011 and scraped assets from the wiki
-   **MissTery** hosts the [raise the Empires Discord](https://discord.gg/xrNE6Hg) server & helping with assets
-   **Antonio225** carried on maintaining the first prototype and helps a lot on discord
-   **nidzo** helped recreate Samantha's city and with the assets

## License

This project is licensed under the GPL-3.0 license - see the [LICENSE](LICENSE) file for details.

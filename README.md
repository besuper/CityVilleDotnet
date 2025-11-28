# CityVille .NET
> A .NET 10 backend recreation of the classic CityVille game

**Alpha Status**: This project is currently in alpha development. The game is playable but many features are still being implemented.

## Features

-   Simple building system - build, farm and expand your city
-   Quest system - many quests are supported and functional
-   User account management and authentication
-   Basic friend system - add, visit and manage friends
-   Support for user inventory

## Getting Started

### Prerequisites
-   Docker & Docker Compose
-   .NET 10 SDK (for development)

### Quick Start

1.  **Clone the repository**
    ```bash
    git clone https://github.com/besuper/CityVilleDotnet
    cd CityVilleDotnet
    ```

2.  **Create folders wwwroot/ and logs/**
3.  **Set up game assets** (see Assets section below)

4.  **Run with Docker**

    ```bash
    docker-compose up --build
    ```

5.  **Access the game**

    -   Game: `http://localhost:8080/Game`

### Development Setup

```bash
# Restore dependencies
dotnet restore

# Run the backend locally
dotnet run --project CityVilleDotnet.Api
```

## Assets

**Important Notice**: Game assets (images, sounds, sprites) are not included in this repository as they are copyrighted material owned by Zynga Inc.

To run the game with proper assets:

1.  **Get assets from Discord** - You can download the assets from [Raise the Empires discord](https://discord.gg/xrNE6Hg)
2.  **Place assets** in the `wwwroot` folder structure:

    ```
    wwwroot/
    ├── assets/
    ├── hashed/
    └── Game.26346.swf
    ```


**Note**: This project is for educational purposes only. All game assets remain the property of Zynga Inc.

## Project Structure

```
CityVilleDotnet/
├── CityVilleDotnet.Api/          # Game backend and endpoints
├── CityVilleDotnet.Common/       # Shared utilities and helpers
├── CityVilleDotnet.Domain/       # Business logic and entities
├── CityVilleDotnet.Persistence/  # Data access layer
├── docker-compose.yml            # Docker configuration
└── Dockerfile                    # Container build instructions
```

## License

This project is licensed under the GPL-3.0 license - see the [LICENSE](https://github.com/besuper/CityVilleDotnet/blob/main/LICENSE) file for details.
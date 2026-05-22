# SESI DEFENSE - Complete Project Structure

## Directory Organization

```
sesi-defense/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── WaveManager.cs
│   │   │   ├── CurrencyManager.cs
│   │   │   ├── BaseManager.cs
│   │   │   ├── GridManager.cs
│   │   │   ├── GameStateManager.cs
│   │   │   └── EventSystem.cs
│   │   │
│   │   ├── Towers/
│   │   │   ├── Base/
│   │   │   │   ├── Tower.cs (Base class)
│   │   │   │   ├── TowerStats.cs
│   │   │   │   ├── TowerUpgrade.cs
│   │   │   │   ├── TowerAbility.cs
│   │   │   │   └── TowerAnimator.cs
│   │   │   │
│   │   │   ├── Students/
│   │   │   │   ├── Vitor_Archer.cs
│   │   │   │   ├── Nandoio_DragonMage.cs
│   │   │   │   ├── Nicolas_Alchemist.cs
│   │   │   │   ├── Paulo_Ninja.cs
│   │   │   │   ├── Henrique_Hacker.cs
│   │   │   │   ├── Diego_Tank.cs
│   │   │   │   ├── Migeu_Farmer.cs
│   │   │   │   ├── Kalebe_Mechanic.cs
│   │   │   │   ├── Nikolas_Complainer.cs
│   │   │   │   └── Gini_Smokemaster.cs
│   │   │   │
│   │   │   ├── Targeting/
│   │   │   │   ├── TargetingSystem.cs
│   │   │   │   ├── TargetSelector.cs
│   │   │   │   └── TargetingStrategies.cs
│   │   │   │
│   │   │   ├── Projectiles/
│   │   │   │   ├── Projectile.cs
│   │   │   │   ├── ProjectileFactory.cs
│   │   │   │   └── ProjectilePool.cs
│   │   │   │
│   │   │   └── Effects/
│   │   │       ├── TowerEffects.cs
│   │   │       └── UpgradeVisualizer.cs
│   │   │
│   │   ├── Enemies/
│   │   │   ├── Base/
│   │   │   │   ├── Enemy.cs
│   │   │   │   ├── EnemyStats.cs
│   │   │   │   ├── EnemyAnimator.cs
│   │   │   │   └── EnemyHealth.cs
│   │   │   │
│   │   │   ├── Types/
│   │   │   │   ├── Monitor.cs
│   │   │   │   ├── Teacher.cs
│   │   │   │   ├── Guard.cs
│   │   │   │   ├── SenaiInstructor.cs
│   │   │   │   ├── Rat.cs
│   │   │   │   ├── Bat.cs
│   │   │   │   ├── SchoolRobot.cs
│   │   │   │   ├── CafeteriaWorker.cs
│   │   │   │   ├── PossessedComputer.cs
│   │   │   │   ├── HomeworkPaper.cs
│   │   │   │   └── LivingChair.cs
│   │   │   │
│   │   │   ├── SpecialEnemies/
│   │   │   │   ├── WifiFailure.cs
│   │   │   │   ├── SchoolPrinter.cs
│   │   │   │   ├── GiantRat.cs
│   │   │   │   ├── BatSwarm.cs
│   │   │   │   └── OverflowingToilet.cs
│   │   │   │
│   │   │   ├── Bosses/
│   │   │   │   ├── Boss.cs (Base class)
│   │   │   │   ├── BossPhase.cs
│   │   │   │   ├── DirectorNecromancer.cs
│   │   │   │   └── Julio.cs
│   │   │   │
│   │   │   ├── AI/
│   │   │   │   ├── EnemyPathfinding.cs
│   │   │   │   ├── EnemyMovement.cs
│   │   │   │   ├── EnemyBehavior.cs
│   │   │   │   └── WaveController.cs
│   │   │   │
│   │   │   └── Spawning/
│   │   │       ├── EnemySpawner.cs
│   │   │       ├── SpawnPoint.cs
│   │   │       └── WaveData.cs
│   │   │
│   │   ├── UI/
│   │   │   ├── Screens/
│   │   │   │   ├── MainMenuUI.cs
│   │   │   │   ├── LobbyUI.cs
│   │   │   │   ├── GameplayUI.cs
│   │   │   │   ├── PauseMenuUI.cs
│   │   │   │   ├── GameOverUI.cs
│   │   │   │   ├── ResultsUI.cs
│   │   │   │   └── SettingsUI.cs
│   │   │   │
│   │   │   ├── HUD/
│   │   │   │   ├── TowerPlacementUI.cs
│   │   │   │   ├── TowerUpgradeUI.cs
│   │   │   │   ├── WaveIndicatorUI.cs
│   │   │   │   ├── HealthBarUI.cs
│   │   │   │   ├── BaseHealthUI.cs
│   │   │   │   ├── CurrencyDisplayUI.cs
│   │   │   │   ├── DamageNumberUI.cs
│   │   │   │   ├── MinimapUI.cs
│   │   │   │   └── TimeDisplayUI.cs
│   │   │   │
│   │   │   ├── Components/
│   │   │   │   ├── AnimatedButton.cs
│   │   │   │   ├── AnimatedPanel.cs
│   │   │   │   ├── AnimatedHealthBar.cs
│   │   │   │   ├── ScrollableContent.cs
│   │   │   │   ├── TooltipSystem.cs
│   │   │   │   └── PopupManager.cs
│   │   │   │
│   │   │   └── Animations/
│   │   │       ├── UIAnimationController.cs
│   │   │       ├── TransitionEffects.cs
│   │   │       └── CanvasAnimator.cs
│   │   │
│   │   ├── Multiplayer/
│   │   │   ├── Networking/
│   │   │   │   ├── NetworkManager.cs
│   │   │   │   ├── PlayerNetworking.cs
│   │   │   │   ├── TowerNetworking.cs
│   │   │   │   ├── EnemyNetworking.cs
│   │   │   │   ├── MessageHandler.cs
│   │   │   │   └── SyncManager.cs
│   │   │   │
│   │   │   ├── Lobbies/
│   │   │   │   ├── LobbyManager.cs
│   │   │   │   ├── LobbyPlayer.cs
│   │   │   │   ├── MatchmakingSystem.cs
│   │   │   │   └── LobbyChat.cs
│   │   │   │
│   │   │   ├── Players/
│   │   │   │   ├── PlayerManager.cs
│   │   │   │   ├── PlayerStats.cs
│   │   │   │   ├── PlayerInventory.cs
│   │   │   │   ├── PlayerSkinsManager.cs
│   │   │   │   └── PlayerInputHandler.cs
│   │   │   │
│   │   │   └── Synchronization/
│   │   │       ├── StateSync.cs
│   │   │       ├── EventSync.cs
│   │   │       └── ReliableMessageQueue.cs
│   │   │
│   │   ├── Progression/
│   │   │   ├── LevelManager.cs
│   │   │   ├── UnlockSystem.cs
│   │   │   ├── AchievementSystem.cs
│   │   │   ├── TowerUnlocks.cs
│   │   │   ├── SkinUnlocks.cs
│   │   │   ├── MapUnlocks.cs
│   │   │   ├── TitleSystem.cs
│   │   │   ├── RaritySystem.cs
│   │   │   └── ProgressionTracker.cs
│   │   │
│   │   ├── Systems/
│   │   │   ├── SaveSystem.cs
│   │   │   ├── SaveData.cs
│   │   │   ├── SettingsManager.cs
│   │   │   ├── AudioManager.cs
│   │   │   ├── ParticleEffectPool.cs
│   │   │   ├── ObjectPool.cs
│   │   │   ├── ConfigManager.cs
│   │   │   ├── LocalizationManager.cs
│   │   │   ├── AnalyticsManager.cs
│   │   │   └── InputManager.cs
│   │   │
│   │   ├── Maps/
│   │   │   ├── MapManager.cs
│   │   │   ├── MapData.cs
│   │   │   ├── MapEnvironment.cs
│   │   │   ├── PathController.cs
│   │   │   ├── TowerPlacementSystem.cs
│   │   │   └── EnvironmentEffects.cs
│   │   │
│   │   ├── Modes/
│   │   │   ├── GameMode.cs (Base)
│   │   │   ├── StoryMode.cs
│   │   │   ├── EndlessMode.cs
│   │   │   └── ChaosMode.cs
│   │   │
│   │   └── Utilities/
│   │       ├── MathUtils.cs
│   │       ├── PhysicsUtils.cs
│   │       ├── CoroutineManager.cs
│   │       ├── DebugManager.cs
│   │       ├── RandomWeightedSelector.cs
│   │       ├── TimerManager.cs
│   │       └── Logger.cs
│   │
│   ├── Prefabs/
│   │   ├── Towers/
│   │   │   ├── Vitor.prefab
│   │   │   ├── Nandoio.prefab
│   │   │   ├── Nicolas.prefab
│   │   │   ├── Paulo.prefab
│   │   │   ├── Henrique.prefab
│   │   │   ├── Diego.prefab
│   │   │   ├── Migeu.prefab
│   │   │   ├── Kalebe.prefab
│   │   │   ├── Nikolas.prefab
│   │   │   └── Gini.prefab
│   │   │
│   │   ├── Enemies/
│   │   │   ├── BasicEnemies/
│   │   │   ├── SpecialEnemies/
│   │   │   ├── Bosses/
│   │   │   └── Projectiles/
│   │   │
│   │   ├── VFX/
│   │   │   ├── Explosions/
│   │   │   ├── Particles/
│   │   │   └── AttackEffects/
│   │   │
│   │   ├── UI/
│   │   │   ├── Screens/
│   │   │   └── HUD/
│   │   │
│   │   └── Environment/
│   │       └── MapElements/
│   │
│   ├── Models/
│   │   ├── Towers/
│   │   ├── Enemies/
│   │   ├── Base/
│   │   ├── Environment/
│   │   └── Characters/
│   │
│   ├── Animations/
│   │   ├── Towers/
│   │   ├── Enemies/
│   │   ├── UI/
│   │   └── Environment/
│   │
│   ├── Materials/
│   │   ├── Towers/
│   │   ├── Enemies/
│   │   ├── Environment/
│   │   ├── Effects/
│   │   └── UI/
│   │
│   ├── Textures/
│   │   ├── Characters/
│   │   ├── Environment/
│   │   ├── UI/
│   │   └── Effects/
│   │
│   ├── Audio/
│   │   ├── SFX/
│   │   │   ├── Towers/
│   │   │   ├── Enemies/
│   │   │   ├── Impacts/
│   │   │   ├── UI/
│   │   │   └── Ambient/
│   │   │
│   │   └── Music/
│   │       ├── MainTheme.mp3
│   │       ├── BattleMusic.mp3
│   │       ├── BossMusic.mp3
│   │       ├── MenuMusic.mp3
│   │       └── EndlessMode.mp3
│   │
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Lobby.unity
│   │   ├── GameplayLabs.unity
│   │   ├── GameplayClassroom.unity
│   │   ├── GameplayYard.unity
│   │   ├── GameplayGym.unity
│   │   ├── GameplayWorkshop.unity
│   │   ├── GameplayHallway.unity
│   │   ├── GameplayLibrary.unity
│   │   ├── Settings.unity
│   │   └── LoadingScreen.unity
│   │
│   └── Resources/
│       ├── Data/
│       │   ├── TowerData.json
│       │   ├── EnemyData.json
│       │   ├── WaveData.json
│       │   ├── MapData.json
│       │   ├── GameBalance.json
│       │   └── UnlockData.json
│       │
│       └── Localization/
│           ├── EN.json
│           └── PT.json
│
├── Packages/
│   └── manifest.json (Netcode for GameObjects, DOTween, etc.)
│
├── ProjectSettings/
│
├── Documentation/
│   ├── GAME_DESIGN.md
│   ├── NETWORK_ARCHITECTURE.md
│   ├── TOWER_SYSTEM.md
│   ├── ENEMY_SYSTEM.md
│   ├── UI_GUIDE.md
│   ├── PROGRESSION_GUIDE.md
│   ├── PERFORMANCE_GUIDE.md
│   └── CONTRIBUTING.md
│
├── Build/
│   ├── WebGL/
│   ├── Windows/
│   ├── Mac/
│   └── Linux/
│
└── .gitignore
```

## File Size Estimates

- **Core Scripts:** ~500KB
- **Tower Scripts:** ~300KB
- **Enemy Scripts:** ~250KB
- **UI Scripts:** ~200KB
- **Multiplayer Scripts:** ~150KB
- **Systems:** ~100KB
- **Total Code:** ~1.5MB

## Dependencies

- Unity 2022.3 LTS
- Netcode for GameObjects
- Mirror (alternative)
- DOTween (animations)
- TextMesh Pro
- Cinemachine (cameras)

## Naming Conventions

- **Classes:** PascalCase
- **Methods:** PascalCase
- **Variables:** camelCase
- **Constants:** UPPER_SNAKE_CASE
- **Prefabs:** PascalCase with component type suffix
- **Scenes:** PascalCase with map name
- **Materials:** "Mat_" prefix
- **Animations:** "Anim_" prefix

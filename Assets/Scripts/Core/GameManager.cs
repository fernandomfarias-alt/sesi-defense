using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace SesiDefense.Core
{
    /// <summary>
    /// Central game manager that orchestrates all major systems.
    /// Handles game state, initialization, and system coordination.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private BaseManager baseManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private float gameStartDelay = 3f;

        private static GameManager instance;
        private EventSystem eventSystem;
        private Dictionary<Type, MonoBehaviour> systemRegistry;
        private bool isInitialized = false;
        private bool isPaused = false;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject managerObject = new GameObject("[GameManager]");
                        instance = managerObject.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSystems();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Initializes all core game systems.
        /// </summary>
        private void InitializeSystems()
        {
            if (isInitialized) return;

            try
            {
                // Create system registry
                systemRegistry = new Dictionary<Type, MonoBehaviour>();

                // Initialize event system
                GameObject eventSystemObject = new GameObject("[EventSystem]");
                eventSystem = eventSystemObject.AddComponent<EventSystem>();
                DontDestroyOnLoad(eventSystemObject);
                RegisterSystem(eventSystem);

                // Initialize core managers if not already assigned
                if (gameStateManager == null)
                {
                    gameStateManager = GetComponent<GameStateManager>();
                    if (gameStateManager == null)
                    {
                        gameStateManager = gameObject.AddComponent<GameStateManager>();
                    }
                }
                RegisterSystem(gameStateManager);

                if (currencyManager == null)
                {
                    currencyManager = GetComponent<CurrencyManager>();
                    if (currencyManager == null)
                    {
                        currencyManager = gameObject.AddComponent<CurrencyManager>();
                    }
                }
                RegisterSystem(currencyManager);

                if (baseManager == null)
                {
                    baseManager = FindObjectOfType<BaseManager>();
                    if (baseManager != null)
                    {
                        RegisterSystem(baseManager);
                    }
                }

                // Find or create other managers
                if (waveManager == null)
                {
                    waveManager = FindObjectOfType<WaveManager>();
                }

                if (gridManager == null)
                {
                    gridManager = FindObjectOfType<GridManager>();
                }

                isInitialized = true;
                Logger.Log("GameManager initialized successfully", LogLevel.Info);

                // Dispatch initialization complete event
                eventSystem?.Dispatch<GameInitializationCompleteEvent>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Error initializing GameManager: {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        /// Registers a system for central management.
        /// </summary>
        private void RegisterSystem<T>(T system) where T : MonoBehaviour
        {
            systemRegistry[typeof(T)] = system;
        }

        /// <summary>
        /// Retrieves a registered system.
        /// </summary>
        public T GetSystem<T>() where T : MonoBehaviour
        {
            if (systemRegistry.TryGetValue(typeof(T), out MonoBehaviour system))
            {
                return system as T;
            }
            return null;
        }

        /// <summary>
        /// Starts a new game with specified settings.
        /// </summary>
        public void StartGame(GameSettings settings)
        {
            if (!isInitialized)
            {
                Logger.Log("Game not initialized", LogLevel.Error);
                return;
            }

            Logger.Log($"Starting game on map: {settings.selectedMap}", LogLevel.Info);

            // Initialize game state
            gameStateManager?.Initialize(settings);

            // Initialize currency
            currencyManager?.Initialize(settings.startingCurrency);

            // Initialize base
            baseManager?.Initialize(settings.baseHealth);

            // Dispatch game start event
            eventSystem?.Dispatch<GameStartedEvent>();

            // Delay wave start for dramatic effect
            StartCoroutine(DelayedWaveStart());
        }

        private IEnumerator DelayedWaveStart()
        {
            yield return new WaitForSeconds(gameStartDelay);
            waveManager?.StartWaves();
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void PauseGame()
        {
            if (isPaused) return;

            isPaused = true;
            Time.timeScale = 0f;
            eventSystem?.Dispatch<GamePausedEvent>();
            Logger.Log("Game paused", LogLevel.Info);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            if (!isPaused) return;

            isPaused = false;
            Time.timeScale = 1f;
            eventSystem?.Dispatch<GameResumedEvent>();
            Logger.Log("Game resumed", LogLevel.Info);
        }

        /// <summary>
        /// Toggles pause state.
        /// </summary>
        public void TogglePause()
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        /// <summary>
        /// Ends the game with victory.
        /// </summary>
        public void EndGameVictory(GameResults results)
        {
            Time.timeScale = 0f;
            eventSystem?.Dispatch(new GameEndedEvent
            {
                isVictory = true,
                results = results
            });
            Logger.Log("Game ended - Victory!", LogLevel.Info);
        }

        /// <summary>
        /// Ends the game with defeat.
        /// </summary>
        public void EndGameDefeat(GameResults results)
        {
            Time.timeScale = 0f;
            eventSystem?.Dispatch(new GameEndedEvent
            {
                isVictory = false,
                results = results
            });
            Logger.Log("Game ended - Defeat!", LogLevel.Warning);
        }

        /// <summary>
        /// Gets the current game state.
        /// </summary>
        public GameState GetGameState()
        {
            return gameStateManager?.CurrentState ?? GameState.Menu;
        }

        /// <summary>
        /// Checks if game is currently running.
        /// </summary>
        public bool IsGameRunning()
        {
            return gameStateManager?.CurrentState == GameState.Playing;
        }

        /// <summary>
        /// Checks if game is paused.
        /// </summary>
        public bool IsGamePaused()
        {
            return isPaused;
        }

        /// <summary>
        /// Gets the event system.
        /// </summary>
        public EventSystem GetEventSystem()
        {
            return eventSystem;
        }

        /// <summary>
        /// Resets the game to initial state.
        /// </summary>
        public void ResetGame()
        {
            Time.timeScale = 1f;
            isPaused = false;

            gameStateManager?.Reset();
            currencyManager?.Reset();
            baseManager?.Reset();
            waveManager?.Reset();

            eventSystem?.Dispatch<GameResetEvent>();
            Logger.Log("Game reset", LogLevel.Info);
        }
    }

    /// <summary>
    /// Game settings container.
    /// </summary>
    public class GameSettings
    {
        public string selectedMap { get; set; }
        public GameMode gameMode { get; set; }
        public int playerCount { get; set; }
        public int startingCurrency { get; set; } = 500;
        public int baseHealth { get; set; } = 100;
        public float gameDifficulty { get; set; } = 1f;
    }

    /// <summary>
    /// Game results after completion.
    /// </summary>
    public class GameResults
    {
        public int finalWave { get; set; }
        public int totalKills { get; set; }
        public int totalDamageDealt { get; set; }
        public int moneyEarned { get; set; }
        public float playTime { get; set; }
        public List<PlayerResult> playerResults { get; set; }
    }

    public class PlayerResult
    {
        public string playerName { get; set; }
        public int towersPlaced { get; set; }
        public int killContribution { get; set; }
        public int moneySpent { get; set; }
    }

    /// <summary>
    /// Game state enumeration.
    /// </summary>
    public enum GameState
    {
        Menu,
        Lobby,
        Loading,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public enum GameMode
    {
        Story,
        Endless,
        Chaos
    }

    /// <summary>
    /// Game initialization event.
    /// </summary>
    public class GameInitializationCompleteEvent { }

    /// <summary>
    /// Game started event.
    /// </summary>
    public class GameStartedEvent { }

    /// <summary>
    /// Game paused event.
    /// </summary>
    public class GamePausedEvent { }

    /// <summary>
    /// Game resumed event.
    /// </summary>
    public class GameResumedEvent { }

    /// <summary>
    /// Game ended event.
    /// </summary>
    public class GameEndedEvent
    {
        public bool isVictory { get; set; }
        public GameResults results { get; set; }
    }

    /// <summary>
    /// Game reset event.
    /// </summary>
    public class GameResetEvent { }
}

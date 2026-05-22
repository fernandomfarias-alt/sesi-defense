using UnityEngine;
using System.Collections.Generic;

namespace SesiDefense.Enemies.Spawning
{
    using Core;
    using Base;

    /// <summary>
    /// Manages enemy spawning for waves.
    /// Handles spawn timing, enemy type instantiation, and pooling.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Dictionary<EnemyType, GameObject> enemyPrefabs = new Dictionary<EnemyType, GameObject>();
        [SerializeField] private int poolSize = 50;

        private Dictionary<EnemyType, Queue<Enemy>> enemyPools = new Dictionary<EnemyType, Queue<Enemy>>();
        private EventSystem eventSystem;
        private WaveManager waveManager;
        private List<Enemy> activeEnemies = new List<Enemy>();

        private void Awake()
        {
            eventSystem = GameManager.Instance.GetEventSystem();
            waveManager = FindObjectOfType<WaveManager>();
            InitializePool();
        }

        /// <summary>
        /// Initializes object pools for all enemy types.
        /// </summary>
        private void InitializePool()
        {
            foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
            {
                enemyPools[enemyType] = new Queue<Enemy>(poolSize);
            }
        }

        /// <summary>
        /// Spawns a wave of enemies based on spawn data.
        /// </summary>
        public void SpawnWave(List<WaveManager.EnemySpawnData> spawnData)
        {
            StartCoroutine(SpawnWaveCoroutine(spawnData));
        }

        private System.Collections.IEnumerator SpawnWaveCoroutine(List<WaveManager.EnemySpawnData> spawnData)
        {
            foreach (WaveManager.EnemySpawnData data in spawnData)
            {
                yield return new WaitForSeconds(data.spawnDelay);
                SpawnEnemy(data);
            }
        }

        /// <summary>
        /// Spawns a single enemy with scaling parameters.
        /// </summary>
        private void SpawnEnemy(WaveManager.EnemySpawnData spawnData)
        {
            Enemy enemy = GetOrCreateEnemy(spawnData.enemyType);
            if (enemy == null)
            {
                Logger.Log($"Failed to spawn enemy: {spawnData.enemyType}", LogLevel.Error);
                return;
            }

            // Apply scaling
            // TODO: Apply health and damage multipliers to enemy

            enemy.gameObject.SetActive(true);
            if (spawnPoint != null)
            {
                enemy.transform.position = spawnPoint.position;
            }

            activeEnemies.Add(enemy);
            eventSystem?.Dispatch(new EnemySpawnedEvent { enemy = enemy });
        }

        /// <summary>
        /// Gets or creates an enemy from the pool.
        /// </summary>
        private Enemy GetOrCreateEnemy(EnemyType enemyType)
        {
            Queue<Enemy> pool = enemyPools[enemyType];

            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }

            // Create new enemy
            GameObject prefab = GetEnemyPrefab(enemyType);
            if (prefab == null)
            {
                Logger.Log($"No prefab for enemy type: {enemyType}", LogLevel.Error);
                return null;
            }

            GameObject enemyObject = Instantiate(prefab, transform);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            return enemy;
        }

        /// <summary>
        /// Gets the prefab for an enemy type.
        /// TODO: Load from resources.
        /// </summary>
        private GameObject GetEnemyPrefab(EnemyType enemyType)
        {
            if (enemyPrefabs.ContainsKey(enemyType))
            {
                return enemyPrefabs[enemyType];
            }

            // Load from resources
            string resourcePath = $"Prefabs/Enemies/{enemyType}";
            GameObject prefab = Resources.Load<GameObject>(resourcePath);

            if (prefab != null)
            {
                enemyPrefabs[enemyType] = prefab;
            }

            return prefab;
        }

        /// <summary>
        /// Returns an enemy to the pool.
        /// </summary>
        public void ReturnEnemyToPool(Enemy enemy, EnemyType enemyType)
        {
            activeEnemies.Remove(enemy);
            enemy.gameObject.SetActive(false);
            enemyPools[enemyType].Enqueue(enemy);
            waveManager?.RecordEnemyDefeat();
        }

        /// <summary>
        /// Checks if all enemies in current wave are defeated.
        /// </summary>
        public bool IsWaveComplete()
        {
            return activeEnemies.Count == 0;
        }

        /// <summary>
        /// Clears all active enemies.
        /// </summary>
        public void ClearWave()
        {
            foreach (Enemy enemy in activeEnemies)
            {
                enemy.gameObject.SetActive(false);
            }
            activeEnemies.Clear();
        }
    }

    public class EnemySpawnedEvent
    {
        public Enemy enemy { get; set; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static TPSGame.TPSGameDataModel;

namespace TPSGame
{
    public class SpawnController : MonoBehaviour
    {
        #region Private Variables
        [Header("Transform Points")]
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
        [SerializeField] private int waveCount = 5;
        [SerializeField] private int enemiesPerWave = 10;

        private int currentWave = 0;
        private int enemiesRemaining = 0;
        #endregion

        #region Public Methods

        private void Start()
        {
            if (TPSGameManager.Instance.CurrentGameState == GameState.GameOver) return;
            TPSGameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            TPSGameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
        public void StartNextWave()
        {
            if (TPSGameManager.Instance.CurrentGameState == GameState.GameOver) return;
            if (currentWave >= waveCount)
            {
                Debug.Log("All waves completed!");
                return;
            }

            currentWave++;
            enemiesRemaining = enemiesPerWave;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemies();
            }
        }

        void SpawnEnemies()
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            int enemiesToSpawn = Random.Range(1, 3); // 1 or 2 enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                bool spawnBigEnemy = Random.value > 0.5f;
                GameObject enemy;

                // Apply a random offset to the spawn position
                Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                Vector3 spawnPosition = spawnPoint.position + offset;

                if (spawnBigEnemy)
                {
                    enemy = ObjectPooling.Instance.SpawnFromPool(TPSGameDataModel.PoolObjectType.SmallEnemy, spawnPoint.position, Quaternion.identity);
                }
                else
                {
                    enemy = ObjectPooling.Instance.SpawnFromPool(TPSGameDataModel.PoolObjectType.LargeEnemy, spawnPoint.position, Quaternion.identity);
                }
                // Optionally perform a ground check and adjust position
                AdjustToGround(enemy);
            }
            UIController.Instance.DisplayPopUpMessage("Enemies are ready to attack you!!");
        }

        void AdjustToGround(GameObject enemy)
        {
            RaycastHit hit;
            if (Physics.Raycast(enemy.transform.position, Vector3.down, out hit))
            {
                Vector3 newPosition = hit.point;
                newPosition.y += enemy.GetComponent<Collider>().bounds.extents.y;
                enemy.transform.position = newPosition;
            }
        }
        public void EnemyDefeated(GameObject enemy, TPSGameDataModel.PoolObjectType enemyType)
        {
            enemiesRemaining--;
           // poolManager.ReturnToPool(enemy, isBigEnemy);
           enemy.SetActive(false);

            if (enemiesRemaining <= 0)
            {
                Invoke(nameof(StartNextWave), 5f); // Wait 5 seconds before starting the next wave
            }
        }

        private void HandleGameStateChanged(GameState newGameState)
        {
            if (newGameState == GameState.GameOver)
            {
                DisableActiveEnemies();
                Debug.Log("Disable The enemies");
            }
        }

        private void DisableActiveEnemies()
        {
            var activeLargeEnemies = GameObject.FindGameObjectsWithTag("LargeEnemy");
            foreach (var enemy in activeLargeEnemies)
            {
                enemy.SetActive(false);
            }

            var activeSmallEnemies = GameObject.FindGameObjectsWithTag("SmallEnemy");
            foreach (var enemy in activeSmallEnemies)
            {
                enemy.SetActive(false);
            }
        }
        #endregion
    }
}

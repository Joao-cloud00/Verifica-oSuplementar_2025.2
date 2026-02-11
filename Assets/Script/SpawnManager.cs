using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public List<GameObject> enemyPrefab;

    [Header("Configurações de Spawn")]
    public float EnemyQtd = 50;

    [Tooltip("Raio de espaço que o inimigo precisa para spawnar")]
    public float checkRadius = 1f;

    [Tooltip("Camada que será considerada obstáculo (ex: outros inimigos, paredes)")]
    public LayerMask collisionLayer;

    [Tooltip("Quantas vezes tentar achar uma posição livre antes de desistir de um inimigo")]
    public int maxSpawnAttempts = 10;

    [Header("Área de Spawn")]
    public Vector3 spawnAreaSize = new Vector3(5f, 0f, 5f);

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < EnemyQtd; i++)
        {

            Vector3 spawnPosition = Vector3.zero;
            bool validPositionFound = false;
            int attempts = 0;

            while (!validPositionFound && attempts < maxSpawnAttempts)
            {
                attempts++;

                Vector3 randomPoint = transform.position + new Vector3(
                    Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                    0,
                    Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
                );

                if (!Physics.CheckSphere(randomPoint, checkRadius, collisionLayer))
                {
                    spawnPosition = randomPoint;
                    validPositionFound = true;
                }
            }

            if (validPositionFound)
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count)], spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Não foi possível encontrar espaço para spawnar o inimigo " + i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
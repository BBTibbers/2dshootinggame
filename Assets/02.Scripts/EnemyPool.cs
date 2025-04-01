using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static Enemy;


public class EnemyPool : MonoBehaviour
{


    public List<Enemy> EnemyPrefabs;

    public int PoolSize = 20;

    private List<Enemy> _enemys;

    public static EnemyPool Instance;

    private void Awake()

    {
        //하나임을 보장하는 코드로 변경
        Instance = this;

        // 풀 크기를 정하고
        int enemyPrefabsCount = EnemyPrefabs.Count;
        _enemys = new List<Enemy>(PoolSize * enemyPrefabsCount);

        foreach (Enemy enemyPrefab in EnemyPrefabs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);

                _enemys.Add(enemy);

                enemy.transform.SetParent(this.transform);

                enemy.gameObject.SetActive(false);
            }
        }
    }

    public Enemy Create(EnemyType enemyType, Vector3 position)
    {
        foreach (Enemy enemy in _enemys)
        {
            if (enemy.enemyData.EnemyType == enemyType && enemy.gameObject.activeInHierarchy == false)
            {
                enemy.transform.position = position;

                enemy.Initialize();

                enemy.gameObject.SetActive(true);

                return enemy;
            }
        }

        return null;
    }
}

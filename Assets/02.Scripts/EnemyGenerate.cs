using UnityEngine;
using static Enemy;

public class EnemyGenerate : MonoBehaviour
{

    public GameObject NewEnemy;
    public float Cooltime = 1f;
    public float OriginCooltime = 1f;
    private float _nextSpawn = 0;
    private const float SPAWN_RANGE = 1f;

    public bool isSpawning = true;

    public enum GeneratorType
    {
        Virt,
        Horizon
    }
    public GeneratorType generatorType;
    // Update is called once per frame
    void Update()
    {
        if(isSpawning)
            Spawn();
    }

private void Spawn()
{

    if (Time.time < _nextSpawn)
    {
        return;
    }
        // 총알 생성
        EnemyType enemyType;
    if (generatorType == GeneratorType.Virt)
    {//프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
        float rand = Random.Range(0f, 1f);
        if (rand > 0.7f)
            enemyType = Enemy.EnemyType.Target;
        else if (rand > 0.2f)
        {
            enemyType = Enemy.EnemyType.Normal;
        }
        else
        {
            enemyType = Enemy.EnemyType.Follow;
        }
    }
    else
    {

        enemyType = Enemy.EnemyType.Horizon;
        }
    
    Enemy newEnemy = EnemyPool.Instance.Create(enemyType, transform.position);
    newEnemy.transform.position = this.transform.position; // 총알 위치 설정
    _nextSpawn = Time.time + Cooltime + Random.Range(-SPAWN_RANGE,SPAWN_RANGE);
    }
}

using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static BulletDataSO;
using static Bullet;


public class BulletPool : MonoBehaviour
{


    public List<Bullet> BulletPrefabs;

    public int PoolSize = 20;

    private List<Bullet> _bullets;

    public static BulletPool Instance;

    private void Awake()
    {
        //하나임을 보장하는 코드로 변경
        Instance = this;

        // 풀 크기를 정하고
        int bulletPrefabsCount = BulletPrefabs.Count;
        _bullets = new List<Bullet>(PoolSize * bulletPrefabsCount);

        foreach (Bullet bulletPrefab in BulletPrefabs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab);

                _bullets.Add(bullet);

                bullet.transform.SetParent(this.transform);

                bullet.gameObject.SetActive(false);
            }
        }
    }

    public Bullet Create(BulletType bulletType, Vector3 position)
    {
        foreach (Bullet bullet in _bullets)
        {
            if (bullet.BulletDataSO.bulletType == bulletType && bullet.gameObject.activeInHierarchy == false)
            {
                bullet.transform.position = position;

                bullet.Initialize();

                bullet.gameObject.SetActive(true);

                return bullet;
            }
        }

        return null;
    }
}

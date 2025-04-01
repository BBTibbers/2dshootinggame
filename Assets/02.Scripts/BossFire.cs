using UnityEngine;

public class BossMove : MonoBehaviour
{
    public GameObject Muzzle;
    public GameObject EnemyBullet;
    public float Cooltime = 1f;
    private float _nextFire = 0;

    void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (Time.time < _nextFire)
        {
            return;
        }
        // 총알 생성
        GameObject bulletLeft = Instantiate(EnemyBullet); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
        bulletLeft.transform.position = Muzzle.transform.position; // 총알 위치 설정

        _nextFire = Time.time + Cooltime;
    }
}


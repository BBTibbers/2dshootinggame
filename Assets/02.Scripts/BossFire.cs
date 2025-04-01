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
        // �Ѿ� ����
        GameObject bulletLeft = Instantiate(EnemyBullet); //�������� �ν��Ͻ�ȭ, ���ӿ�����Ʈ�� ������ ���� ���� �ִ´�.
        bulletLeft.transform.position = Muzzle.transform.position; // �Ѿ� ��ġ ����

        _nextFire = Time.time + Cooltime;
    }
}


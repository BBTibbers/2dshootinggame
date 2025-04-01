using System.Security.Cryptography;
using UnityEngine;

public class Pet : MonoBehaviour
{
    private GameObject _player;
    private Vector2 _lookAt = new Vector2(0, 0);
    private Vector2 _direction = new Vector2(0, 0);
    private const float CURVE = 50f;
    public float Speed;
    private float _nextFire = 0;
    public float Cooltime;
    public GameObject BulletPrefab;
    public int BulletCount=5;
    public int BulletCountNow = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {

        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player"); // ������ Player �ڵ����� ã��
            if (_player == null)
            {
                Debug.LogError("������ 'Player' ������Ʈ�� ã�� �� �����ϴ�!");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveFollow();
        PetFire();
    }

    private void MoveFollow()
    {
        Vector3 offset = new Vector3(1, 1, 0);
        _lookAt = (_player.transform.position-(offset)) - transform.position;
        _lookAt.Normalize();
        if (_direction.x == 0 && _direction.y == 0)
        {
            _direction = _lookAt;
        }

        _direction = _direction * (1 - CURVE * Time.deltaTime) + _lookAt * (CURVE * Time.deltaTime);
        _direction.Normalize();
        float speed = Speed * Vector2.Distance((_player.transform.position - (offset)), transform.position);

        transform.Translate(_direction * speed * Time.deltaTime);
    }

    private void PetFire()
    {
        if (Time.time < _nextFire)
        {
            return;
        }
        if (BulletCountNow == 0)
            Cooltime /= (BulletCount*10);
        if(BulletCountNow == BulletCount)
        {
            BulletCountNow = 0;
            Cooltime *= (BulletCount*10);
        }
        else
        {
            BulletCountNow++;
        }
            // �Ѿ� ����
            GameObject bulletLeft = Instantiate(BulletPrefab); //�������� �ν��Ͻ�ȭ, ���ӿ�����Ʈ�� ������ ���� ���� �ִ´�.
        bulletLeft.transform.position = transform.position; // �Ѿ� ��ġ ����

        _nextFire = Time.time + Cooltime;
    }
}


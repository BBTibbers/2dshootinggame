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
            _player = GameObject.FindWithTag("Player"); // 씬에서 Player 자동으로 찾기
            if (_player == null)
            {
                Debug.LogError("씬에서 'Player' 오브젝트를 찾을 수 없습니다!");
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
            // 총알 생성
            GameObject bulletLeft = Instantiate(BulletPrefab); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
        bulletLeft.transform.position = transform.position; // 총알 위치 설정

        _nextFire = Time.time + Cooltime;
    }
}


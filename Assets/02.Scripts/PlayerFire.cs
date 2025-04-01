using UnityEngine;
using static Bullet;

public class PlayerFire : MonoBehaviour
{

    // 목표 : 총알 만들어서 발사하기

    // 필요 속성  : 총알 프리팹, 총알 발사 위치, 총알 발사 속도
    public GameObject BulletPrefab;
    public GameObject MuzzleLeft;
    public GameObject MuzzleRight;
    public float Cooltime;
    private float _nextFire = 0;
    private bool _isFIre = true;
    private bool _isAutoAttack = true;
    // 필요 기능 : 발사
    void Update()
    {
        GetKey();

        if(_isFIre)
        {
            Fire();
        }
    }

    public void SetAutoAttack(bool set)
    {
        _isAutoAttack = set;
        _isFIre = set;
    }
    private void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _isAutoAttack = true;
            _isFIre = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _isAutoAttack = false;
            _isFIre = false;
        }
        if (_isAutoAttack == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isFIre = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isFIre = false;
            }
        }
    }
    private void Fire()
    {

        if(Time.time < _nextFire)
        {
            return;
        }
        // 총알 생성
        /*GameObject bulletLeft = Instantiate(BulletPrefab); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
        bulletLeft.transform.position = MuzzleLeft.transform.position; // 총알 위치 설정
        GameObject bulletRight = Instantiate(BulletPrefab); //프리팹의 인스턴스화, 게임오브젝트를 복제해 씬에 새로 넣는다.
        bulletRight.transform.position = MuzzleRight.transform.position; // 총알 위치 설정
       */
        BulletPool.Instance.Create(BulletPrefab.GetComponent<Bullet>().BulletDataSO.bulletType, MuzzleLeft.transform.position);
        BulletPool.Instance.Create(BulletPrefab.GetComponent<Bullet>().BulletDataSO.bulletType, MuzzleRight.transform.position);
        _nextFire = Time.time + Cooltime;
    
    }

}

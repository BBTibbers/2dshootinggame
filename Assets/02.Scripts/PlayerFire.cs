using UnityEngine;
using static Bullet;

public class PlayerFire : MonoBehaviour
{

    // ��ǥ : �Ѿ� ���� �߻��ϱ�

    // �ʿ� �Ӽ�  : �Ѿ� ������, �Ѿ� �߻� ��ġ, �Ѿ� �߻� �ӵ�
    public GameObject BulletPrefab;
    public GameObject MuzzleLeft;
    public GameObject MuzzleRight;
    public float Cooltime;
    private float _nextFire = 0;
    private bool _isFIre = true;
    private bool _isAutoAttack = true;
    // �ʿ� ��� : �߻�
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
        // �Ѿ� ����
        /*GameObject bulletLeft = Instantiate(BulletPrefab); //�������� �ν��Ͻ�ȭ, ���ӿ�����Ʈ�� ������ ���� ���� �ִ´�.
        bulletLeft.transform.position = MuzzleLeft.transform.position; // �Ѿ� ��ġ ����
        GameObject bulletRight = Instantiate(BulletPrefab); //�������� �ν��Ͻ�ȭ, ���ӿ�����Ʈ�� ������ ���� ���� �ִ´�.
        bulletRight.transform.position = MuzzleRight.transform.position; // �Ѿ� ��ġ ����
       */
        BulletPool.Instance.Create(BulletPrefab.GetComponent<Bullet>().BulletDataSO.bulletType, MuzzleLeft.transform.position);
        BulletPool.Instance.Create(BulletPrefab.GetComponent<Bullet>().BulletDataSO.bulletType, MuzzleRight.transform.position);
        _nextFire = Time.time + Cooltime;
    
    }

}

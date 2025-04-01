using UnityEngine;
using static Unity.Mathematics.math;

public class Bullet : MonoBehaviour
{
    public BulletDataSO BulletDataSO;

    private void Start()
    {

    }

    public void Initialize()
    {
        // 초기화 코드가 들어갈 예정이다.
        // Health = 100;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = Vector2.up; // 방향 벡터 생성
        transform.Translate(direction * BulletDataSO.Speed * Time.deltaTime); 
    }

    private void OnTriggerEnter2D(Collider2D other) // stay, exit도 있음
    {
        if (other.CompareTag("Enemy"))
        {

            other.GetComponent<Enemy>().takeDamage(new Damage(Damage.DamageType.Bullet, (int)StatManager.Instance.Stats[(int)StatType.Damage].Value));
            this.gameObject.SetActive(false);

        }
    }
}

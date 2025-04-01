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
        // �ʱ�ȭ �ڵ尡 �� �����̴�.
        // Health = 100;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = Vector2.up; // ���� ���� ����
        transform.Translate(direction * BulletDataSO.Speed * Time.deltaTime); 
    }

    private void OnTriggerEnter2D(Collider2D other) // stay, exit�� ����
    {
        if (other.CompareTag("Enemy"))
        {

            other.GetComponent<Enemy>().takeDamage(new Damage(Damage.DamageType.Bullet, (int)StatManager.Instance.Stats[(int)StatType.Damage].Value));
            this.gameObject.SetActive(false);

        }
    }
}

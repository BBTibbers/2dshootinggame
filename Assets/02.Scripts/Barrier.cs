using UnityEngine;

public class Barrier : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) // stay, exit�� ����

    {
        if (other.CompareTag("Enemy"))
        {

            other.gameObject.GetComponent<Enemy>().takeDamage(new Damage(Damage.DamageType.Boom,999));
        }
    }
}

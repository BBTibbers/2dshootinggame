using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float Speed;


    // Update is called once per frame
    void Update()
    {

        Vector2 direction = Vector2.down; // ���� ���� ����
        transform.Translate(direction * Speed * Time.deltaTime);

    }
}

using UnityEngine;

public class Destroier : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) // stay, exit�� ����
    { 

            other.gameObject.SetActive(false);

    }
    
}

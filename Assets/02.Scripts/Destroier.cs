using UnityEngine;

public class Destroier : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) // stay, exitµµ ¿÷¿Ω
    { 

            other.gameObject.SetActive(false);

    }
    
}

using UnityEngine;
using UnityEngine.Rendering;

public class CameraShaking : MonoBehaviour
{

    private bool _isShaking = false;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float time = 0;
    public void Shake(bool isShaking)
    {
        _isShaking = isShaking;
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isShaking)
        {
            float magnitude = 0.1f - (Time.time - time)*0.2f;
            transform.position = new Vector3(Random.Range(-magnitude, magnitude),Random.Range(-magnitude, magnitude),-10);
            if(Time.time - time > 0.5f)
            {
                transform.position = new Vector3(0, 0, -10);
                _isShaking = false;
            }
        }
    }
}

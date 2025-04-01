using UnityEngine;

public class Background : MonoBehaviour
{
    public SpriteRenderer MySpriteRenderer;
    public float ScrollSpeed = 1f;
    private MaterialPropertyBlock _propertyBlock;
    private Vector2 _offset;




    private void Awake()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        _offset.y += ScrollSpeed * Time.deltaTime;

        // 현재 머티리얼 프로퍼티 블록 가져오기
        MySpriteRenderer.GetPropertyBlock(_propertyBlock);

        // "_MainTex_ST"의 z, w 값을 이용해 오프셋 변경 (셰이더에서 `_MainTex_ST`의 z, w가 offset 역할)
        _propertyBlock.SetVector("_MainTex_ST", new Vector4(1, 1, _offset.x, _offset.y));

        // 변경된 PropertyBlock을 다시 설정
        MySpriteRenderer.SetPropertyBlock(_propertyBlock);
    }
}

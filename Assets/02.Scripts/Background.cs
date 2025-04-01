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

        // ���� ��Ƽ���� ������Ƽ ��� ��������
        MySpriteRenderer.GetPropertyBlock(_propertyBlock);

        // "_MainTex_ST"�� z, w ���� �̿��� ������ ���� (���̴����� `_MainTex_ST`�� z, w�� offset ����)
        _propertyBlock.SetVector("_MainTex_ST", new Vector4(1, 1, _offset.x, _offset.y));

        // ����� PropertyBlock�� �ٽ� ����
        MySpriteRenderer.SetPropertyBlock(_propertyBlock);
    }
}

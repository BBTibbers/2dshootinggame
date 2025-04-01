using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "Scriptable Objects/BulletDataSO")]
public class BulletDataSO : ScriptableObject
{
    public enum BulletType
    {
        Normal,
        Semi,
        Pet
    }
    public BulletType bulletType;
    public float Speed;
}

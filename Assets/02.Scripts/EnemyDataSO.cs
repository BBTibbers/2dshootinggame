using UnityEngine;
using static Enemy;

[CreateAssetMenu(fileName ="EnemyDataSO", menuName ="Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject 
{
    public EnemyType EnemyType;
    public float OriginSpeed;
    public int MaxHealth;
}

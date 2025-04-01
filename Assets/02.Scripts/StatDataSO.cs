using UnityEngine;

[CreateAssetMenu(fileName = "StatDataSO", menuName = "Scriptable Objects/StatDataSO")]
public class StatDataSO : ScriptableObject
{
    public float DefaultValue;
    public float UpgradeAddValue;

    public int DefaultCost;
    public float UpgradeAddCost;
}
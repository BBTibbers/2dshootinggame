using UnityEngine;

public class Damage
{
    public int damage;

    public enum DamageType
    {
        Bullet,
        Boom,
    }

    public DamageType damageType;

    public Damage(DamageType damageType, int damage)
    {
        this.damage = damage;
        this.damageType = damageType;
    }
}

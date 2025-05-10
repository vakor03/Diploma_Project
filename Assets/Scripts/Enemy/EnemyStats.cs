using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;

    public float Health => health;
    public float Speed => speed;
    public float Damage => damage;

    public EnemyStats(float health = 100f, float speed = 5f, float damage = 10f)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
    }
} 
using UnityEngine;

public class Dancer : MonoBehaviour
{
    private float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public bool CheckDeath()
    {
        return health<=0;
    }
}

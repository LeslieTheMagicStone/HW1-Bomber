using UnityEngine;

public class DamageField : MonoBehaviour
{
    public bool canDestroyVoxel = false;
    public int damage;
    public float dizzyTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Damageable>().TakeDamage(damage);
            other.GetComponent<PlayerLogic>().SetUnmovable(dizzyTime);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Damageable>().TakeDamage(damage);
            other.GetComponent<MonsterLogic>().SetUnmovable(dizzyTime);
        }

        if (canDestroyVoxel && other.CompareTag("Voxel"))
        {
            other.GetComponent<Damageable>().TakeDamage(damage);
        }
    }
}

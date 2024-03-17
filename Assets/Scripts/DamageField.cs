using UnityEngine;

public class DamageField : MonoBehaviour
{
    public int damage;
    public bool isEnemy;
    public float dizzyTime;

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemy && other.CompareTag("Player"))
        {
            other.GetComponent<Damageable>().TakeDamage(damage);
            other.GetComponent<PlayerLogic>().SetUnmovable(dizzyTime);
        }
    }
}

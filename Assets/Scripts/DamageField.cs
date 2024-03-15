using UnityEngine;

public class DamageField : MonoBehaviour
{
    public int damage;
    public bool isEnemy;
    public float dizzyTime;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1.0f, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, transform.lossyScale.x / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemy && other.CompareTag("Player"))
        {
            other.GetComponent<Damageable>().TakeDamage(damage);
            other.GetComponent<PlayerLogic>().SetUnmovable(dizzyTime);
        }
    }
}

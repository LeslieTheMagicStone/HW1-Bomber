using System.Collections;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 100;
    public float showTextFrequency = 1.0f;
    private int health;
    [SerializeField]
    private Canvas damageTextPrefab;
    [SerializeField]
    private ParticleSystem deathEffectPrefab;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Random.Range(0, 1f) <= showTextFrequency)
        {
            var canvas = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            var tmpro = canvas.GetComponentInChildren<TMP_Text>();
            tmpro.text = damage.ToString();
        }

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}

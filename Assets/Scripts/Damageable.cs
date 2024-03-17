using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent onDeath, onHurt;
    public int maxHealth = 100;
    public float showTextFrequency = 1.0f;
    private int health;
    [SerializeField] private Canvas damageTextPrefab;
    [SerializeField] private ParticleSystem deathEffectPrefab;
    [SerializeField] private AudioClip[] deathAudios;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        onHurt.Invoke();
        if (Random.Range(0, 1f) <= showTextFrequency)
        {
            var canvas = Instantiate(damageTextPrefab, transform.position, damageTextPrefab.transform.rotation);
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
        onDeath.Invoke();
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        }
        if (deathAudios.Length != 0 && Random.Range(0, 5) == 0)
        {
            int randIndex = Random.Range(0, deathAudios.Length);
            AudioManager.instance.Play(deathAudios[randIndex], 0.1f);
        }

        Destroy(gameObject);
    }
}

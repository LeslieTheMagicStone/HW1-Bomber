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

    const float DAMAGE_TEXT_LIFETIME = 2.0f;
    const float DAMAGE_TEXT_ELEVATE_SPEED = 1f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Random.Range(0, 1f) <= showTextFrequency)
            StartCoroutine(ShowDamageTextCoroutine(damage));

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ShowDamageTextCoroutine(int damage)
    {
        // Damage text anim to make the game juicy.
        Canvas damageText = Instantiate(damageTextPrefab);
        damageText.transform.position = transform.position;

        var tmpro = damageText.GetComponentInChildren<TMP_Text>();
        tmpro.text = damage.ToString();
        Destroy(damageText.gameObject, DAMAGE_TEXT_LIFETIME);

        float timer = DAMAGE_TEXT_LIFETIME;
        while (timer > 0)
        {
            float t = timer / DAMAGE_TEXT_LIFETIME;
            Color color = tmpro.faceColor;
            color.a = t;
            tmpro.faceColor = color;
            damageText.transform.Translate(Time.deltaTime * DAMAGE_TEXT_ELEVATE_SPEED * Vector3.up);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 2);
    }
}

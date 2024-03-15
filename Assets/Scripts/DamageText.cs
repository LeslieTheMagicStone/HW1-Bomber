using UnityEngine;
using System.Collections;
using TMPro;

public class DamageText : MonoBehaviour
{
    const float DAMAGE_TEXT_LIFETIME = 1.0f;
    const float DAMAGE_TEXT_ELEVATE_SPEED = 2.0f;

    private IEnumerator Start()
    {
        // Damage text anim to make the game juicy.
        var tmpro = GetComponentInChildren<TMP_Text>();
        Destroy(gameObject, DAMAGE_TEXT_LIFETIME);

        float timer = DAMAGE_TEXT_LIFETIME;
        while (timer > 0)
        {
            float t = timer / DAMAGE_TEXT_LIFETIME;
            Color color = tmpro.faceColor;
            color.a = t;
            tmpro.faceColor = color;
            transform.Translate(Time.deltaTime * DAMAGE_TEXT_ELEVATE_SPEED * Vector3.up);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}

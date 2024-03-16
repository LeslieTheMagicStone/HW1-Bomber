using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool isReady = false;

    const float SPAWN_ANIM_TIME = 0.3f;
    const float EXPLODE_RADIUS = 4f;
    const float EXPLODE_TIME = 2f;
    const int DAMAGE = 50;

    [SerializeField]
    private BombImpact impactPrefab;
    [SerializeField]
    private ParticleSystem boom;

    private IEnumerator Start()
    {
        // Spawn anim: grow big.
        float timer = 0;
        Vector3 maxScale = transform.localScale;

        while (timer < SPAWN_ANIM_TIME)
        {
            timer += Time.deltaTime;
            var t = timer / SPAWN_ANIM_TIME;
            transform.localScale = maxScale * t;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = maxScale;
        isReady = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1f, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, EXPLODE_RADIUS);
    }

    public void Fire()
    {
        if (!isReady) return;
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(EXPLODE_TIME);
        Explode();
    }

    protected virtual void Explode()
    {
        var damageColliders = Physics.OverlapSphere(transform.position, EXPLODE_RADIUS);

        // Handle damage behavior.
        foreach (var collider in damageColliders)
        {
            if (collider.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(DAMAGE);
            }
        }

        BombImpact impact = Instantiate(impactPrefab, null);
        impact.transform.position = transform.position;

        // Boom animation.
        boom.transform.SetParent(null);
        boom.Play();
        Destroy(boom.gameObject, 10f);

        Destroy(gameObject);
    }
}

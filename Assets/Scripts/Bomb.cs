using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool isReady = false;

    const float SPAWN_ANIM_TIME = 0.3f;
    const float EXPLODE_RADIUS = 4f;
    const float EXPLODE_TIME = 1.5f;
    const int DAMAGE = 50;
    protected float explodeTimer = EXPLODE_TIME;
    protected bool isFired = false;

    [SerializeField]
    protected BombImpact impactPrefab;
    [SerializeField]
    protected ParticleSystem boom;
    [SerializeField]
    protected AudioClip[] boomAudios;
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

    private void Update()
    {
        if (isFired)
        {
            explodeTimer -= Time.deltaTime;
        }
        if (explodeTimer <= 0f)
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1f, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, EXPLODE_RADIUS);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Voxel")) return;
        if (isFired && explodeTimer / EXPLODE_TIME <= 0.95f)
        {
            explodeTimer = 0.01f;
        }
    }

    public void Fire()
    {
        if (!isReady) return;
        isFired = true;
    }

    public virtual void Explode()
    {
        var damageColliders = Physics.OverlapSphere(transform.position, EXPLODE_RADIUS);

        // Handle damage behavior.
        // foreach (var collider in damageColliders)
        // {
        //     if (collider.TryGetComponent(out Damageable damageable))
        //     {
        //         damageable.TakeDamage(DAMAGE);
        //     }
        // }

        BombImpact impact = Instantiate(impactPrefab, null);
        var damageField = impact.AddComponent<DamageField>();
        damageField.damage = DAMAGE;
        damageField.canDestroyVoxel = true;
        Destroy(damageField, 0.5f * impact.duration);
        impact.transform.position = transform.position;

        // Boom animation.
        boom.transform.SetParent(null);
        boom.Play();
        Destroy(boom.gameObject, 10f);

        int randIndex = Random.Range(0, boomAudios.Length);
        AudioManager.instance.Play(boomAudios[randIndex], 0.1f);

        Destroy(gameObject);
    }
}

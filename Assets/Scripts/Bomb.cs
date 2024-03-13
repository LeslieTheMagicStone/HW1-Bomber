using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    const float SPAWN_ANIM_TIME = 0.5f;
    const float EXPLODE_RADIUS = 4f;

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1f, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, EXPLODE_RADIUS);
    }

    public void Fire()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(4f);
        Explode();
    }

    private void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, EXPLODE_RADIUS);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Voxel"))
            {
                Destroy(collider.gameObject);
            }
        }

        Destroy(gameObject);
    }
}

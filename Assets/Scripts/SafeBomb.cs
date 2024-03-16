using UnityEngine;

class SafeBomb : Bomb
{
    protected override void Explode()
    {
        BombImpact impact = Instantiate(impactPrefab, null);
        impact.transform.position = transform.position;

        // Boom animation.
        boom.transform.SetParent(null);
        boom.Play();
        Destroy(boom.gameObject, 10f);

        Destroy(gameObject);
    }
}
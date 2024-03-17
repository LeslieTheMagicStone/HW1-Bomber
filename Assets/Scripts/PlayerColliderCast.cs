using UnityEngine;

public class PlayerColliderCast : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(player.position, Camera.main.transform.position);
        var scale = transform.localScale;
        scale.z = distance;
        transform.localScale = scale;
        transform.position = (player.position + Camera.main.transform.position) / 2;
        transform.LookAt(player);
        transform.Translate(new(0, scale.y / 2, 0), Space.Self);
    }
}

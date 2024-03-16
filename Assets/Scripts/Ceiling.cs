using UnityEngine;

public class Ceiling : MonoBehaviour
{
    private Material originMaterial;
    private new Renderer renderer;
    private GameObject player;

    public float warningRange;
    public float highestWarningDistance;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
            return;

        float distanceY = Mathf.Abs(transform.position.y - player.transform.position.y);
        if (distanceY <= highestWarningDistance)
        {
            SetTransparency(originMaterial.color.a);
            return;
        }

        float scaler = 1 - Mathf.Abs(distanceY - highestWarningDistance) / (warningRange - highestWarningDistance);
        if (scaler > 0)
        {
            SetTransparency(scaler * originMaterial.color.a);
        }
        else
        {
            SetTransparency(0);
        }
    }

    public void SetTransparency(float a)
    {
        Material newMaterial = new(originMaterial);

        Color color = newMaterial.color;
        color.a = a;
        newMaterial.color = color;

        renderer.material = newMaterial;
    }
}

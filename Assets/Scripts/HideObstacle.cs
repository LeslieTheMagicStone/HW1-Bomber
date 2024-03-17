using UnityEngine;

public class HideObstacle : MonoBehaviour
{
    private Material originMaterial;
    [SerializeField] private Material transParentMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originMaterial = meshRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColliderCast"))
        {
            meshRenderer.material = transParentMaterial;
            // Material mat = new(originMaterial);
            // var color = mat.color;
            // color.a = 0f;
            // mat.color = color;
            // meshRenderer.material = mat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ColliderCast"))
        {
            meshRenderer.material = originMaterial;
        }
    }

}

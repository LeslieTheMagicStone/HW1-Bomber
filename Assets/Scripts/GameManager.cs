using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject voxelPrefab;
    [SerializeField]
    Transform spawnPoint;
    private void Start()
    {
        for (int x = -15; x < 15; x++)
        {
            for (int z = -15; z < 15; z++)
            {
                for (int y = 0; y < 3; y++)
                {
                    GameObject cube = Instantiate(voxelPrefab);
                    cube.transform.SetParent(spawnPoint);
                    cube.transform.localPosition = new(x, y + 1f, z);
                    cube.name = "Cube(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
                }
            }
        }
    }
}

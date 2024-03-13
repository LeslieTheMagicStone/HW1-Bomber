using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject voxelPrefab;
    [SerializeField]
    Transform spawnPoint;
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                for (int k = 0; k < 10; k++)
                {
                    GameObject cube = Instantiate(voxelPrefab);
                    cube.transform.SetParent(spawnPoint);
                    cube.transform.localPosition = new(i, k + 1f, j);
                    cube.name = "Cube(" + i.ToString() + "," + k.ToString() + "," + k.ToString() + ")";
                }
            }
        }
    }
}

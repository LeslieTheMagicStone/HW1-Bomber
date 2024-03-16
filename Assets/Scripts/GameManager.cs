using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int xSize, zSize, ySize;
    [SerializeField]
    GameObject voxelPrefab, wallPrefab;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    MonsterLogic monsterPrefab;

    const float SPAWN_INVERVAL_CONVERGENCE_RATE = 0.005f;
    const float MAX_SPAWN_MONSTER_INTERVAL = 3f;
    private float spawnMonsterInterval = MAX_SPAWN_MONSTER_INTERVAL;
    private float spawnMonsterTimer = MAX_SPAWN_MONSTER_INTERVAL;

    private void Start()
    {
        for (int x = -xSize; x <= xSize; x++)
        {
            for (int z = -zSize; z <= zSize; z++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    GameObject cube = Instantiate(voxelPrefab);
                    cube.transform.SetParent(spawnPoint);
                    cube.transform.localPosition = new(x, y + 1f, z);
                    cube.name = "Cube(" + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
                }
            }
        }

        var leftWall = Instantiate(wallPrefab, new Vector3(-xSize - 2.5f, 0, 0), Quaternion.identity);
        leftWall.transform.localScale = new(4f, 5 * ySize, 3 * zSize);
        var rightWall = Instantiate(wallPrefab, new Vector3(xSize + 2.5f, 0, 0), Quaternion.identity);
        rightWall.transform.localScale = new(4f, 5 * ySize, 3 * zSize);
        var forwardWall = Instantiate(wallPrefab, new Vector3(0, 0, -zSize - 2.5f), Quaternion.identity);
        forwardWall.transform.localScale = new(3 * xSize, 5 * ySize, 4f);
        var backWall = Instantiate(wallPrefab, new Vector3(0, 0, zSize + 2.5f), Quaternion.identity);
        backWall.transform.localScale = new(3 * xSize, 5 * ySize, 4f);
    }

    private void Update()
    {
        spawnMonsterInterval = MAX_SPAWN_MONSTER_INTERVAL * Mathf.Exp(-SPAWN_INVERVAL_CONVERGENCE_RATE * Time.time);

        if (spawnMonsterTimer <= 0f)
        {
            var monster = Instantiate(monsterPrefab);
            Vector3 randomPos = new(Random.Range(-15f, 15f), 10f, Random.Range(-15f, 15f));
            monster.transform.position = randomPos;
            spawnMonsterTimer = spawnMonsterInterval;
        }
        spawnMonsterTimer -= Time.deltaTime;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject voxelPrefab;
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

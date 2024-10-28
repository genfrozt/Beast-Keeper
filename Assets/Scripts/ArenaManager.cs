using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public Transform playerSpawnPoint;

    void Start()
    {
        if (MonsterDataManager.Instance.HasMonsterData())
        {
            GameObject monsterInstance = Instantiate(MonsterDataManager.Instance.MonsterPrefab, playerSpawnPoint.position, Quaternion.identity);
            MonsterStats monsterInArena = monsterInstance.GetComponent<MonsterStats>();

            if (monsterInArena != null)
            {
                monsterInArena.SetStats(
                    MonsterDataManager.Instance.Health,
                    MonsterDataManager.Instance.Strength,
                    MonsterDataManager.Instance.Speed,
                    MonsterDataManager.Instance.Stamina,
                    MonsterDataManager.Instance.Lifespan
                );
                monsterInstance.name = MonsterDataManager.Instance.MonsterName;
                Debug.Log($"Arena Monster: {monsterInstance.name}");
            }
            else
            {
                Debug.LogError("MonsterStats component is missing on the instantiated prefab.");
            }
        }
        else
        {
            Debug.LogError("No monster data found!");
        }
    }
}

using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public Transform playerSpawnPoint;

    void Start()
    {
        if (MonsterDataManager.Instance.HasMonsterData())
        {
            GameObject monsterInstance = Instantiate(MonsterDataManager.Instance.MonsterPrefab, playerSpawnPoint.position, Quaternion.identity);
            
           
        }
        else
        {
            Debug.LogError("No monster data found!");
        }
    }
}

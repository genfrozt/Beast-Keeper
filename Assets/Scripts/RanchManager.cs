using UnityEngine;
using TMPro;

public class RanchManager : MonoBehaviour
{
    public Transform spawnPosition;  // Set in the Inspector for where the monster should appear
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI monsterNameText;

    void Start()
    {
        if (MonsterTransfer.HasMonsterData())
        {
            // Spawn the monster
            GameObject spawnedMonster = Instantiate(MonsterTransfer.monsterPrefab, spawnPosition.position, Quaternion.identity);

            // Display monster's name and stats
            monsterNameText.text = MonsterTransfer.monsterName;
            monsterStatsText.text = $"Health: {MonsterTransfer.health}\nStrength: {MonsterTransfer.strength}\nSpeed: {MonsterTransfer.speed}";

            Debug.Log("Monster spawned in RanchScene: " + MonsterTransfer.monsterName);
        }
        else
        {
            Debug.LogWarning("No monster data found in MonsterTransfer.");
        }
    }
}

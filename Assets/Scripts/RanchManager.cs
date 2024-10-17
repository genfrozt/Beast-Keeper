using UnityEngine;
using TMPro;

public class RanchManager : MonoBehaviour
{
    public Transform monsterSpawnPoint;  // Where the monster will be instantiated
    public TextMeshProUGUI monsterNameText;  // Text UI for displaying monster's name

    void Start()
    {
        if (MonsterTransfer.instance.monsterPrefab != null)
        {
            // Instantiate the monster from the saved data
            GameObject monster = Instantiate(MonsterTransfer.instance.monsterPrefab, monsterSpawnPoint.position, Quaternion.identity);
            MonsterStats stats = monster.GetComponent<MonsterStats>();

            // Set the monster's stats
            stats.SetStats(MonsterTransfer.instance.health, MonsterTransfer.instance.strength, MonsterTransfer.instance.speed);

            // Display the monster's name
            monsterNameText.text = MonsterTransfer.instance.monsterName;
        }
    }
}

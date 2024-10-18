using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RanchManager : MonoBehaviour
{
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI monsterNameText;
    public MonsterStats currentMonster;  // Reference to the current monster's stats script
    public TextMeshProUGUI dayText;      // Display for the current day
    public Transform monsterSpawnPoint;  // Where the monster will be instantiated
    public Button shadowBoxingButton;
    public Button weightLiftingButton;
    public Button joggingButton;

    private int day = 1;

    void Start()
    {
        if (MonsterTransfer.HasMonsterData())
        {
            // Instantiate the transferred monster prefab
            GameObject monsterInstance = Instantiate(MonsterTransfer.monsterPrefab, monsterSpawnPoint.position, Quaternion.identity);
            currentMonster = monsterInstance.GetComponent<MonsterStats>();

            // Set the stats from the MonsterTransfer data
            currentMonster.SetStats(MonsterTransfer.health, MonsterTransfer.strength, MonsterTransfer.speed);

            // Update UI
            monsterNameText.text = MonsterTransfer.monsterName;
            UpdateMonsterStatsUI();
        }
        else
        {
            Debug.LogError("No monster data found!");
        }

        // Set up button listeners for training regimens
        shadowBoxingButton.onClick.AddListener(() => TrainMonster("speed", 2));  // Shadowboxing adds +2 to speed
        weightLiftingButton.onClick.AddListener(() => TrainMonster("strength", 2));  // Weightlifting adds +2 to strength
        joggingButton.onClick.AddListener(() => TrainMonster("health", 2));  // Jogging adds +2 to health

        // Initialize day text
        UpdateDayUI();
    }

    void TrainMonster(string statType, int increaseAmount)
    {
        switch (statType)
        {
            case "speed":
                currentMonster.speed += increaseAmount;
                break;
            case "strength":
                currentMonster.strength += increaseAmount;
                break;
            case "health":
                currentMonster.health += increaseAmount;
                break;
        }

        UpdateMonsterStatsUI();
        AdvanceDay();
    }

    void UpdateMonsterStatsUI()
    {
        monsterStatsText.text = $"Health: {currentMonster.health}\nStrength: {currentMonster.strength}\nSpeed: {currentMonster.speed}";
    }

    void AdvanceDay()
    {
        day++;
        UpdateDayUI();
    }

    void UpdateDayUI()
    {
        dayText.text = $"Day: {day}";
    }
}

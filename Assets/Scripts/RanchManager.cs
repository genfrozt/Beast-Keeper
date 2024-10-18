using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RanchManager : MonoBehaviour
{
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI monsterNameText;
    public TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI moodText;  // Display for the monster's mood
    public MonsterStats currentMonster;
    public TextMeshProUGUI dayText;
    public Transform monsterSpawnPoint;
    public Button shadowBoxingButton;
    public Button weightLiftingButton;
    public Button joggingButton;
    public Button restButton;  // Rest button to reset stamina
    public Button yesButton;
    public Button noButton;

    private int day = 1;
    private int year = 1000;
    private string selectedTrainingType = "";
    private int trainingIncreaseAmount = 0;

    // Mood and related variables
    private string monsterMood = "Relaxed";  // Default mood
    private int restCounter = 0;  // Tracks consecutive rest days with stamina >= 50
    private int consecutiveTrainingDays = 0;

    void Start()
    {
        if (MonsterTransfer.HasMonsterData())
        {
            GameObject monsterInstance = Instantiate(MonsterTransfer.monsterPrefab, monsterSpawnPoint.position, Quaternion.identity);
            currentMonster = monsterInstance.GetComponent<MonsterStats>();

            currentMonster.SetStats(MonsterTransfer.health, MonsterTransfer.strength, MonsterTransfer.speed, MonsterTransfer.stamina, MonsterTransfer.lifespan);
            monsterNameText.text = MonsterTransfer.monsterName;
            UpdateMonsterStatsUI();
            currentMonster.name = MonsterTransfer.monsterName;
        }
        else
        {
            Debug.LogError("No monster data found!");
        }

        shadowBoxingButton.onClick.AddListener(() => PrepareTraining("speed", 2, "Shadowboxing can increase monster speed. Are you sure you want to start training?"));
        weightLiftingButton.onClick.AddListener(() => PrepareTraining("strength", 2, "Weightlifting can increase monster strength. Are you sure you want to start training?"));
        joggingButton.onClick.AddListener(() => PrepareTraining("health", 2, "Jogging can increase monster health. Are you sure you want to start training?"));

        restButton.onClick.AddListener(RestMonster);  // Assign Rest button functionality
        yesButton.onClick.AddListener(ConfirmTraining);
        noButton.onClick.AddListener(CancelTraining);

        HideConfirmationButtons();
        UpdateDayUI();
        UpdateMoodUI();
    }

    void PrepareTraining(string statType, int increaseAmount, string message)
    {
        if (currentMonster.stamina == 25)
        {
            npcDialogueText.text = $"{currentMonster.name} is already tired. Are you sure you want to continue training?";
            ShowConfirmationButtons();
            selectedTrainingType = statType;
            trainingIncreaseAmount = increaseAmount;
        }
        else if (currentMonster.stamina <= 0)
        {
            npcDialogueText.text = $"{currentMonster.name} is too tired and needs a break!";
            return;
        }
        else
        {
            selectedTrainingType = statType;
            trainingIncreaseAmount = increaseAmount;
            npcDialogueText.text = message;
            ShowConfirmationButtons();
        }
    }

    void ConfirmTraining()
    {
        if (currentMonster.stamina <= 0)
        {
            npcDialogueText.text = $"{currentMonster.name} cannot train anymore. Please give the monster a break.";
        }
        else
        {
            TrainMonster();
            npcDialogueText.text = $"{currentMonster.name} completed training and increased its {selectedTrainingType} by {trainingIncreaseAmount}!";
        }
        HideConfirmationButtons();
    }

    void CancelTraining()
    {
        npcDialogueText.text = "Training canceled.";
        HideConfirmationButtons();
    }

    void TrainMonster()
    {
        consecutiveTrainingDays++;
        restCounter = 0;  // Reset rest counter on training

        switch (selectedTrainingType)
        {
            case "speed":
                currentMonster.speed += AdjustTrainingGain(trainingIncreaseAmount);
                break;
            case "strength":
                currentMonster.strength += AdjustTrainingGain(trainingIncreaseAmount);
                break;
            case "health":
                currentMonster.health += AdjustTrainingGain(trainingIncreaseAmount);
                break;
        }

        currentMonster.stamina -= 25;

        // Check if the monster becomes Overworked
        if (consecutiveTrainingDays >= 3 && currentMonster.stamina <= 0)
        {
            SetMonsterMood("Overworked");
        }

        UpdateMonsterStatsUI();
    }

    // Adjusts training gains based on mood
    int AdjustTrainingGain(int baseAmount)
    {
        if (monsterMood == "Eager")
        {
            return Mathf.CeilToInt(baseAmount * 1.5f);  // 50% boost in eager mood
        }
        else if (monsterMood == "Overworked")
        {
            return Mathf.FloorToInt(baseAmount * 0.5f);  // 50% reduction in overworked mood
        }
        return baseAmount;  // Default training gain in Relaxed mood
    }

    void AdvanceDay()
    {
        day++;
        if (day > 360)
        {
            day = 1;
            year++;
        }
        UpdateDayUI();
    }

    public void RestMonster()
    {
        consecutiveTrainingDays = 0;  // Reset training days
        currentMonster.stamina = 100;  // Rest restores stamina to 100

        // Increment rest counter if stamina is 50 or more
        if (currentMonster.stamina >= 50)
        {
            restCounter++;
            if (restCounter >= 3)
            {
                SetMonsterMood("Eager");
            }
        }
        // Reset rest counter to 1 if stamina is 25 or below
        else if (currentMonster.stamina <= 25)
        {
            restCounter = 1;
            SetMonsterMood("Relaxed");
        }

        AdvanceDay();
        UpdateMonsterStatsUI();
    }

    void SetMonsterMood(string newMood)
    {
        monsterMood = newMood;
        UpdateMoodUI();
    }

    void UpdateMonsterStatsUI()
    {
        monsterStatsText.text = $"Health: {currentMonster.health}\nStrength: {currentMonster.strength}\nSpeed: {currentMonster.speed}\nStamina: {currentMonster.stamina}\nLifespan: {currentMonster.lifespan}";
        moodText.text = monsterMood;
    }

    void UpdateMoodUI()
    {
        moodText.text = $"Mood: {monsterMood}";
    }

    void UpdateDayUI()
    {
        dayText.text = $"Year: {year}, Day: {day}";
    }

    void ShowConfirmationButtons()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    void HideConfirmationButtons()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}

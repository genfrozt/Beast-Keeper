using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RanchManager : MonoBehaviour
{
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI monsterNameText;
    public TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI moodText;  // Display for the monster's mood
    public MonsterStats currentMonster;
    public Transform monsterSpawnPoint;
    public Button shadowBoxingButton;
    public Button weightLiftingButton;
    public Button joggingButton;
    public Button restButton;  // Rest button to reset stamina
    public Button yesButton;
    public Button noButton;
    public CalendarManager calendarManager;

    private string selectedTrainingType = "";
    private int trainingIncreaseAmount = 0;
    

    // Mood and related variables
    private string monsterMood = "Relaxed";  // Default mood
    private int restCounter = 0;  // Tracks consecutive rest days with stamina >= 50
    private int overworkedCounter = 0;


    public Button foodButton;  // Button to open the food menu
    public GameObject foodMenu;  // Food menu window to show available food
    public Button strengthFoodButton;
    public Button agilityFoodButton;
    public Button healthFoodButton;
    public Button staminaFoodButton;

    private bool hasFedToday = false;  // Tracks if the monster has been fed today
    private string selectedFoodType = "";  // Tracks the selected food

    private bool isTrainingAction = false;
    private bool isFeedingAction = false;

    private int gold = 1000;  // Initial gold at the start of the game
    private readonly int foodCost = 100;

    void Start()
    {
        if (calendarManager == null)
        {
            calendarManager = FindObjectOfType<CalendarManager>();
        }

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

        foodMenu.SetActive(false);
        //Training System
        shadowBoxingButton.onClick.AddListener(() => PrepareTraining("speed", 2, "Shadowboxing can increase monster speed. Are you sure you want to start training?"));
        weightLiftingButton.onClick.AddListener(() => PrepareTraining("strength", 2, "Weightlifting can increase monster strength. Are you sure you want to start training?"));
        joggingButton.onClick.AddListener(() => PrepareTraining("health", 2 , "Jogging can increase monster health. Are you sure you want to start training?"));

        //Food System
        foodButton.onClick.AddListener(OpenFoodMenu);
        strengthFoodButton.onClick.AddListener(() => SelectFood("strength"));
        agilityFoodButton.onClick.AddListener(() => SelectFood("agility"));
        healthFoodButton.onClick.AddListener(() => SelectFood("health"));
        staminaFoodButton.onClick.AddListener(() => SelectFood("stamina"));

        restButton.onClick.AddListener(RestMonster);  // Assign Rest button functionality
        yesButton.onClick.AddListener(ConfirmAction);
        noButton.onClick.AddListener(ConfirmCancel);
        HideFoodMenu();
        
        HideConfirmationButtons();
        UpdateMoodUI();
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        goldText.text = $"Gold: {gold}g";
    }

    void PrepareTraining(string statType, int increaseAmount, string message)
    {
        if (currentMonster.stamina == 25)
        {
            npcDialogueText.text = $"{currentMonster.name} is already tired. Are you sure you want to continue training?";
            ShowConfirmationButtons();
            isTrainingAction = true;   // Indicate this is a training action
            isFeedingAction = false;
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
            isTrainingAction = true;   // Indicate this is a training action
            isFeedingAction = false;
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

   

    public void RestMonster()
    {

        hasFedToday = false;  // Reset the feeding flag
       
        // Increment rest counter if stamina is 50 or more
        if (currentMonster.stamina >= 50)
        {
            SetMonsterMood("Relaxed");
            restCounter++;
            if (overworkedCounter >= 3)
            {
                overworkedCounter = 0;
                SetMonsterMood("Relaxed");
            
            }
            if (restCounter >= 3)
            {
                overworkedCounter = 0;
                SetMonsterMood("Eager");
            }
        }
        // Reset rest counter to 1 if stamina is 25 or below
        else if (currentMonster.stamina == 25)
        {
            
            restCounter = 0;
            overworkedCounter = 0;
            
            SetMonsterMood("Relaxed");
        }
        else if (currentMonster.stamina <= 0)
        {
            overworkedCounter++;
            if (restCounter >= 3)
            {
                restCounter = 0;
                SetMonsterMood("Relaxed");

            }
            if (overworkedCounter >= 3)
            {
                restCounter = 0;
                SetMonsterMood("Overworked");
            }
        }

        calendarManager.AdvanceDay();
        if (monsterMood == "Overworked")
        {
            currentMonster.lifespan-=2;
        }
        else
        {
            currentMonster.lifespan--;
        }
      
        currentMonster.stamina = 100;
        UpdateMonsterStatsUI();
    }


    void OpenFoodMenu()
    {
        if (hasFedToday)
        {
            npcDialogueText.text = $"{currentMonster.name} has already been fed today!";
            return;
        }

        foodMenu.SetActive(true);
        npcDialogueText.text = "Please select a food to feed your monster.";
    }


    // Select food type
    void SelectFood(string foodType)
    {

        selectedFoodType = foodType;
        string foodDescription = "";
       
            switch (foodType)
            {
                case "strength":
                    foodDescription = "Meat will increase Strength by 1 and restore 25 stamina.";
                    break;
                case "agility":
                    foodDescription = "Milk will increase Agility by 1 and restore 25 stamina.";
                    break;
                case "health":
                    foodDescription = "Bread will increase Health by 1 and restore 25 stamina.";
                    break;
                case "stamina":
                    foodDescription = "Energy Bar will restore 25 stamina and make the monster Eager.";
                    break;
            }
        
        
        npcDialogueText.text = foodDescription + " Do you want to feed this to your monster?";
        isFeedingAction = true;
        isTrainingAction = false;
        ShowConfirmationButtons();
    }

    // Confirm food selection and apply effects

    void ApplyFoodEffect()
    {
        if (gold >= foodCost)  // Check if player has enough gold
        {
            switch (selectedFoodType)
            {
                case "strength":
                    currentMonster.strength += 1;
                    currentMonster.stamina += 25;
                    break;
                case "agility":
                    currentMonster.speed += 1;
                    currentMonster.stamina += 25;
                    break;
                case "health":
                    currentMonster.health += 1;
                    currentMonster.stamina += 25;
                    break;
                case "stamina":
                    currentMonster.stamina += 25;
                    SetMonsterMood("Eager");
                    break;
            }
        }
        gold -= foodCost;
        UpdateGoldUI();
        hasFedToday = true;
        npcDialogueText.text = $"{currentMonster.name} has eaten {selectedFoodType} food!";
        HideConfirmationButtons();
        HideFoodMenu();
        UpdateMonsterStatsUI();
    }


    // Cancel food selection
    void CancelFood()
    {
        npcDialogueText.text = "Feeding canceled.";
        HideConfirmationButtons();
    }

    // Hide the food menu window
    void HideFoodMenu()
    {
        foodMenu.SetActive(false);
    }


    void ConfirmAction()
    {
        if (isTrainingAction)
        {
            ConfirmTraining();
        }
        else if (isFeedingAction)
        {
            ApplyFoodEffect();
        }
    }

    void ConfirmCancel()
    {
        if (isTrainingAction)
        {
            CancelTraining();
        }
        else if (isFeedingAction)
        {
            CancelFood();
        }
    }


    void SetMonsterMood(string newMood)
    {
        monsterMood = newMood;
        UpdateMoodUI();
    }

    void UpdateMonsterStatsUI()
    {
        monsterStatsText.text = $"Health: {currentMonster.health}\nStrength: {currentMonster.strength}\nSpeed: {currentMonster.speed}\nStamina: {currentMonster.stamina}\nLifespan: {currentMonster.lifespan}";
        moodText.text = $"Mood: {monsterMood}";
    }

    void UpdateMoodUI()
    {
        moodText.text = $"Mood: {monsterMood}";
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

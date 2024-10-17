using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SummonManager : MonoBehaviour
{
    public TMP_InputField playerInput;
    public GameObject[] monsterPrefabs;
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI ticketCountText;
    public TextMeshProUGUI monsterNameText; // UI for monster name
    public Image npcImage;                  // NPC Image
    public GameObject dialogueBox;          // Dialogue box panel
    public TextMeshProUGUI dialogueText;    // Dialogue text inside the dialogue box
    public int ticketCount = 1;
    private GameObject currentMonster;
    public Transform spawnPosition;
    public static bool isMonsterSummoned = false;  // Track if a monster is summoned

    void Start()
    {
        UpdateTicketUI();
        ShowDialogue("Welcome! To summon a monster, type a name and press enter. You need a ticket to summon, and you have " + ticketCount + " tickets to start.");
        playerInput.onSubmit.AddListener(SummonMonster);
    }

    public void SummonMonster(string inputText)
    {
        if (ticketCount <= 0)
        {
            ShowDialogue("You don't have enough tickets to summon a monster!");
            return;
        }

        if (!string.IsNullOrEmpty(inputText))
        {
            if (currentMonster != null)
            {
                Destroy(currentMonster);  // Destroy current monster before summoning a new one
            }

            GenerateMonster(inputText);
            ticketCount--;
            UpdateTicketUI();

            // Clear the input field after summoning
            playerInput.text = "";

            // Show NPC dialogue with the monster's name
            ShowDialogue($"Wow! You have summoned {inputText}!");

            // Set the flag to true after summoning a monster
            isMonsterSummoned = true;
        }
        else
        {
            ShowDialogue("Please enter a name to summon a monster!");  // If input is empty
        }
    }

    void GenerateMonster(string input)
    {
        int monsterType = GetMonsterType(input);
        int health = GetStatFromInput(input);
        int strength = GetStatFromInput(input);
        int speed = GetStatFromInput(input);

        currentMonster = Instantiate(monsterPrefabs[monsterType], spawnPosition.position, Quaternion.identity);

        MonsterStats stats = currentMonster.GetComponent<MonsterStats>();
        stats.SetStats(health, strength, speed);

        // Display the monster's name at the top
        monsterNameText.text = input;

        UpdateMonsterStatsUI(health, strength, speed);
    }

    int GetMonsterType(string input)
    {
        int firstLetter = input[0];
        return firstLetter % monsterPrefabs.Length;
    }

    int GetStatFromInput(string input)
    {
        int statValue = 0;
        foreach (char c in input)
        {
            statValue += c;
        }
        return statValue % 100;
    }

    void UpdateMonsterStatsUI(int health, int strength, int speed)
    {
        monsterStatsText.text = $"Health: {health}\nStrength: {strength}\nSpeed: {speed}";
    }

    void UpdateTicketUI()
    {
        ticketCountText.text = $"Tickets: {ticketCount}";
    }

    // Show NPC dialogue
    void ShowDialogue(string message)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = message;
        npcImage.gameObject.SetActive(true);
    }


    void SaveMonsterData()
    {
        MonsterStats stats = currentMonster.GetComponent<MonsterStats>();
        MonsterTransfer.instance.SetMonsterData(currentMonster, monsterNameText.text, stats.health, stats.strength, stats.speed);
    }

    // When navigating to Ranch
    void GoToRanch()
    {
        if (currentMonster != null)
        {
            SaveMonsterData();
            UnityEngine.SceneManagement.SceneManager.LoadScene("RanchScene");
        }
        else
        {
            ShowDialogue("Summon a monster first before going to the Ranch.");
        }
    }



}

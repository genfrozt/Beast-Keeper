using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummonManager : MonoBehaviour
{
    public TMP_InputField playerInput;
    public GameObject[] monsterPrefabs;
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI ticketCountText;
    public TextMeshProUGUI monsterNameText;
    public Image npcImage;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public int ticketCount = 1;
    private GameObject currentMonster;
    public Transform spawnPosition;

    void Start()
    {
        UpdateTicketUI();
        ShowDialogue("Welcome! To summon a monster, type a name and press enter. You need a ticket to summon, and you have {ticketCount} tickets to start.");
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
                Destroy(currentMonster);  // Destroy any existing monster
            }

            GenerateMonster(inputText);
            ticketCount--;
            UpdateTicketUI();
            playerInput.text = "";

            // Show NPC dialogue with the monster's name
            ShowDialogue($"Wow! You have summoned {inputText}!");

            // Save the summoned monster data for the Ranch scene
            MonsterStats stats = currentMonster.GetComponent<MonsterStats>();
            MonsterTransfer.SetMonsterData(currentMonster, inputText, stats.health, stats.strength, stats.speed);
        }
        else
        {
            ShowDialogue("Please enter a name to summon a monster!");
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

    // Hide NPC dialogue
    void HideDialogue()
    {
        dialogueBox.SetActive(false);
        npcImage.gameObject.SetActive(false);
    }

    // Button click event for navigating to the Ranch scene
    public void OnRanchButtonClick()
    {
        if (MonsterTransfer.HasMonsterData())
        {
            Debug.Log("Loading RanchScene with monster: " + MonsterTransfer.monsterName);
            Debug.Log($"Monster stats - Health: {MonsterTransfer.health}, Strength: {MonsterTransfer.strength}, Speed: {MonsterTransfer.speed}");
            SceneManager.LoadScene("RanchScene");
        }
        else
        {
            ShowDialogue("We need to summon a monster before going to the ranch.");
        }
    }


}

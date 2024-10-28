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
                Destroy(currentMonster);
            }

            GenerateMonster(inputText);
            ticketCount--;
            UpdateTicketUI();
            playerInput.text = "";

            ShowDialogue($"Wow! You have summoned {inputText}!");

           
            // Debug check before storing
            MonsterStats stats = currentMonster.GetComponent<MonsterStats>();
            int monsterType = GetMonsterType(inputText);

            MonsterDataManager.Instance.SetMonsterData(monsterPrefabs[monsterType], inputText, stats.health, stats.strength, stats.speed, stats.stamina, stats.lifespan);
            Debug.Log($"Monster data set: {inputText}");
        }
        else
        {
            ShowDialogue("Please enter a name to summon a monster!");
        }
    
}

    void GenerateMonster(string input)
    {
        int monsterType = GetMonsterType(input);

        // Randomize stats between 1 and 10
        int health = Random.Range(1, 11);
        int strength = Random.Range(1, 11);
        int speed = Random.Range(1, 11);
        int stamina =   100;
        int lifespan = Random.Range(360, 720);

        currentMonster = Instantiate(monsterPrefabs[monsterType], spawnPosition.position, Quaternion.identity);
        MonsterStats stats = currentMonster.GetComponent<MonsterStats>();
        stats.SetStats(health, strength, speed, stamina, lifespan);

        monsterNameText.text = input;
        UpdateMonsterStatsUI(health, strength, speed);
    }

    int GetMonsterType(string input)
    {
        int firstLetter = input[0];
        return firstLetter % monsterPrefabs.Length;
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

    // Button click event for navigating to the Ranch scene
    public void OnRanchButtonClick()
    {
        if (MonsterTransfer.HasMonsterData())
        {
            SceneManager.LoadScene("RanchScene");
        }
        else
        {
            ShowDialogue("We need to summon a monster before going to the ranch.");
        }
    }
}

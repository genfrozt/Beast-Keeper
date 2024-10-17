using UnityEngine;
using TMPro;

public class SummonManager : MonoBehaviour
{
    public TMP_InputField playerInput; // Reference to the TextMeshPro InputField
    public GameObject[] monsterPrefabs; // Array of monster prefabs


    void Start()
    {
        playerInput.onEndEdit.AddListener(delegate { SummonMonster(); });
    }


    public void SummonMonster()
    {
        string inputText = playerInput.text; // Get input text from TMP_InputField
        Debug.Log("Player Input: " + inputText); // Log player input

        if (!string.IsNullOrEmpty(inputText))
        {
            GenerateMonster(inputText);
        }
        else
        {
            Debug.Log("No input text provided."); // Handle empty input case
        }
    }

    void GenerateMonster(string input)
    {
        int monsterType = GetMonsterType(input);
        int health = GetStatFromInput(input);
        int strength = GetStatFromInput(input);
        int speed = GetStatFromInput(input);

        Debug.Log("Monster Type: " + monsterType + " | Health: " + health + " | Strength: " + strength + " | Speed: " + speed);

        if (monsterPrefabs.Length > 0)
        {
            GameObject newMonster = Instantiate(monsterPrefabs[monsterType], new Vector3(0, 0, 0), Quaternion.identity);
            MonsterStats stats = newMonster.GetComponent<MonsterStats>();
            stats.SetStats(health, strength, speed);
            Debug.Log("Monster summoned: " + newMonster.name);
        }
        else
        {
            Debug.LogWarning("No monster prefabs available.");
        }
    }

    int GetMonsterType(string input)
    {
        int firstLetter = input[0]; // Get ASCII value of the first letter
        Debug.Log("First letter ASCII: " + firstLetter);
        return firstLetter % monsterPrefabs.Length; // Map to available monster prefabs
    }

    int GetStatFromInput(string input)
    {
        int statValue = 0;
        foreach (char c in input)
        {
            statValue += c; // Simple way to sum ASCII values for a stat
        }
        Debug.Log("Stat value from input: " + statValue % 100);
        return statValue % 100; // Cap stats at a max of 100
    }
}

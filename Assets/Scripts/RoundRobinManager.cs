using UnityEngine;
using UnityEngine.UI;

public class RoundRobinManager : MonoBehaviour
{
    public Text[] matchResults;  // Array of Text elements, one for each match-up

    // Example data structure to hold results
    private string[] matches = new string[]
    {
        "Player vs MonsterA",
        "MonsterB vs MonsterC",
        "Player vs MonsterB",
        "MonsterA vs MonsterC"
    };

    private string[] results = new string[] { "O", "X", "O", "X" };  // Placeholder results

    private void Start()
    {
        UpdateMatchResults();
    }

    private void UpdateMatchResults()
    {
        for (int i = 0; i < matchResults.Length; i++)
        {
            matchResults[i].text = $"{matches[i]}: {results[i]}";
        }
    }

    // Method to update results programmatically
    public void SetMatchResult(int matchIndex, bool playerWon)
    {
        results[matchIndex] = playerWon ? "O" : "X";
        UpdateMatchResults();
    }
}

using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI seasonText;
    public TextMeshProUGUI weatherText;
    public TextMeshProUGUI npcDialogueText;  // Reference to NPC dialogue UI
    public GameObject calendarPanel;  // Calendar panel reference
    public List<TextMeshProUGUI> calendarDays;  // List of TMPs for 30 days
    public Color tournamentColor;  // Color for tournament days
    public Button calendarButton;
    public Button calendarCloseButton;
    public List<int> tournamentDays = new(); // Store tournament days

    private int day = 1;
    private int year = 1000;
    private int month = 1;
    private string currentSeason = "Spring";
    private string currentWeather = "Sunny";

    private readonly Dictionary<int, List<int>> tournamentDates = new();
    private readonly string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    private int seasonIndex = 0;

    private readonly string[] months = {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };

    private readonly string[] weatherTypes = { "Sunny", "Windy", "Rainy", "Stormy" };
    private readonly float[] weatherChances = { 0.75f, 0.10f, 0.10f, 0.05f };

    void Start()
    {
        // Example: Adding tournament dates (Month: [Days])
        tournamentDates[1] = new List<int> { 10, 20};  // Tournaments on January 10th and 20th
        tournamentDates[2] = new List<int> { 5, 15, 25 };  // February tournaments
        calendarButton.onClick.AddListener(ToggleCalendarPanel);
        calendarCloseButton.onClick.AddListener(CloseCalendarPanel);
        UpdateUI();
        GenerateWeather();
        AnnounceWeather();
        PopulateCalendarUI();
        SetupTournamentDays();
    }

    public void AdvanceDay()
    {
        day++;
        if (day > 30)
        {
            day = 1;
            month++;

            if (month > 12)
            {
                month = 1;
                year++;
            }

            if (month % 3 == 1 && day == 1)
            {
                seasonIndex = (seasonIndex + 1) % seasons.Length;
                currentSeason = seasons[seasonIndex];
            }
        }

        GenerateWeather();
        UpdateUI();
        AnnounceWeather();
        PopulateCalendarUI();  // Update calendar with new dates
        CheckTournamentNotifications();

    }

    void SetupTournamentDays()
    {
        // For example, tournaments on day 10, 20, and 30
        tournamentDays.Add(10);
        tournamentDays.Add(20);
        tournamentDays.Add(30);
    }

    // NPC will notify before and on tournament days
    void CheckTournamentNotifications()
    {
        if (tournamentDays.Contains(day+1))
        {
            npcDialogueText.text = "A tournament is coming tomorrow! Make sure to get your monster ready.";
        }
        else if (tournamentDays.Contains(day))
        {
            npcDialogueText.text = "Today's the tournament! You can access it by pressing the Battle button.";
        }
    }
    void AnnounceWeather()
    {
        npcDialogueText.text = $"Good morning! Today's weather is {currentWeather}.";
    }

    void GenerateWeather()
    {
        float randomValue = UnityEngine.Random.value;
        float cumulativeProbability = 0;

        for (int i = 0; i < weatherTypes.Length; i++)
        {
            cumulativeProbability += weatherChances[i];
            if (randomValue <= cumulativeProbability)
            {
                currentWeather = weatherTypes[i];
                break;
            }
        }
    }

    void UpdateUI()
    {
        dayText.text = $"Year {year}, {months[month - 1]}, Day {day}";
        seasonText.text = $"Season: {currentSeason}";
        weatherText.text = $"Weather: {currentWeather}";
    }

    // New Method: Populating calendar days with tournament highlights
    void PopulateCalendarUI()
    {
       
        // Loop through each day button
        for (int i = 0; i < 30; i++)
        {
            // Ensure the text is always set to the day number (1–30)
            calendarDays[i].text = (i + 1).ToString();
            // Reset calendar day color to default
            calendarDays[i].color = Color.black;

            // Check if there's a tournament on this day
            if (tournamentDates.ContainsKey(month) && tournamentDates[month].Contains(i + 1))
            {
                // Highlight the tournament day
                calendarDays[i].color = tournamentColor;
            }

           
        }
    }

    // Toggle Calendar Panel Visibility
    public void ToggleCalendarPanel()
    {
        npcDialogueText.text = "The red dates are for tournaments. You can participate by pressing the Battle button.";
        calendarPanel.SetActive(true);
    }

    public void CloseCalendarPanel()
    {
        
        calendarPanel.SetActive(false);
    }
}

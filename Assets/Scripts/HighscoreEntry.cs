using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class HighscoreEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI positionText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI difficultyText;

    [SerializeField]
    private TextMeshProUGUI timeTakenText;

    [SerializeField]
    private TextMeshProUGUI killsText;

    [SerializeField]
    private TextMeshProUGUI timeAcomplishedText;

    [SerializeField]
    private Color lastTint;

    [SerializeField]
    private Image background;

    [SerializeField]
    private bool isMenuEntry = false;

    [SerializeField]
    private bool isAfterGame = false;

    private void Start()
    {
        if (isMenuEntry && ScoreEntry.Exists(0))
        {
            int numHighscores = PlayerPrefs.GetInt("numHighscores", 0);
            ScoreEntry[] scores = new ScoreEntry[numHighscores];
            for (int i = 0; i < numHighscores; i++)
            {
                scores[i] = ScoreEntry.Read(i);
            }
            List<ScoreEntry> highscores = scores.ToList();
            highscores.Sort();
            Initialize(highscores[0], 0);
        }
    }

    public void Initialize(ScoreEntry entry, int index)
    {
        if (!isMenuEntry && !isAfterGame)
            name = $"Highscore #{index.ToString("D4")} - {entry.name}";

        if (!isMenuEntry && !isAfterGame)
            positionText.text = "#" + index.ToString("D4");
        scoreText.text = entry.score.ToString("D7");
        if (!isAfterGame)
            nameText.text = entry.name;

        if (entry.difficulty == Difficulty.Hard && (entry.name == "PP" || entry.name == "PEEPEE" || entry.name == "PEPE"))
            difficultyText.text = "SOFT";
        else
            difficultyText.text = entry.difficulty.ToString().ToUpper();
        if (!isMenuEntry)
            killsText.text = entry.kills.ToString("D5");
        else
            killsText.text = $"{entry.kills.ToString("D5")} kills";

        int hours = Mathf.FloorToInt(entry.timeTaken / 3600f);
        int minutes = Mathf.FloorToInt((entry.timeTaken % 3600f) / 60f);
        int seconds = Mathf.FloorToInt((entry.timeTaken % 60f));
        if (isMenuEntry || isAfterGame)
        {
            if (hours != 0)
                timeTakenText.text = $"{hours}h {minutes}m {seconds}s";
            else if (minutes != 0)
                timeTakenText.text = $"{minutes}m {seconds}s";
            else
                timeTakenText.text = $"{seconds}s";
        }
        else
        {
            timeTakenText.text = $"{hours}h {minutes}m {seconds}s";
        }
        if (!isAfterGame)
            timeAcomplishedText.text = GetDateString(entry);

        if (entry.isLast && !isAfterGame)
            background.color *= lastTint;
    }

    private static string GetDateString(ScoreEntry entry)
    {
        string dateTime;
        TimeSpan ts = DateTime.Now - entry.entryIRL_Time;
        TimeInfo timeInfo = GetTimeInfo(ts);
        if (entry.isLast)
        {
            dateTime = "a second ago";
        }
        else if (timeInfo.years > 0)
        {
            if (timeInfo.years == 1)
                dateTime = $"1 year ago";
            else
                dateTime = $"{timeInfo.years} years ago";
        }
        else if (timeInfo.months > 0)
        {
            if (timeInfo.months == 1)
                dateTime = $"1 month ago";
            else
                dateTime = $"{timeInfo.months} months ago";
        }
        else if (timeInfo.days > 0)
        {
            if (timeInfo.days == 1)
                dateTime = $"1 day ago";
            else
                dateTime = $"{timeInfo.days} days ago";
        }
        else if (timeInfo.hours > 0)
        {
            if (timeInfo.hours == 1)
                dateTime = $"1 hour ago";
            else
                dateTime = $"{timeInfo.hours} hours ago";
        }
        else if (timeInfo.minutes > 0)
        {
            if (timeInfo.minutes == 1)
                dateTime = $"1 minute ago";
            else
                dateTime = $"{timeInfo.minutes} minutes ago";
        }
        else
        {
            if (timeInfo.seconds == 1)
                dateTime = "1337"; // easter egg
            else
                dateTime = $"{timeInfo.seconds} seconds ago";
        }
        return dateTime;
    }

    private static TimeInfo GetTimeInfo(TimeSpan timeSpan)
    {
        TimeInfo timeInfo = new TimeInfo(
        years: Mathf.FloorToInt(timeSpan.Days / 365f),
        months: Mathf.FloorToInt(timeSpan.Days / 30f),
        days: timeSpan.Days,
        hours: timeSpan.Hours,
        minutes: timeSpan.Minutes,
        seconds: timeSpan.Seconds
        );
        return timeInfo;
    }

    public void SetNewName(string playerName)
    {
        nameText.text = playerName;
        name += playerName;
    }
}

internal struct TimeInfo
{
    public int years;
    public int months;
    public int days;
    public int hours;
    public int minutes;
    public int seconds;

    public TimeInfo(int years, int months, int days, int hours, int minutes, int seconds)
    {
        this.years = years;
        this.months = months;
        this.days = days;
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
    }
}
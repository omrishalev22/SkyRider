using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoresController : MonoBehaviour
{
    public static ScoresController instance;
    private List<HighscoreEntry> highscoreEntryList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (highscoreEntryList == null)
        {
            highscoreEntryList = new List<HighscoreEntry>();
        }
    }

    private void SortHighscoreEntryList(List<HighscoreEntry> highscoreEntryList)
    {
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    HighscoreEntry tempEntry = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tempEntry;
                }
            }
        }
    }

    public class HighscoreEntry
    {
        public int score;
        public string timer;
    }

    public void AddHighscoreEntry(int score, string timer)
    {
        HighscoreEntry newEntry = new HighscoreEntry { score = score, timer = timer };
        highscoreEntryList.Add(newEntry);
        SortHighscoreEntryList(highscoreEntryList);
        
    }

    public List<HighscoreEntry> GetHighscoreEntries(int firstEntries)
    {
        return highscoreEntryList.GetRange(0, Math.Min(firstEntries, highscoreEntryList.Count));
    }
}

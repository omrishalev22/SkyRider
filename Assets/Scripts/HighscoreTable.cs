using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    static List<HighscoreEntry> highscoreEntryList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        highscoreEntryTransformList = new List<Transform>();
        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 23434, timer = "10:10:10" },
            new HighscoreEntry{ score = 62345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" },
            new HighscoreEntry{ score = 12345, timer = "10:10:10" }
        };

        SortHighscoreEntryList(highscoreEntryList);

        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
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

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        float baseHeight = 10f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count - baseHeight);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        entryTransform.Find("posText").GetComponent<Text>().text = rank.ToString();
        entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.score.ToString();
        entryTransform.Find("timerText").GetComponent<Text>().text = highscoreEntry.timer;

        transformList.Add(entryTransform);
    }

    private class HighscoreEntry
    {
        public int score;
        public string timer;
    }

    public void AddHighscoreEntry(int score, string timer)
    {
        HighscoreEntry newEntry = new HighscoreEntry { score = score, timer = timer };
        highscoreEntryList.Add(newEntry);
    }

}


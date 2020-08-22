using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private int totalEntries = 10;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        highscoreEntryTransformList = new List<Transform>();
    }

    private void Start()
    {
        if (ScoresController.instance)
        {
            foreach (ScoresController.HighscoreEntry highscoreEntry in ScoresController.instance.GetHighscoreEntries(totalEntries))
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    private void CreateHighscoreEntryTransform(ScoresController.HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
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
}


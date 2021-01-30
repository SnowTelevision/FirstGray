using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameProcess : MonoBehaviour
{
    public TMP_Text levelFlavorText; // Some flavor text to show the player for each level
    public TMP_Text levelNameText;
    public List<LevelPatterns> allLevels;
    public List<string> allLevelNames;
    public List<TMP_Text> allLevelFlavorTexts;

    public static GameProcess instance;
    public int currentLevelIndex; // Index of the current playing level

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Start level 1
        currentLevelIndex = 0;
        StartLevel(currentLevelIndex);
    }

    /// <summary>
    /// Start a level
    /// </summary>
    /// <param name="levelIndex"></param>
    public void StartLevel(int levelIndex)
    {
        levelNameText.text = allLevelNames[levelIndex];
        levelFlavorText.text = allLevelFlavorTexts[levelIndex].text;
        PlayLevel.instance.StartNewLevel(allLevels[levelIndex]);
    }

    /// <summary>
    /// Process when player wins the current level
    /// </summary>
    public void WinLevel()
    {
        // If player win the last level
        if (currentLevelIndex == allLevels.Count - 1)
        {

        }
        else
        {
            currentLevelIndex++;
            StartLevel(currentLevelIndex);
        }
    }

    /// <summary>
    /// Process when player loses the current level
    /// </summary>
    public void LoseLevel()
    {
        StartLevel(currentLevelIndex);
    }
}

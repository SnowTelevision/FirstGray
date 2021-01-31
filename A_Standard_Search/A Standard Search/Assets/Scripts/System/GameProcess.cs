using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class GameProcess : MonoBehaviour
{
    public TMP_Text levelFlavorText; // Some flavor text to show the player for each level
    public TMP_Text levelNameText;
    public List<LevelPatterns> allLevels;
    public List<string> allLevelNames;
    public List<TMP_Text> allLevelFlavorTexts;
    public GameObject endingUI; // Game ending graphic to show when player clear all levels

    public static GameProcess instance;
    public int currentLevelIndex; // Index of the current playing level
    public static bool firstStart; // Is this the level start after hitting "start" button on main menu?

    private void Awake()
    {
        instance = this;
        firstStart = true;
    }

    private void Start()
    {
        // Start level 1
        currentLevelIndex = 0;
        StartLevel(currentLevelIndex);

        // Play BGM
        //AudioManager.instance.StartBGM(AudioManager.instance.bgm);
    }

    /// <summary>
    /// Start a level
    /// </summary>
    /// <param name="levelIndex"></param>
    public void StartLevel(int levelIndex)
    {
        StartCoroutine(PlayLevel.instance.LevelTransition(allLevels[levelIndex]));
    }

    /// <summary>
    /// Process when player wins the current level
    /// </summary>
    public void WinLevel()
    {
        // Update level fader color
        PlayLevel.instance.levelFader.color = Color.white;

        // If player win the last level
        if (currentLevelIndex == allLevels.Count - 1)
        {
            StartCoroutine(GameEnding());
        }
        else
        {
            // Play move sound effect
            AudioManager.instance.PlaySFX(AudioManager.instance.levelWinSFX);

            currentLevelIndex++;
            StartLevel(currentLevelIndex);
        }
    }

    /// <summary>
    /// Process when player loses the current level
    /// </summary>
    public void LoseLevel()
    {
        // Update level fader color
        PlayLevel.instance.levelFader.color = Color.black;

        // Play move sound effect
        AudioManager.instance.PlaySFX(AudioManager.instance.levelLoseSFX);

        StartLevel(currentLevelIndex);
    }

    /// <summary>
    /// Process for when player clear the game
    /// </summary>
    public IEnumerator GameEnding()
    {
        // Stop player input after game end
        PlayLevel.instance.canPlayerMove = false;

        float duration = 0.75f;

        // Fade in screen
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Color newColor = PlayLevel.instance.levelFader.color;
            newColor.a = t / duration;
            PlayLevel.instance.levelFader.color = newColor;
            yield return null;
        }
        Color fullColor = PlayLevel.instance.levelFader.color;
        fullColor.a = 1;
        PlayLevel.instance.levelFader.color = fullColor;

        endingUI.SetActive(true);

        // Play ending BGM
        AudioManager.instance.StartBGM(AudioManager.instance.endingBGM);

        // Fade out screen
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Color newColor = PlayLevel.instance.levelFader.color;
            newColor.a = (duration - t) / duration;
            PlayLevel.instance.levelFader.color = newColor;
            yield return null;
        }
        Color noColor = PlayLevel.instance.levelFader.color;
        noColor.a = 0;
        PlayLevel.instance.levelFader.color = noColor;
    }

    /// <summary>
    /// 
    /// </summary>
    [ShowInInspector]
    public void TestGameEnding()
    {
        StartCoroutine(GameEnding());
    }
}

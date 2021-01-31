using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

/// <summary>
/// Handle player input for playing a level and the process of the played level
/// 0, 0 is bottom left corner
/// </summary>
public class PlayLevel : MonoBehaviour
{
    public GameObject gridObject; // Prefab for display for a single grid
    public float gridDistance; // Distance between each grid display
    public float camHeight;
    public Sprite exitWhite; // White / 0 sprite for the exit tile
    public Sprite exitBlack; // Black / 1 sprite for the exit tile
    public GameObject levelFrame; // The frame sprite for the outside border of each level (need to adjust size to level)
    public Image levelFader;

    public bool canPlayerMove; // Prevent player from moving while the pattern update animation is ongoing
    public LevelPatterns currentLevel;
    public int playerXcoord;
    public int playerYcoord;
    public SinglePattern currentPattern;
    public List<GameObject> currentGridDisplays; // Objects for all the grids in current level
    public int startPointXcoord;
    public int startPointYcoord;
    public int exitPointXcoord;
    public int exitPointYcoord;
    public List<Vector2> moveHistory; // Player's move history of the current playing level
    public WaitForSeconds animationWait; // How long is the level pattern update animation

    public static PlayLevel instance;

    private void Awake()
    {
        instance = this;
        animationWait = new WaitForSeconds(1);
    }

    /// <summary>
    /// Keyboard controls
    /// </summary>
    private void Update()
    {

        if (!canPlayerMove) // Only take player's key input if there is no animation playing
        {
            if (Input.GetKeyUp(KeyCode.W) | Input.GetKeyUp(KeyCode.UpArrow))//move up
            {
                this.PlayerMoved(0, 1);
            }
            if (Input.GetKeyUp(KeyCode.S) | Input.GetKeyUp(KeyCode.DownArrow))//move down
            {
                this.PlayerMoved(0, -1);
            }
            if (Input.GetKeyUp(KeyCode.A) | Input.GetKeyUp(KeyCode.LeftArrow))//move left
            {
                this.PlayerMoved(-1, 0);
            }
            if (Input.GetKeyUp(KeyCode.D) | Input.GetKeyUp(KeyCode.RightArrow))//move right
            {
                this.PlayerMoved(1, 0);
            }
            if (Input.GetKeyUp(KeyCode.Space))// press space
            {
                this.CheckWinning();
            }
        }
    }

    /// <summary>
    /// Set start point and exit point randomly
    /// </summary>
    private void SetRandomStartAndExit()
    {
        while (true)
        {
            startPointXcoord = BetterRandom.betterRandom(0, currentLevel.width - 1);
            startPointYcoord = BetterRandom.betterRandom(0, currentLevel.height - 1);//get a random start point
            exitPointXcoord = BetterRandom.betterRandom(0, currentLevel.width - 1);
            exitPointYcoord = BetterRandom.betterRandom(0, currentLevel.height - 1);//get a random exit point
            if (startPointXcoord != exitPointXcoord || startPointYcoord != exitPointYcoord)
                break;//check if player was born at exit
        }
    }

    /// <summary>
    /// When player moves in a direction
    /// </summary>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    /// <returns></returns>
    public SinglePattern PlayerMoved(int xDir, int yDir)
    {
        // Store last position coord
        if (xDir != 0 || yDir != 0)
        {
            moveHistory.Add(new Vector2(playerXcoord, playerYcoord));
        }

        // Get player new position
        playerXcoord += xDir;
        if (playerXcoord < 0)
        {
            playerXcoord = currentLevel.width - 1;
        }
        if (playerXcoord == currentLevel.width)
        {
            playerXcoord = 0;
        }
        playerYcoord += yDir;
        if (playerYcoord < 0)
        {
            playerYcoord = currentLevel.height - 1;
        }
        if (playerYcoord == currentLevel.height)
        {
            playerYcoord = 0;
        }

        // Proceed result base on the grid color and coord the player moved to
        SinglePattern previousPattern = currentPattern;
        SinglePattern nextPattern = null;

        // If this is the start move from a new level
        if (xDir == 0 && yDir == 0)
        {
            previousPattern = GetPattern(playerXcoord, playerYcoord);
            nextPattern = GetPattern(playerXcoord, playerYcoord);
        }
        else
        {
            // Play move sound effect
            AudioManager.instance.PlaySFX(AudioManager.instance.playerMoveSFX);

            switch (GetGrid(currentPattern, playerXcoord, playerYcoord))
            {
                // White grid
                case 0:
                    nextPattern = GetPattern(playerXcoord, playerYcoord);
                    break;
                // Gray grid
                case 1:
                    nextPattern = currentPattern;
                    break;
            }
        }

        currentPattern = nextPattern;

        // Play the pattern update animation (can have animation even if the pattern didn't change)
        canPlayerMove = true;
        StartCoroutine(UpdateGridsDisplay(currentPattern, previousPattern));

        return nextPattern;
    }

    /// <summary>
    /// Get a grid's value based on the given pattern and coord
    /// </summary>
    /// <param name="refPattern"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetGrid(SinglePattern refPattern, int x, int y)
    {
        int yIndex = refPattern.height - y - 1; // Get the y index for the grid for the given coord
        int index = x + refPattern.width * yIndex; // Get the index for the coord
        return refPattern.pattern[index];
    }

    /// <summary>
    ///  Get a pattern for a grid in the current level
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public SinglePattern GetPattern(int x, int y)
    {
        int yIndex = currentLevel.height - y - 1; // Get the y index for the grid for the given coord
        int index = x + currentLevel.width * y; // Get the index for the coord
        return currentLevel.patterns[index];
    }

    /// <summary>
    /// Update the game's grids base on the given pattern
    /// </summary>
    /// <param name="newPattern"></param>
    /// <param name="currentPattern"></param>
    public IEnumerator UpdateGridsDisplay(SinglePattern newPattern, SinglePattern currentPattern)
    {
        for (int i = 0; i < currentGridDisplays.Count; i++)
        {
            currentGridDisplays[i].GetComponent<Tile>().StateChange(newPattern.pattern[i]);
            //currentGridDisplays[i].GetComponent<Tile>().currState = newPattern.pattern[i];
            //UpdateTile(currentGridDisplays[i], newPattern.pattern[i]);
        }

        yield return animationWait;

        canPlayerMove = false;
    }

    /// <summary>
    /// Check if player stands on the exit grid when player press the checking key
    /// </summary>
    /// <returns></returns>
    public bool CheckWinning()
    {
        if (playerXcoord == exitPointXcoord && playerYcoord == exitPointYcoord)
        {
            //print("win");
            GameProcess.instance.WinLevel();
            return true;
        }
        else
        {
            //print("lose");
            GameProcess.instance.LoseLevel();
            return false;
        }
    }

    /// <summary>
    /// Create GameObjects for the grid for a new level and re-focus the camera
    /// </summary>
    public void CreateNewLevelGridObjects()
    {
        currentGridDisplays.ForEach(g => DestroyImmediate(g)); // Clear any existing grid display from last level
        currentGridDisplays.Clear();

        for (int y = 0; y < currentLevel.height; y++)
        {
            for (int x = 0; x < currentLevel.width; x++)
            {
                GameObject newGridDisplay = Instantiate(gridObject);
                Vector3 newGridPosition = new Vector3(x * gridDistance, 0, (currentLevel.height - y - 1) * gridDistance);
                newGridDisplay.transform.position = newGridPosition;
                currentGridDisplays.Add(newGridDisplay);

                // If is exit
                if (x == exitPointXcoord && currentLevel.height - y - 1 == exitPointYcoord)
                {
                    newGridDisplay.GetComponent<Tile>().IAmExit();
                    //newGridDisplay.GetComponent<Tile>().isExit = true;
                    //newGridDisplay.GetComponent<Tile>().white = exitWhite;
                    //newGridDisplay.GetComponent<Tile>().black = exitBlack;
                }
            }
        }

        // Reposition and rescale level frame 
        Vector3 newFramePosition = new Vector3((currentLevel.width - 1) * gridDistance * 0.5f, -0.5f, (currentLevel.height - 1) * gridDistance * 0.5f + 0.05f);
        levelFrame.transform.position = newFramePosition;
        Vector2 newFrameScale = new Vector2();
        newFrameScale.x = 1.5f * currentLevel.width + 0.9f;
        newFrameScale.y = 1.5f * currentLevel.height + 0.9f;
        levelFrame.GetComponent<SpriteRenderer>().size = newFrameScale;

        // Reposition level map
        currentGridDisplays.ForEach(g => g.transform.parent = levelFrame.transform);
        levelFrame.transform.position = Vector3.zero;

        //// Reposition camera
        //Vector3 newCamPosition = new Vector3((currentLevel.width - 1) * gridDistance * 0.5f, camHeight, (currentLevel.height - 1) * gridDistance * 0.5f);
        //Camera.main.transform.position = newCamPosition;
    }

    /// <summary>
    /// Start a new level
    /// </summary>
    /// <param name="newLevelData"></param>
    [ShowInInspector]
    public void StartNewLevel(LevelPatterns newLevelData)
    {
        // Change level texts
        GameProcess.instance.levelNameText.text = GameProcess.instance.allLevelNames[GameProcess.instance.currentLevelIndex];
        GameProcess.instance.levelFlavorText.text = GameProcess.instance.allLevelFlavorTexts[GameProcess.instance.currentLevelIndex].text;

        // Refresh player move history
        moveHistory = new List<Vector2>();

        currentLevel = newLevelData;

        SetRandomStartAndExit();

        // Create new map
        CreateNewLevelGridObjects();

        // Make first move for the player
        playerXcoord = startPointXcoord;
        playerYcoord = startPointYcoord;
        PlayerMoved(0, 0);
    }

    /// <summary>
    /// Transition for the level when start new level or repeat current level
    /// </summary>
    /// <returns></returns>
    public IEnumerator LevelTransition(LevelPatterns newLevelData)
    {
        canPlayerMove = false;

        float duration = 0.75f;

        if (!GameProcess.firstStart) // If the game just start then only fade out
        {
            // Fade in screen
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                Color newColor = levelFader.color;
                newColor.a = t / duration;
                levelFader.color = newColor;
                yield return null;
            }
            Color fullColor = levelFader.color;
            fullColor.a = 1;
            levelFader.color = fullColor;
        }
        else
        {
            GameProcess.firstStart = false;
        }

        StartNewLevel(newLevelData);

        // Fade out screen
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Color newColor = levelFader.color;
            newColor.a = (duration - t) / duration;
            levelFader.color = newColor;
            yield return null;
        }
        Color noColor = levelFader.color;
        noColor.a = 0;
        levelFader.color = noColor;

        canPlayerMove = true;
    }

    /// <summary>
    /// Simply switch the tile's sprite for pattern change
    /// </summary>
    /// <param name="tileToUpdate"></param>
    /// <param name="state"></param>
    //public void UpdateTile(GameObject tileToUpdate, int state)
    //{
    //    if (tileToUpdate.TryGetComponent(out SpriteRenderer sr))
    //    {
    //        if (state == 0)
    //        {
    //            sr.sprite = tileToUpdate.GetComponent<Tile>().white;
    //        }
    //        else
    //        {
    //            sr.sprite = tileToUpdate.GetComponent<Tile>().black;
    //        }
    //    }
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Handle player input for playing a level and the process of the played level
/// 0, 0 is bottom left corner
/// </summary>
public class PlayLevel : MonoBehaviour
{
    // TEMP
    public Material white;
    public Material black;
    // TEMP
    public GameObject gridObject; // Prefab for display for a single grid
    public float gridDistance; // Distance between each grid display

    public bool isUpdatingPattern; // Prevent player from moving while the pattern update animation is ongoing
    public LevelPatterns currentLevel;
    public int playerXcoord;
    public int playerYcoord;
    public SinglePattern currentPattern;
    public List<GameObject> currentGridDisplays; // Objects for all the grids in current level
    private int startPointXcoord;
    private int startPointYcoord;
    private int exitPointXcoord;
    private int exitPointYcoord;
    public List<Vector2> moveHistory; // Player's move history of the current playing level

    /// <summary>
    /// Keyboard controls
    /// </summary>
    private void Update()
    {

        if (!isUpdatingPattern) // Only take player's key input if there is no pattern update animation playing
        {
            if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow))//move up
            {
                this.PlayerMoved(0, 1);
            }
            if (Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.DownArrow))//move down
            {
                this.PlayerMoved(0, -1);
            }
            if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))//move left
            {
                this.PlayerMoved(-1, 0);
            }
            if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))//move right
            {
                this.PlayerMoved(1, 0);
            }
            if (Input.GetKey(KeyCode.Space))// press space
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
            startPointXcoord = BetterRandom.betterRandom(0, currentPattern.width);
            startPointYcoord = BetterRandom.betterRandom(0, currentPattern.height);//get a random start point
            exitPointXcoord = BetterRandom.betterRandom(0, currentPattern.width);
            exitPointYcoord = BetterRandom.betterRandom(0, currentPattern.height);//get a random exit point
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
        moveHistory.Add(new Vector2(playerXcoord, playerYcoord));

        // Get player new position
        playerXcoord += xDir;
        playerYcoord += yDir;

        // Proceed result base on the grid color and coord the player moved to
        SinglePattern previousPattern = currentPattern;
        SinglePattern nextPattern = null;
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

        currentPattern = nextPattern;

        // Play the pattern update animation (can have animation even if the pattern didn't change)
        isUpdatingPattern = true;
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
        int index = x + refPattern.width * y; // Get the index for the coord
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
        }

        yield return null;

        isUpdatingPattern = false;
    }

    /// <summary>
    /// Check if player stands on the exit grid when player press the checking key
    /// </summary>
    /// <returns></returns>
    public bool CheckWinning()
    {
        if (playerXcoord == exitPointXcoord && playerYcoord == exitPointYcoord)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Create GameObjects for the grid for a new level and re-focus the camera
    /// </summary>
    public void CreateNewLevelGridObjects()
    {
        currentGridDisplays.ForEach(g => Destroy(g)); // Clear any existing grid display from last level

        for (int y = 0; y < currentLevel.height; y++)
        {
            for (int x = 0; x < currentLevel.width; x++)
            {
                GameObject newGridDisplay = Instantiate(gridObject);
                Vector3 newGridPosition = new Vector3(x * gridDistance, 0, (currentLevel.height - y - 1) * gridDistance);
                newGridDisplay.transform.position = newGridPosition;
                currentGridDisplays.Add(newGridDisplay);
            }
        }

        // Reposition camera
        Vector3 newCamPosition = new Vector3(currentLevel.width * gridDistance * 0.5f, 0, currentLevel.height * gridDistance * 0.5f);
        Camera.main.transform.position = newCamPosition;
    }

    /// <summary>
    /// Start a new level
    /// </summary>
    /// <param name="newLevelData"></param>
    [ShowInInspector]
    public void StartNewLevel(LevelPatterns newLevelData)
    {
        // Refresh player move history
        moveHistory = new List<Vector2>();

        currentLevel = newLevelData;

        // Create new map
        CreateNewLevelGridObjects();

        SetRandomStartAndExit();

        // Make first move for the player
        playerXcoord = startPointXcoord;
        playerYcoord = startPointYcoord;
        PlayerMoved(0, 0);
    }
}

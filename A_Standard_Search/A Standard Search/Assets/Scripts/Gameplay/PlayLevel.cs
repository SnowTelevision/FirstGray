using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle player input for playing a level and the process of the played level
/// 0, 0 is bottom left corner
/// </summary>
public class PlayLevel : MonoBehaviour
{


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


    /// <summary>
    /// Keyboard controls
    /// </summary>
    private void Update()
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

    /// <summary>
    /// Set start point and exit point randomly
    /// </summary>
    private void SetRandomStartAndExit() {
        while (true)
        {
            startPointXcoord = Random.Range(0, currentPattern.width);
            startPointYcoord = Random.Range(0, currentPattern.height);//get a random start point
            exitPointXcoord = Random.Range(0, currentPattern.width);
            exitPointYcoord = Random.Range(0, currentPattern.height);//get a random exit point
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
}

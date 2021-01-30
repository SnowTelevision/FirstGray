using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelPatterns : MonoBehaviour
{
    public int width; // Width (x length) of the level
    public int height; // Height ( y length) of the level

    [ShowInInspector]
    public List<SinglePattern> patterns; // All the patterns for each grid of this level. Ordered by rows then column of rows. For example a 2*2 grid will have its first row be the [0] and [1], then second row will be [2], [3]

    /// <summary>
    /// Create all the empty pattern storage objects based on the entered width and height
    /// </summary>
    [ShowInInspector]
    public void CreateLevelPatternStorages()
    {
        // Valid check
        if (width <= 0 || height <= 0)
        {
            print("Invalid level size");
            return;
        }

        // Create pattern storages
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject newStorage = new GameObject(x + ", " + y);
                SinglePattern newPattern = newStorage.AddComponent<SinglePattern>();
                newPattern.width = width;
                newPattern.height = height;
                newStorage.transform.parent = transform;
                patterns.Add(newPattern);
            }

        }
    }
}

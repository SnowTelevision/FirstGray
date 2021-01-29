using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class SinglePattern : MonoBehaviour
{
    public string patternEntry; // Editor enter the pattern into this string which will auto fill into the list for later update

    [ShowInInspector]
    public List<int> pattern; // One pattern for one grid of the level, 0 is white, 1 is gray, order is same as the level patterns order
    [HideInInspector]
    public int width;
    [HideInInspector]
    public int height;

    [ShowInInspector]
    public void EnterPattern()
    {
        patternEntry.ForEach(grid => pattern.Add(int.Parse(grid.ToString())));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{

    public int currState;
    public int animate;
    private Animator animator;
    private enum animatenames {a1,a2,a3,a4};


    public void StateChange(int state)
    {
        if(state == 0 && currState == 0)//light to light
        {
            playAnime(0);
        }
        if (state == 0 && currState == 1)//light to dark
        {
            playAnime(0);
        }
        if (state == 1 && currState == 0)//dark to light
        {
            playAnime(0);
        }
        if (state == 1 && currState == 1)//dark to dark
        {
            playAnime(0);
        }
        Debug.Log("My state changes.");
        currState = state;
        return;
    }

    void playAnime(int animeID)//play a state change animate 
    {

        return;
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

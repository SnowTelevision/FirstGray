using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{



    public int currState;
    private Animator animator;
    public bool isExit;

    void Start()
    {
       animator= GetComponent<Animator>();
    }


    public void StateChange(int state)
    {
        if(state == 0 && currState == 0)//light to light
        {
            animator.playAnime(0);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

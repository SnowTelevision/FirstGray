using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



public class Tile : MonoBehaviour
{



    public int currState;
    public Animator animator;
    public bool isExit;
    public Sprite white; // Sprite for the "white" / 0 state
    public Sprite black; // Sprite for the "black" / 1 state

    void Start()
    {
 
    }
    public void IAmExit()
    {
        isExit = true;
        StateChange(currState);
    }
    
    public void StateChange(int state)
    {
        if (!isExit)
        {
            if (state == 0 && currState == 0)//light to light
            {
                animator.SetInteger("State", 1);
            }
            if (state == 0 && currState == 1)//dark to light
            {
                animator.SetInteger("State", 3);
            }
            if (state == 1 && currState == 0)//light to dark
            {
                animator.SetInteger("State", 0);
            }
            if (state == 1 && currState == 1)//dark to dark
            {
                animator.SetInteger("State", 2);
            }
        }
        else
        {
            if (state == 0 && currState == 0)//light to light
            {
                animator.SetInteger("State", 5);
            }
            if (state == 0 && currState == 1)//dark to light
            {
                animator.SetInteger("State", 4);
            }
            if (state == 1 && currState == 0)//light to dark
            {
                animator.SetInteger("State", 7);
            }
            if (state == 1 && currState == 1)//dark to dark
            {
                animator.SetInteger("State", 6);
            }
        }
        
        currState = state;
        return;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
}

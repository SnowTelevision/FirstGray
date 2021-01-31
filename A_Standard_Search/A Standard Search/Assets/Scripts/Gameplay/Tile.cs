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
                animator.Play("Base Layer.0 to 0", 0, 0.0f);
                Debug.Log("light to light");
            }
            if (state == 0 && currState == 1)//dark to light
            {
                animator.SetInteger("State", 3);
                Debug.Log("dark to light");
            }
            if (state == 1 && currState == 0)//light to dark
            {
                animator.SetInteger("State", 0);
                Debug.Log("light to dark");
            }
            if (state == 1 && currState == 1)//dark to dark
            {
                animator.SetInteger("State", 2);
                animator.Play("Base Layer.1 to 1", 0, 0.0f);
                Debug.Log("dark to dark");
            }
        }
        else
        {
            if (state == 0 && currState == 0)//light to light
            {
                animator.SetInteger("State", 5);
                animator.Play("Base Layer.exit 0 to 0", 0, 0.0f);
                Debug.Log("EXlight to light");
            }
            if (state == 0 && currState == 1)//dark to light
            {
                animator.SetInteger("State", 4);
                Debug.Log("EXdark to light");
            }
            if (state == 1 && currState == 0)//light to dark
            {
                animator.SetInteger("State", 7);
                Debug.Log("EXlight to dark");
            }
            if (state == 1 && currState == 1)//dark to dark
            {
                animator.SetInteger("State", 6);
                animator.Play("Base Layer.exit 1 to 1", 0, 0.0f);
                Debug.Log("EXdark to dark");
            }
        }
        Debug.Log("state" + currState + "to" + state);

        currState = state;
        return;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
            //
    }
}

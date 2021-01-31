using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUi : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("It should work...");

        // Stop menu bgm
        AudioManager.instance.PauseBGM();
    }

    private void Start()
    {
        AudioManager.instance.StartBGM(AudioManager.instance.bgm);
    }
}

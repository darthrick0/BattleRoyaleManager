using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

    public void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //Application.Quit;
        print("Quit Game");
    }

}

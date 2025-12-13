using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    private int _health = 5;

    public int Health
    {
        get { return _health; }
    }

    public void Hurt(int damage)
    {
        _health -= damage;
        //Debug.Log("Player Health: " + _health);

        if (_health == 0)
            QuitGame();
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}

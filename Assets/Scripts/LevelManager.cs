using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public UnityEvent OnLevelReset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }





    public void ResetLevel()
    {
        OnLevelReset.Invoke();
    }


    public void WinLevel()
    {
        // UI stuff
        // gives rewards
        //loads next level 

    }

    public void LoadLevel(LevelObject levelObject)
    {
        //instantiate the level objects 
        //transform player position into the spawn position of the object 
        // give inputs to player 
    }


    
}

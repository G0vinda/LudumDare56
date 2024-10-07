using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public UnityEvent OnLevelDestroyed;

    [HideInInspector]
    public LevelObject currentLevel;

    public LevelObject currentLevelPrefab;


    [HideInInspector]
    public List<CharacterInput> spawnedCharacters = new List<CharacterInput>();
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
        foreach(var character in spawnedCharacters)
        {
            Destroy(character);
        }

        currentLevel.DestoryLevel();
        OnLevelDestroyed.Invoke();
    }


    public void WinLevel()
    {
        // UI stuff
        // gives rewards
        //loads next level 
        
        GameManager.instance.StartRandomLevel();
    }

    public void LoadLevel(LevelObject levelObject)
    {
        //instantiate the level objects 
        //transform player position into the spawn position of the object 
        // give inputs to player 
        if (currentLevel != null)
        {

           currentLevel.DestoryLevel();
        }
        currentLevelPrefab = levelObject;
        currentLevel = Instantiate(levelObject);
        currentLevel.SetupLevel(GameManager.instance.TranslateRoundToDifficulty(GameManager.instance.currentRound)) ;
        SpawnPlayers(levelObject);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelPrefab);
    }


    public void SpawnPlayers(LevelObject levelObject)
    {
        //spawn every character 1 by 1 to the spawn position on the level objects
       // CharacterManager.instance._characters

        for(int i = 0; i < 3; i++)
        {
            CharacterInput character=Instantiate(GameManager.instance.characterPrefabs[i]);

            character.transform.position = levelObject.spawnPositions[i].position;
            spawnedCharacters.Add(character);
        }

        CharacterManager.instance.SetupCharacters(spawnedCharacters);
        CameraManager.instance.SwitchCameraFollowTarget(spawnedCharacters[0].transform);

    }



    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public UnityEvent OnLevelDestroyed;
    [HideInInspector] public LevelObject currentLevel;
    public LevelObject currentLevelPrefab; 
    [HideInInspector] public List<CharacterInput> spawnedCharacters = new ();
    
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentLevel != null)
            RestartLevel();
    }

    public void ResetLevel()
    {
        foreach(var character in spawnedCharacters)
        {
            Destroy(character.gameObject);
        }
        spawnedCharacters.Clear();
        
        currentLevel.DestroyLevel();
        OnLevelDestroyed.Invoke();
    }

    public void WinLevel()
    {
        // UI stuff
        // gives rewards
        //loads next level 
        
        ResetLevel();
        GameManager.instance.StartRandomLevel();
    }

    public void LoadLevel(LevelObject levelObject)
    {
        if (currentLevel != null)
        {
           ResetLevel();
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

    private void SpawnPlayers(LevelObject levelObject)
    {
        for(int i = 0; i < 3; i++)
        {
            CharacterInput character = Instantiate(GameManager.instance.SelectedCharacterPrefabs[i]);

            character.transform.position = levelObject.spawnPositions[i].position;
            spawnedCharacters.Add(character);
        }

        CharacterSelectionManager.Instance.SetupCharacters(spawnedCharacters);
        CameraManager.Instance.SwitchCameraFollowTarget(spawnedCharacters[0].transform);
    }
}

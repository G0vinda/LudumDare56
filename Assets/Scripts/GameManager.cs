using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    public LevelList_SO levelList;
    public CharacterList_SO characterList;


    public static GameManager instance;




    public List<KeyValuePair<LevelObject, int>> pastLevels = new List<KeyValuePair<LevelObject, int>>();

    [Header("dont assign")]
    public List<CharacterInput> characterPrefabs = new List<CharacterInput>();

    public int currentRound = 0;



    public Transform rewardGrid;
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

    private IEnumerator Start()
    {

        yield return StartCoroutine(SquadSelection());

        StartRandomLevel();
        yield return null;


    }

    int choice = 0;


    public IEnumerator SquadSelection()
    {
        for (int i = 0; i < 3; i++)
        {

            CharacterReward reward1 = Instantiate(characterList.characterRewardPrefabs[Random.Range(0, characterList.characterRewardPrefabs.Count)], rewardGrid);
            CharacterReward reward2 = Instantiate(characterList.characterRewardPrefabs[Random.Range(0, characterList.characterRewardPrefabs.Count)], rewardGrid);
            choice = 0;
            yield return WaitForplayerChoice(reward1, reward2);

            if (choice == 1)
            {
                AddCharacterToRooster(reward1.characterPrefab);

            }
            if (choice == 2)
            {
                AddCharacterToRooster(reward2.characterPrefab);
            }

            Destroy(reward1.gameObject);
            Destroy(reward2.gameObject);

        }
    }

    IEnumerator WaitForplayerChoice(CharacterReward reward1, CharacterReward reward2)
    {
        reward1.selectButton.onClick.AddListener(() => { choice = 1; });
        reward2.selectButton.onClick.AddListener(() => { choice = 2; });

        while (choice == 0)
        {

            yield return null;
        }

        reward1.selectButton.onClick.RemoveAllListeners();
        reward2.selectButton.onClick.RemoveAllListeners();


    }

    public void AddCharacterToRooster(CharacterInput character)
    {
        if (characterPrefabs.Count >= 3)
        {
            Debug.LogError("there are 3 charactters already");
            return;
        }

        characterPrefabs.Add(character);

    }

    public void SwitchCharacterWithRooster(int characterIndex, CharacterInput character)
    {
        characterPrefabs[characterIndex] = character;
    }



    public void StartRandomLevel()
    {
        //select a random level from the list 
        // if its the same level with same difficulty from a past level choose another one 
        currentRound++;
        LevelObject levelobject;
        while (true)
        {
            levelobject = levelList.objects[Random.Range(0, levelList.objects.Count)];
            bool levelUnique = true;

            for (int i = 0; i < pastLevels.Count; i++)
            {
                if (pastLevels[i].Key == levelobject && pastLevels[i].Value == TranslateRoundToDifficulty(currentRound))
                {
                    levelUnique = false;
                    break;
                }

            }

            if (levelUnique) break;
        }

        LevelManager.instance.LoadLevel(levelobject);
        pastLevels.Add(new KeyValuePair<LevelObject, int>(levelobject, TranslateRoundToDifficulty(currentRound)));



    }



    public int TranslateRoundToDifficulty(int round)
    {
        if (round < 4) return 0;
        else if (round < 8) return 1;
        else if (round <= 10) return 2;

        return 2;


    }
}

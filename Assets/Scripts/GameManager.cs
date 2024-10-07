using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public LevelList_SO levelList;
    public int currentRound = 0;
    public Transform rewardGrid;
    public List<KeyValuePair<LevelObject, int>> pastLevels = new ();

    [SerializeField] private CharacterInput[] availableCharacterPrefabs;
    [SerializeField] private CharacterReward characterRewardPrefab;
    
    public List<CharacterInput> SelectedCharacterPrefabs { get; set; }
    
    public static GameManager instance;
    
    private int _characterChoice = 0;
    
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
    
    private IEnumerator SquadSelection()
    {
        for (int i = 0; i < 3; i++)
        {
            var characterOption1 = GetRandomCharacterPrefabExcept(null);
            var characterOption2 = GetRandomCharacterPrefabExcept(characterOption1);
            
            var reward1 = InstantiateCharacterReward(characterOption1);
            var reward2 = InstantiateCharacterReward(characterOption2);
            
            _characterChoice = 0;
            yield return WaitForPlayerChoice(reward1, reward2);

            if (_characterChoice == 1)
            {
                AddCharacterToRooster(reward1.characterPrefab);
            }
            if (_characterChoice == 2)
            {
                AddCharacterToRooster(reward2.characterPrefab);
            }

            Destroy(reward1.gameObject);
            Destroy(reward2.gameObject);
        }
    }

    private CharacterInput GetRandomCharacterPrefabExcept(CharacterInput except)
    {
        CharacterInput randomCharacter;
        do
        {
            randomCharacter = availableCharacterPrefabs[Random.Range(0, availableCharacterPrefabs.Length)];
        } while (randomCharacter == except);

        return randomCharacter;
    }

    private CharacterReward InstantiateCharacterReward(CharacterInput character)
    {
        var reward = Instantiate(characterRewardPrefab, rewardGrid);
        reward.Initialize(character);
        
        return reward;
    }

    private IEnumerator WaitForPlayerChoice(CharacterReward reward1, CharacterReward reward2)
    {
        reward1.selectButton.onClick.AddListener(() => { _characterChoice = 1; });
        reward2.selectButton.onClick.AddListener(() => { _characterChoice = 2; });

        while (_characterChoice == 0)
        {
            yield return null;
        }

        reward1.selectButton.onClick.RemoveAllListeners();
        reward2.selectButton.onClick.RemoveAllListeners();
    }

    public void AddCharacterToRooster(CharacterInput character)
    {
        if (SelectedCharacterPrefabs.Count >= 3)
        {
            Debug.LogError("there are 3 charactters already");
            return;
        }

        SelectedCharacterPrefabs.Add(character);
    }

    public void SwitchCharacterWithRooster(int characterIndex, CharacterInput character)
    {
        SelectedCharacterPrefabs[characterIndex] = character;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterList", menuName = "CharacterList")]
public class CharacterList_SO : ScriptableObject
{
    public List<CharacterInput> characterPrefabs = new List<CharacterInput>();
    public List<CharacterReward> characterRewardPrefabs = new List<CharacterReward>();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "LevelList")]
public class LevelList_SO : ScriptableObject
{
   public List<LevelObject> objects;
}

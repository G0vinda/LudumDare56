using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{


    [Header("Need exactly 3")]
    public Transform[] spawnPositions;
    public LevelGoal levelGoal;

    public LevelObstacle[] obstacles;
    public LevelBeneficiary[] beneficiaries;




    public void SetupLevel(int difficulty)
    {


        // activate obstacles 



    }


    public void DestoryLevel()
    {
        Destroy(this.gameObject);
    }

}


[System.Serializable]
public class LevelObstacle
{
    public GameObject obstacle;


    public bool existsInEasyDifficulty;
    public bool existsInMediumDifficulty;
    public bool existsInHardDifficulty;


}
[System.Serializable]
public class LevelBeneficiary {

    public GameObject beneficiary;

}
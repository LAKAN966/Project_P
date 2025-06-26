using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    private static StageManager instance;

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StageManager();
            }
            return instance;
        }
    }
    //public int GetStageID(int index) // 이게 의미 있는건가?
    //{

    //    var stage = StageDataManager.Instance.GetStageData(id);

    //    return stage.ID;
    //}




}

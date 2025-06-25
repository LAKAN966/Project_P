using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    // Start is called before the first frame update
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close(/*bool kill = false*/)
    {
        //if (kill)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        gameObject.SetActive(false);
    }
}

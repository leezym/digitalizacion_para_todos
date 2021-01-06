using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public void EnterTutorial(bool _in)
    {
        GetComponent<Animator>().SetBool("in", _in);
        if (!_in)
        {
            BookManager.Instance.rightHotSpot.raycastTarget = true;
        }
    }
}

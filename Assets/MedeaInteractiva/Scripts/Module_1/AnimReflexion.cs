using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimReflexion : MonoBehaviour
{

    public void StartAnim()
    {
        BookManager.Instance.contentInputReflexion.SetActive(false);
    }

    public void PopupReflexion()
    {
        BookManager.Instance.PopupReflexion(true);
    }
}

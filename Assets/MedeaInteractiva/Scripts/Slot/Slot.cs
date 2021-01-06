using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Slot : MonoBehaviour, IDropHandler {

    public int totalChilds = 4;

	public GameObject Piece
    {
        get
        {
            //if(transform.childCount > 0)
            //{
            //    return transform.GetChild(0).gameObject;
            //}
            if (transform.childCount == totalChilds)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Piece)
        {
            MSystemHandler.PieceBegingDragged.transform.SetParent(transform);
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x,y) => x.HasChanged());
        }
    }
}

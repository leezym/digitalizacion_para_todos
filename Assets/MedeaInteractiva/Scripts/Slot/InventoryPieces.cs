using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class InventoryPieces : MonoBehaviour, IHasChanged
{
    public Transform slots;

    public Transform slotPieces;

    public string inventoryText;

    public UnityEvent OnDrop;

    public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        foreach (Transform slotTransform in slots)
        {
            GameObject piece = slotTransform.GetComponent<Slot>().Piece;
            if (piece)
            {
                builder.Append(piece.name);
                builder.Append("-");
            }
        }
        inventoryText = builder.ToString();

        if(OnDrop != null)
        {
            OnDrop.Invoke();
        }
    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}

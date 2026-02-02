using UnityEngine;
using UnityEngine.EventSystems;

public class Shuffle : Booster, IPointerDownHandler
{
    public override void OnEndDrag(PointerEventData eventData)
    {

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
     
    }

    public override void OnDrag(PointerEventData eventData)
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameEvents.RequestNewShapes();
        BackToStartingPosition();
    }
}

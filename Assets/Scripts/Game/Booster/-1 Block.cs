using UnityEngine;
using UnityEngine.EventSystems;

public class MinusOneBlock : Booster
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            BackToStartingPosition();
            Debug.Log("no object to enter");
            return;
        }

        GameObject target = eventData.pointerEnter;


        Transform shapeTransform = target.transform;
        while (shapeTransform != null && !shapeTransform.CompareTag("Shape"))
        {
            shapeTransform = shapeTransform.parent;
        }

        if (shapeTransform != null)
        {
            Debug.Log("Found shape");


            foreach (Transform child in shapeTransform)
            {
                if (child.name.Contains("SquareImage"))
                {
                    child.gameObject.SetActive(false);
                }
            }
            BackToStartingPosition();
        }
        else
        {
            BackToStartingPosition();
        }
    }

}

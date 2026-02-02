using JetBrains.Annotations;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Booster : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    protected Vector3 _startPosition;
    protected RectTransform _transform;

    protected Canvas _canvas;
    protected CanvasGroup _canvasGroup;
    protected virtual void Awake()
    {
        _startPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        _transform = this.GetComponent<RectTransform>();
        _canvas = transform.parent.GetComponentInParent<Canvas>();
        _canvasGroup = this.GetComponent<CanvasGroup>();
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        // GameObject shape = eventData.pointerEnter.gameObject;

        // if (shape.tag == "Shape")
        // {
        //     foreach (Transform child in gameObject.transform)
        //     {
        //         child.gameObject.SetActive(false);
        //     }
        // }
        // else
        // {
        //     transform.GetComponent<RectTransform>().anchoredPosition = _startPosition;
        // }
    }

    public void BackToStartingPosition()
    {
        _transform.anchoredPosition = _startPosition;
    }
}

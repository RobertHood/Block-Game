using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GridSquare : MonoBehaviour
{
    public Image _hoverImage;
    public Image _boosterHoverImage;
    public Image _activeImage;
    public Image _normalImage;
    public List<Sprite> _normalImages;
    public bool Selected{ get; set; }
    public int SquareIndex{ get; set;}
    public bool SquareOccupied{ get; set;}

    private RectTransform _rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Selected = false;
        SquareOccupied = false;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetImage(bool _setFirstImage)
    {
        _normalImage.GetComponent<Image>().sprite = _setFirstImage ? _normalImages[0] : _normalImages[1];
    }

    public void Highlight(bool enable)
    {
        _hoverImage.gameObject.SetActive(enable);
        Selected = enable;
    }

    public void BoosterHighlight(bool enable)
    {
        _boosterHoverImage.gameObject.SetActive(enable);
        Selected = enable;
    }

    public bool CanUseThisSquare()
    {
        return _hoverImage.gameObject.activeSelf;
    }

    public void ActivateSquare()
    {
        _hoverImage.gameObject.SetActive(false);
        _activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate(){
        _activeImage.gameObject.SetActive(false);
        Selected = false;
        SquareOccupied = false;
    }
    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }
}

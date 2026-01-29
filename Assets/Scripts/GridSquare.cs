using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridSquare : MonoBehaviour
{
    public Image _normalImage;
    public List<Sprite> _normalImages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(bool _setFirstImage)
    {
        _normalImage.GetComponent<Image>().sprite = _setFirstImage ? _normalImages[0] : _normalImages[1];
    }
}

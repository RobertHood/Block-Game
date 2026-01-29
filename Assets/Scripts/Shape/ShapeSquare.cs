using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
}

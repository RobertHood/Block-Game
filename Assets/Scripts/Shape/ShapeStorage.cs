using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }

    void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }
    public Shape getCurrentSelectedShape()
    {
        foreach(var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        Debug.LogError("no shape selected");
        return null;
    }

    public void RequestNewShapes()
    {
        foreach (var s in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            s.RequestNewShape(shapeData[shapeIndex]);
        }
    }
}
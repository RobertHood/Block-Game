using UnityEngine;
using UnityEditor;
using NUnit.Framework.Constraints;
using System.Runtime.Remoting.Messaging;
using Unity.VectorGraphics;
using UnityEditor.Build.Content;

[CustomEditor(typeof(ShapeData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();

        if (ShapeDataInstance.board != null && ShapeDataInstance.column > 0 && ShapeDataInstance.row > 0) 
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    private void DrawColumnsInputFields()
    {
        var columnsTemp = ShapeDataInstance.column;
        var rowsTemp = ShapeDataInstance.row;

        ShapeDataInstance.column = EditorGUILayout.IntField("Columns", ShapeDataInstance.column);
        ShapeDataInstance.row = EditorGUILayout.IntField("Rows", ShapeDataInstance.row);

        if (ShapeDataInstance.column != columnsTemp || ShapeDataInstance.row != rowsTemp && ShapeDataInstance.column > 0 && ShapeDataInstance.row > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }
    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle();
        tableStyle.padding = new RectOffset(10,10,10,10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row = 0; row < ShapeDataInstance.row; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for(var column = 0; column < ShapeDataInstance.column; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row]._column[column], dataFieldStyle);
                ShapeDataInstance.board[row]._column[column] = data;
                EditorGUILayout.EndHorizontal(); 
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] GameObject _prefabToPlace;
    [SerializeField] Texture2D _mazeTexture;
    [SerializeField] float _xOffset=1;
    [SerializeField] float _zOffset=1;
    [SerializeField] int _width = 32;
    [SerializeField] int _height = 32;

    [SerializeField] Transform _mazeParent;

    public void GenerateMaze()
    {

        var allPixels = _mazeTexture.GetPixels();
        Debug.Log(allPixels.Length);
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                //Debug.Log(allPixels[i + _width * j]);
                Color color = allPixels[i + _width * j];
                if (color.r < 0.5f && color.g < 0.5f && color.b < 0.5f)
                {
                    var obj = Instantiate(_prefabToPlace, _mazeParent);
                    obj.transform.position = new Vector3(i * _xOffset, 2.5f, j * _zOffset);
                }
            }
        }
    }

    public void DestroyMaze()
    {
        foreach (var tr in _mazeParent.GetComponentsInChildren<Transform>())
        {
            if (tr != _mazeParent)
                DestroyImmediate(tr.gameObject);

        }
    }
}

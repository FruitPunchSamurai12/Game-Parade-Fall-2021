using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MazeGenerator myScript = (MazeGenerator)target;
        if (GUILayout.Button("Generate Maze"))
        {
            myScript.GenerateMaze();
        }
        if (GUILayout.Button("Destroy Maze"))
        {
            myScript.DestroyMaze();
        }
    }
}
#endif

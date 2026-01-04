using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelCreator : MonoBehaviour
{
    [Header("Save Settings")]
    public string savePath = "Assets/Resources/Levels/";
    public string levelName = "Level_New";

    public LineController lineController;
    public int width = 4;
    public int height = 4;
#if UNITY_EDITOR
    [ContextMenu("Save Current Board to File")]
    public void SaveLevel()
    {
        LevelDataSO levelAsset = ScriptableObject.CreateInstance<LevelDataSO>();
        levelAsset.width = width;
        levelAsset.height = height;
        levelAsset.dots = new List<LevelDataSO.DotPair>();

        foreach(var kvp in lineController.GetLines())
        {
            LineRenderer lr = kvp.Value.GetComponent<LineRenderer>();

            if(lr.positionCount >= 2)
            {
                LevelDataSO.DotPair pair = new LevelDataSO.DotPair();
                pair.color = kvp.Key;

                Vector3 startWorld = lr.GetPosition(0);
                Vector3 endWorld = lr.GetPosition(lr.positionCount - 1);

                pair.pos1 = WorldToGrid(startWorld);
                pair.pos2 = WorldToGrid(endWorld);
                levelAsset.dots.Add(pair);
            }
        }

        if(!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        string fullPath = $"{savePath}/{levelName}.asset";
        AssetDatabase.CreateAsset(levelAsset, fullPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"<color=green>Đã lưu level thành công: {fullPath}</color>");
        Selection.activeObject = levelAsset;

    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        // Giả sử offset tính như các bài trước
        float offsetX = (width - 1) * 0.5f;
        float offsetY = (height - 1) * 0.5f;

        int x = Mathf.RoundToInt(worldPos.x + offsetX);
        int y = Mathf.RoundToInt(worldPos.y + offsetY);
        return new Vector2Int(x, y);
    }
#endif
}

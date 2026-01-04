using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leve_00", menuName = "ConnectTheDots/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public int LevelID;
    public int width = 4;
    public int height = 4;
    [System.Serializable]
    public struct DotPair
    {
        public Constants.COLOR color;
        public Vector2Int pos1;
        public Vector2Int pos2;
    }
    public List<DotPair> dots = new List<DotPair>();
}

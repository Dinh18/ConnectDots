using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private BoardController board;
    private InputCotroller input;
    private LineController line;
    public List<LevelDataSO> allLevels;
    public int CurrentLevelIndex = 0;
    void Awake()
    {
        board = GameObject.Find("Board Holder").GetComponent<BoardController>();
        input = GameObject.Find("Input Controller").GetComponent<InputCotroller>();
        line = GameObject.Find("Line Holder").GetComponent<LineController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex",0);
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BoardController GetBoard() => board;
    private void LoadLevel()
    {
        if(CurrentLevelIndex < allLevels.Count)
        {
            LevelDataSO data = allLevels[CurrentLevelIndex];

            LoadLevelData(data);

            line.Setup(this);

            input.SetUp(board,line,this);
        }

    }

    private void LoadLevelData(LevelDataSO data)
    {
        board.Setup(data.width, data.height);
        foreach(var dot in data.dots)
        {
            board.SetupStartDots(dot.color, dot.pos1);
            board.SetupStartDots(dot.color, dot.pos2);
        }
    }

    public void Won()
    {
        for(int x = 0; x < board.GetWidth(); x++)
        {
            for(int y = 0; y < board.GetHeight(); y++)
            {
                Vector2Int pos = new Vector2Int(x,y);
                if(board.GetCellAtPosition(pos).GetCellColor() == Constants.COLOR.WHITE)
                {
                     Debug.Log("No");
                     return;
                }
        }
        Debug.Log("Win");
        }
    }
}
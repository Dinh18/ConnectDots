using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        LevelCompleted,
        Paused
    }
    public GameState currentgameState = GameState.MainMenu;
    private BoardController board;
    private InputCotroller input;
    private LineController line;
    private CameraAdapter cameraAdapter;
    private UIController uiController;
    public List<LevelDataSO> allLevels;
    public int CurrentLevelIndex = 0;
    void Awake()
    {
        board = GameObject.Find("Board Holder").GetComponent<BoardController>();
        input = GameObject.Find("Input Controller").GetComponent<InputCotroller>();
        line = GameObject.Find("Line Holder").GetComponent<LineController>();
        cameraAdapter = GameObject.Find("Main Camera").GetComponent<CameraAdapter>();
        uiController = FindObjectOfType<UIController>();
        uiController.Setup(this);
        

        CurrentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex",0);

        SetState(GameState.MainMenu);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentgameState == GameState.Playing)
        {
            input.HandleInput();
        }
    }

    public void SetState(GameState gameState)
    {
        this.currentgameState = gameState;
        if(uiController != null)
        {
            uiController.UpdateUI(gameState);
        }
    }

    public LevelDataSO GetLevelData(int level)
    {
        return allLevels[level];
    }

    public BoardController GetBoard() => board;
    public void LoadLevel()
    {
        
        if(CurrentLevelIndex < allLevels.Count)
        {
            LevelDataSO data = allLevels[CurrentLevelIndex];

            LoadLevelData(data);

            line.Setup(this,CurrentLevelIndex);

            input.SetUp(board,line,this);
        }

        SetState(GameState.Playing);

    }

    public void LoadNextLevel()
    {
        CurrentLevelIndex++;
        LoadLevel();
    }

    private void LoadLevelData(LevelDataSO data)
    {
        board.Setup(data.width, data.height);
        foreach(var dot in data.dots)
        {
            board.SetupStartDots(dot.color, dot.pos1);
            board.SetupStartDots(dot.color, dot.pos2);
        }
        cameraAdapter.AdjustCamera(data.width, data.height);
    }

    // public void Won()
    // {
    //     for(int x = 0; x < board.GetWidth(); x++)
    //     {
    //         for(int y = 0; y < board.GetHeight(); y++)
    //         {
    //             Vector2Int pos = new Vector2Int(x,y);
    //             if(board.GetCellAtPosition(pos).GetCellColor() == Constants.COLOR.WHITE)
    //             {
    //                 Debug.Log("No");s
    //                 return;
    //             }
    //     }
    //     CurrentLevelIndex++;
    //     LoadLevel();
    //     }
    // }
}
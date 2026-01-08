using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform boardHolder;
    private float cellGap = 0.2f;
    private GameObject[,] cells;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Setup(width, height);
        // GenerateBoard();
        // TestStartGame();
        // DebugColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetHeight() => height;
    public void SetHeight(int height) => this.height = height;
    public int GetWidth() => width;
    public void SetWidth(int width) => this.width = width;
    public Cell GetCellAtPosition(Vector2Int pos) => cells[pos.x, pos.y].GetComponent<Cell>();


    public void Setup(int width, int height)
    {
        this.width = width;
        this.height = height;

        foreach(Transform child in boardHolder)
        {
            Destroy(child.gameObject);
        }

        cells = new GameObject[width, height];
        Vector2 offset = new Vector2 (0,0);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject newCell = Instantiate(cellPrefab, boardHolder);
                newCell.transform.position = new Vector3(x + offset.x, y + offset.y,0);
                newCell.name = $"Cell {x} {y}";
                newCell.transform.localScale = Vector3.one * (1 - cellGap);

                cells[x, y] = newCell;
            }
        }
    }

    // public void TestStartGame()
    // {
    //     SetStartDots(Constants.COLOR.RED, new Vector2Int(0,0));
    //     SetStartDots(Constants.COLOR.RED, new Vector2Int(3,3));

    //     SetStartDots(Constants.COLOR.BLUE, new Vector2Int(1,2));
    //     SetStartDots(Constants.COLOR.BLUE, new Vector2Int(2,3));
    // }

    public void SetupStartDots(Constants.COLOR color, Vector2Int pos1)
    {
        GameObject dot = Instantiate(dotPrefab);
        dot.transform.SetParent(boardHolder);
        dot.transform.position = cells[pos1.x, pos1.y].transform.position;
        cells[pos1.x, pos1.y].GetComponent<Cell>().SetCellColor(color);
        dot.GetComponent<SpriteRenderer>().color = Constants.GetColorString(color);
        cells[pos1.x, pos1.y].GetComponent<Cell>().SetStartDot(true);
    }

    

    public void DebugColor()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {

                Debug.Log(cells[x,y].GetComponent<Cell>().GetCellColor());
                Debug.Log(cells[x,y].GetComponent<Cell>().IsStartDot());
            }
        }
    }
}

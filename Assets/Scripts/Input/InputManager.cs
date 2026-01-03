using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2Int startPos;
    private Vector2Int currentPos;
    private Vector2Int lastPos;
    private BoardController board;
    private LineController lineController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManagerSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
    }

    public void InputManagerSetUp()
    {
        board = GameObject.Find("BoardHolder").GetComponent<BoardController>();
        lineController = GameObject.Find("Line Holder").GetComponent<LineController>();
    }
    
    private void HandleInput()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = GetWorldPosition(touch.position);
            if(touch.phase == TouchPhase.Began)
            {
                ProcessInputBegan(touchPos);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                ProcessInputMoved(touchPos);
            }
        }
    }

    private void ProcessInputBegan(Vector3 touchPos)
    {
        startPos = Vector2Int.RoundToInt(touchPos);
        lastPos = startPos;
        if(IsValidCell(startPos))
        {
            EnterStartCell(startPos);
        }
    }

    private void ProcessInputMoved(Vector3 touchPos)
    {
        currentPos = Vector2Int.RoundToInt(touchPos);
        if(currentPos != lastPos && IsValidCell(currentPos))
        {
            EnterCell(lastPos, currentPos);
            lastPos = currentPos;
        }
    }

    private Vector3 GetWorldPosition(Vector3 screenPos)
    {
        screenPos.z = -mainCamera.transform.position.z; 
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private void EnterStartCell(Vector2Int startPos)
    {
        if(board.GetCellAtPosition(startPos).GetCellColor() != Constants.COLOR.WHITE)
        {
            lineController.DrawLine(board.GetCellAtPosition(startPos).GetCellColor(), startPos);
            return;
        }
    }

    private void EnterCell(Vector2Int lastPos, Vector2Int currentPos)
    {
        Constants.COLOR lastColorCell = board.GetCellAtPosition(lastPos).GetCellColor();
        Debug.Log("Last Color: " + lastColorCell);
        if(lastColorCell != Constants.COLOR.WHITE)
        {
            
            lineController.DrawLine(lastColorCell, currentPos);
            board.GetCellAtPosition(currentPos).SetCellColor(lastColorCell);
        }
    }
    private bool IsValidCell(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < board.GetWidth() && pos.y >= 0 && pos.y < board.GetHeight();
    }

}



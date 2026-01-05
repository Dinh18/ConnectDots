using UnityEngine;

public class InputCotroller : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2Int startPos;
    private Vector2Int currentPos;
    private Vector2Int lastPos;
    private BoardController board;
    private LineController lineController;
    private GameController gameController;
    [Header("Editor Mode")]
    public bool isEditorMode = false; // Bật cái này lên khi muốn tự vẽ level
    public Constants.COLOR editorCurrentColor = Constants.COLOR.RED; // Màu đang chọn để vẽ

    void HandleInputEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(worldPos.x);
            int y = Mathf.RoundToInt(worldPos.y);
                currentPos = new Vector2Int(x, y);
            if (IsValidCell(currentPos))
            {
                lastPos = currentPos;

                // --- LOGIC MỚI Ở ĐÂY ---
                if (isEditorMode)
                {
                    // Nếu là chế độ Editor: Bấm vào đâu cũng vẽ được!
                    // Tự động gọi hàm tạo dây mới với màu đang chọn
                    lineController.CreateLine(editorCurrentColor, lastPos, currentPos, board);
                
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        // HandleInputEditor();

    }

    public void SetUp(BoardController board, LineController lineController, GameController gameController)
    {
        this.board = board;
        this.lineController = lineController;
        this.gameController = gameController;
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
            lineController.DrawLine(board.GetCellAtPosition(startPos).GetCellColor(), startPos, startPos, board);
        }
    }

    private void EnterCell(Vector2Int lastPos, Vector2Int currentPos)
    {
        Constants.COLOR lastColorCell = board.GetCellAtPosition(lastPos).GetCellColor();
        Debug.Log("Last Color: " + lastColorCell);
        if(lastColorCell != Constants.COLOR.WHITE)
        {
            lineController.DrawLine(lastColorCell, lastPos, currentPos, board);
        }
    }
    private bool IsValidCell(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < board.GetWidth() && pos.y >= 0 && pos.y < board.GetHeight();
    }

}



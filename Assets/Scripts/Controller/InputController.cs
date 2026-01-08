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
    private Constants.COLOR currentDrawingColor; // Lưu màu đang vẽ
    private bool isDragging = false; // Kiểm soát trạng thái kéo
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
    // void Start()
    // {
    //     board = GameObject.Find("BoardHolder").GetComponent<BoardController>();
    //     lineController = GameObject.Find("Line Holder").GetComponent<LineController>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     // HandleInput();
    //     HandleInputEditor();

    // }

    public void SetUp(BoardController board, LineController lineController, GameController gameController)
    {
        this.board = board;
        this.lineController = lineController;
        this.gameController = gameController;
    }
    
    public void HandleInput()
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
        if (!IsValidCell(startPos)) return;
        Constants.COLOR cellColor = board.GetCellAtPosition(startPos).GetCellColor();
        if(board.GetCellAtPosition(startPos).IsStartDot())
        {
            lineController.ResetLine(cellColor, startPos,board);

        }
        
        // Chỉ cho phép vẽ nếu bấm vào Dot hoặc Dây đã có màu
        if (cellColor != Constants.COLOR.WHITE)
        {
            isDragging = true;
            currentDrawingColor = cellColor; // <--- QUAN TRỌNG: Lưu màu lại ngay!
            
            // Gọi vẽ điểm đầu để khởi tạo dây
            lineController.DrawLine(currentDrawingColor, startPos, startPos, board);
        }
    }

    private void ProcessInputMoved(Vector3 touchPos)
    {
        if (!isDragging) return;

        currentPos = Vector2Int.RoundToInt(touchPos);

        // Chỉ xử lý khi sang ô mới và ô đó nằm trong bảng
        if (currentPos != lastPos && IsValidCell(currentPos))
        {
            Cell currentCell = board.GetCellAtPosition(currentPos);
            
            // --- 1. LOGIC ĐỔI MÀU (NẾU CHẠM VÀO DOT KHÁC) ---
            if (currentCell.IsStartDot() && currentCell.GetCellColor() != currentDrawingColor)
            {
                return; 
            }

            // --- 2. LOGIC VẼ THƯỜNG (QUAN TRỌNG) ---
            // Gọi hàm vẽ và HỨNG KẾT QUẢ TRẢ VỀ (bool)
            bool drawSuccess = lineController.DrawLine(currentDrawingColor, lastPos, currentPos, board);
            
            // --- CHỈ CẬP NHẬT lastPos NẾU VẼ THÀNH CÔNG ---
            // Nếu vẽ thất bại (do đi chéo), lastPos GIỮ NGUYÊN là ô cũ.
            if (drawSuccess) 
            {
                lastPos = currentPos;
            }
        }
    }

    private Vector3 GetWorldPosition(Vector3 screenPos)
    {
        screenPos.z = -mainCamera.transform.position.z; 
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private bool IsValidCell(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < board.GetWidth() && pos.y >= 0 && pos.y < board.GetHeight();
    }

}



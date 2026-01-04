using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private int numberOfCoupleOfDots;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject lrHolder;
    private Dictionary<Constants.COLOR, GameObject> lines = new Dictionary<Constants.COLOR, GameObject>(); 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<Constants.COLOR, GameObject> GetLines() => lines;

    /// <summary>
    /// Nếu chưa có và là điểm xuất phát thì thêm line mới
    /// Nếu có rồi
    ///     Nếu là cell bình thường thì thêm điểm mới vào line
    ///     Nếu điểm trước đó là điểm xuất phát thì xóa hết điểm cũ và thêm điểm xuất phát và thêm điểm hiện tại vào
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>

    public void DrawLine(Constants.COLOR color, Vector2Int lastPos, Vector2Int currentPos, BoardController board)
    {
        LineRenderer lr;
        if (lines.TryGetValue(color, out GameObject lineObj))
        {
            lr = lineObj.GetComponent<LineRenderer>();
        }
        else
        {
            // Tạo mới nếu chưa có
            lineObj = Instantiate(linePrefab, lrHolder.transform);
            lines.Add(color, lineObj);
            
            lr = lineObj.GetComponent<LineRenderer>();
            lr.startColor = Constants.GetColorString(color);
            lr.endColor = Constants.GetColorString(color);
            
            // Khởi tạo điểm đầu tiên
            lr.positionCount = 1;
            lr.SetPosition(0, new Vector3(currentPos.x, currentPos.y, -5f));
            return;
        }

        if(lastPos == currentPos)
        {
            return; // Nếu điểm hiện tại trùng với điểm cuối cùng của line thì không làm gì
        }

        int dx = Mathf.Abs(currentPos.x - lastPos.x);
        int dy = Mathf.Abs(currentPos.y - lastPos.y);

        if(dx + dy != 1)
        {
            return; // Chỉ chấp nhận di chuyển ngang hoặc dọc một ô
        }
        

        if(board.GetCellAtPosition(lastPos).IsStartDot())
        {
            for(int i = 1; i < lr.positionCount - 1; i++)
            {
                board.GetCellAtPosition(new Vector2Int((int)lr.GetPosition(i).x, (int)lr.GetPosition(i).y)).SetCellColor(Constants.COLOR.WHITE);
            }

            // Xóa hết điểm cũ, chỉ giữ lại điểm xuất phát
            lr.positionCount = 1;
            lr.SetPosition(0, new Vector3(lastPos.x, lastPos.y, -5f));
        }

        if(board.GetCellAtPosition(currentPos).GetCellColor() != Constants.COLOR.WHITE && board.GetCellAtPosition(currentPos).GetCellColor() != color && !board.GetCellAtPosition(currentPos).IsStartDot())
        {
            LineRenderer otherLR;
            if (lines.TryGetValue(board.GetCellAtPosition(currentPos).GetCellColor(), out GameObject otherLineObj))
            {
                otherLR = otherLineObj.GetComponent<LineRenderer>();
                for(int i = 0; i < otherLR.positionCount; i++)
                {
                    Vector2Int pos = new Vector2Int((int)otherLR.GetPosition(i).x, (int)otherLR.GetPosition(i).y);
                    if(!board.GetCellAtPosition(pos).IsStartDot()) board.GetCellAtPosition(pos).SetCellColor(Constants.COLOR.WHITE);
                }
                otherLR.positionCount = 0;
            }
        }

        if(board.GetCellAtPosition(currentPos).GetCellColor() == color || board.GetCellAtPosition(currentPos).GetCellColor() == Constants.COLOR.WHITE)
        {
            Vector3 newPos = new Vector3(currentPos.x, currentPos.y, -5f);

            // 2. Tìm xem điểm này đã tồn tại trong line chưa? (Để xử lý Backtrack)
            // Dùng -1 để đánh dấu là "Chưa tìm thấy"
            int foundIndex = -1;

            for (int i = 0; i < lr.positionCount - 1; i++)
            {
                // So sánh vị trí (Lưu ý: float nên so sánh khoảng cách nhỏ, hoặc ép kiểu nếu chắc chắn là grid)
                if (Vector3.Distance(lr.GetPosition(i), newPos) < 0.01f)
                {
                    foundIndex = i;
                    break; // Tìm thấy điểm cũ đầu tiên thì dừng ngay
                }
            }
            if (foundIndex != -1)
            {
                // TRƯỜNG HỢP BACKTRACK (Đi lùi)
                // Cắt line về đúng vị trí tìm thấy.
                // Ví dụ: Line [0, 1, 2, 3], chạm vào 1 -> Giữ lại [0, 1] -> Count mới là 2 (index + 1)
                for(int i = foundIndex + 1; i < lr.positionCount; i++)
                {
                    board.GetCellAtPosition(new Vector2Int((int)lr.GetPosition(i).x, (int)lr.GetPosition(i).y)).SetCellColor(Constants.COLOR.WHITE);
                }
                lr.positionCount = foundIndex + 1;
                
                // Cập nhật lại vị trí điểm cuối cho chính xác tuyệt đối (phòng trường hợp sai số nhỏ)
                lr.SetPosition(foundIndex, newPos);
            }
            else
            {
                // TRƯỜNG HỢP VẼ MỚI
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, newPos);
                board.GetCellAtPosition(currentPos).SetCellColor(color);
            }
            
        }

    }
    // Trong LineManager.cs (hoặc nơi chứa hàm DrawLine)

public void CreateLine(Constants.COLOR color, Vector2Int lastPos, Vector2Int currentPos, BoardController board)
{
    // Logic tạo dây mới (như các bài trước đã làm)
    LineRenderer lr;
        if (lines.TryGetValue(color, out GameObject lineObj))
        {
            lr = lineObj.GetComponent<LineRenderer>();
        }
        else
        {
            // Tạo mới nếu chưa có
            lineObj = Instantiate(linePrefab, lrHolder.transform);
            lines.Add(color, lineObj);
            
            lr = lineObj.GetComponent<LineRenderer>();
            lr.startColor = Constants.GetColorString(color);
            lr.endColor = Constants.GetColorString(color);
            
            // Khởi tạo điểm đầu tiên
            lr.positionCount = 1;
            lr.SetPosition(0, new Vector3(currentPos.x, currentPos.y, -5f));
            return;
        }

    // QUAN TRỌNG: Trong Editor Mode, khi đặt bút xuống,
    // ta coi như ô đó là điểm bắt đầu luôn.
    // Gọi BoardController để đánh dấu ô này có màu (để hiển thị lên)
    board.GetCellAtPosition(currentPos).SetCellColor(color);
}
}
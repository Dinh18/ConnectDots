using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject lrHolder;
    [SerializeField] private Dictionary<Constants.COLOR, GameObject> lines = new Dictionary<Constants.COLOR, GameObject>(); 
    private GameController gameController;

    public Dictionary<Constants.COLOR, GameObject> GetLines() => lines;

    public static event Action<int> OnLineStep;
    public static event Action OnLineCompleted;
    public static event Action OnLineCut;
    public static event Action OnLineError;
    public static event Action OnLevelCompleted;

    public void Setup(GameController gameController, int currentLevel)
    {
        foreach(var kvp in lines)
        {
            if(kvp.Value != null)
            {
                Destroy(kvp.Value.gameObject);
            }
        }
        lines.Clear();

        this.gameController = gameController;

        foreach(var dot in gameController.GetLevelData(currentLevel).dots)
        {
            GameObject lineObj = Instantiate(linePrefab, lrHolder.transform);
            
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            lr.startColor = Constants.GetColorString(dot.color);
            lr.endColor = Constants.GetColorString(dot.color);
            lr.positionCount = 0;

            lines.Add(dot.color, lineObj);
        }
    }

    public void ResetLine(Constants.COLOR color, Vector2Int pos,BoardController board)
    {
        if (!lines.ContainsKey(color)) return;
        GameObject lineObj = lines[color];
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        for(int i = 0; i < lr.positionCount; i++)
        {
            int px = Mathf.RoundToInt(lr.GetPosition(i).x);
            int py = Mathf.RoundToInt(lr.GetPosition(i).y);
            Vector2Int poslr = new Vector2Int(px,py);
            if(!board.GetCellAtPosition(poslr).IsStartDot()) board.GetCellAtPosition(poslr).SetCellColor(Constants.COLOR.WHITE);
        }
        lr.positionCount = 1;
        lr.SetPosition(0, new Vector3(pos.x, pos.y, -5));
    }

    public void CutLine(Constants.COLOR color, Vector2Int pos,BoardController board)
    {
        int index = -1;
        if (!lines.ContainsKey(color)) return;
        GameObject lineObj = lines[color];
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        for(int i = 0; i < lr.positionCount; i++)
        {
            int px = Mathf.RoundToInt(lr.GetPosition(i).x);
            int py = Mathf.RoundToInt(lr.GetPosition(i).y);
            if(px == pos.x && py == pos.y)
            {
                index = i;
                break;
            }
        }
        if(index != -1)
        {
            for(int i = lr.positionCount - 1; i > index; i--)
            {
                int px = Mathf.RoundToInt(lr.GetPosition(i).x);
                int py = Mathf.RoundToInt(lr.GetPosition(i).y);
                Vector2Int poslr = new Vector2Int(px, py);
                if(!board.GetCellAtPosition(poslr).IsStartDot()) board.GetCellAtPosition(poslr).SetCellColor(Constants.COLOR.WHITE);
            }
            lr.positionCount = index + 1;
            OnLineCut?.Invoke();
        }
    }

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

    public bool DrawLine(Constants.COLOR color, Vector2Int lastPos, Vector2Int currentPos, BoardController board)
    {

        if (!lines.ContainsKey(color)) return false;
        GameObject lineObj = lines[color];
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();

        // if (lr.positionCount == 0)
        // {
        //     lr.positionCount = 1;
        //     lr.SetPosition(0, new Vector3(currentPos.x, currentPos.y, -5f));
        //     board.GetCellAtPosition(currentPos).SetCellColor(color);
        //     return;
        // }
        


        if(lastPos == currentPos)
        {
            return false; // Nếu điểm hiện tại trùng với điểm cuối cùng của line thì không làm gì
        }


        int dx = Mathf.Abs(currentPos.x - lastPos.x);
        int dy = Mathf.Abs(currentPos.y - lastPos.y);

        if(dx + dy != 1 || board.GetCellAtPosition(currentPos).GetCellColor() == Constants.COLOR.WHITE)
        {
            return false; // Chỉ chấp nhận di chuyển ngang hoặc dọc một ô
        }
        

        if(board.GetCellAtPosition(lastPos).IsStartDot())
        {
            for(int i = 0; i < lr.positionCount; i++)
            {
                int px = Mathf.RoundToInt(lr.GetPosition(i).x);
                int py = Mathf.RoundToInt(lr.GetPosition(i).y);
                Vector2Int pos = new Vector2Int(px, py);
                
                if(!board.GetCellAtPosition(pos).IsStartDot()) board.GetCellAtPosition(pos).SetCellColor(Constants.COLOR.WHITE);
            }

            // Xóa hết điểm cũ, chỉ giữ lại điểm xuất phát
            lr.positionCount = 1;
            lr.SetPosition(0, new Vector3(lastPos.x, lastPos.y, -5f));
        }

        if(board.GetCellAtPosition(currentPos).GetCellColor() != Constants.COLOR.WHITE 
            && board.GetCellAtPosition(currentPos).GetCellColor() != color 
            && !board.GetCellAtPosition(currentPos).IsStartDot())
        {
            LineRenderer otherLR;
            if (lines.TryGetValue(board.GetCellAtPosition(currentPos).GetCellColor(), out GameObject otherLineObj))
            {
                otherLR = otherLineObj.GetComponent<LineRenderer>();
                for(int i = 0; i < otherLR.positionCount; i++)
                {
                    int px = Mathf.RoundToInt(otherLR.GetPosition(i).x);
                    int py = Mathf.RoundToInt(otherLR.GetPosition(i).y);
                    Vector2Int pos = new Vector2Int(px,py);
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
                if (Vector3.Distance(lr.GetPosition(i), newPos) < 0.5f)
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
                    int px = Mathf.RoundToInt(lr.GetPosition(i).x);
                    int py = Mathf.RoundToInt(lr.GetPosition(i).y);
                    Vector2Int pos = new Vector2Int(px,py);
                    if(!board.GetCellAtPosition(pos).IsStartDot()) board.GetCellAtPosition(pos).SetCellColor(Constants.COLOR.WHITE);
                }
                lr.positionCount = foundIndex + 1;
                
                // Cập nhật lại vị trí điểm cuối cho chính xác tuyệt đối (phòng trường hợp sai số nhỏ)
                lr.SetPosition(foundIndex, newPos);
                Debug.Log("Ve mau " + color + " tu " + lastPos + " " + currentPos);
                if(isFinished(lr,board))
                {
                    OnLineCompleted?.Invoke();
                    if(CheckWinCodition(board))
                    {
                        OnLevelCompleted?.Invoke();
                        gameController.SetState(GameController.GameState.LevelCompleted);
                    }
                }
                return true;
            }
            else
            {
                // TRƯỜNG HỢP VẼ MỚI
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, newPos);
                board.GetCellAtPosition(currentPos).SetCellColor(color);
                Debug.Log("Ve mau " + color + " tu " + lastPos + " " + currentPos);
                OnLineStep?.Invoke(lr.positionCount);
                if(isFinished(lr,board))
                {
                    OnLineCompleted?.Invoke();
                    if(CheckWinCodition(board))
                    {
                        OnLevelCompleted?.Invoke();
                        gameController.SetState(GameController.GameState.LevelCompleted);
                    }
                }
                return true;
            }
            
        }
        return false;

    }
    
    private bool isFinished(LineRenderer lr, BoardController board)
    {
        int px = Mathf.RoundToInt(lr.GetPosition(lr.positionCount - 1).x);
        int py = Mathf.RoundToInt(lr.GetPosition(lr.positionCount - 1).y);
        Vector2Int pos = new Vector2Int(px, py);
        if(lr.positionCount > 2 && board.GetCellAtPosition(pos).IsStartDot()) return true;
        return false;
    }

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
                board.GetCellAtPosition(currentPos).SetCellColor(color);
                return;
            }

        // QUAN TRỌNG: Trong Editor Mode, khi đặt bút xuống,
        // ta coi như ô đó là điểm bắt đầu luôn.
        // Gọi BoardController để đánh dấu ô này có màu (để hiển thị lên)
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, new Vector3(currentPos.x, currentPos.y, -5f));
        board.GetCellAtPosition(currentPos).SetCellColor(color);
    }

    private bool CheckWinCodition(BoardController board)
    {
        foreach ((Constants.COLOR color, GameObject lineObj) in lines)
        {
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            if(lr == null || lr.positionCount < 2) return false;
            int px = Mathf.RoundToInt(lr.GetPosition(lr.positionCount - 1).x);
            int py = Mathf.RoundToInt(lr.GetPosition(lr.positionCount - 1).y);
            Vector2Int pos = new Vector2Int(px, py);
            if(!board.GetCellAtPosition(pos).IsStartDot()) return false;
        }
        return true;
    }
}
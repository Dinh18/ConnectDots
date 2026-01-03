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

    public void DrawLine(Constants.COLOR color, Vector2Int point)
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
            lr.SetPosition(0, new Vector3(point.x, point.y, -5f));
            return;
        }

        if(lr.GetPosition(lr.positionCount - 1) == new Vector3(point.x, point.y, -5f))
        {
            return; // Nếu điểm hiện tại trùng với điểm cuối cùng của line thì không làm gì
        }

        Vector3 newPos = new Vector3(point.x, point.y, -5f);

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
            lr.positionCount = foundIndex + 1;
            
            // Cập nhật lại vị trí điểm cuối cho chính xác tuyệt đối (phòng trường hợp sai số nhỏ)
            lr.SetPosition(foundIndex, newPos);
        }
        if (foundIndex != -1)
        {
            // TRƯỜNG HỢP BACKTRACK (Đi lùi)
            // Cắt line về đúng vị trí tìm thấy.
            // Ví dụ: Line [0, 1, 2, 3], chạm vào 1 -> Giữ lại [0, 1] -> Count mới là 2 (index + 1)
            lr.positionCount = foundIndex + 1;
            // Cập nhật lại vị trí điểm cuối cho chính xác tuyệt đối (phòng trường hợp sai số nhỏ)
            lr.SetPosition(foundIndex, newPos);
        }
        else
        {
            // TRƯỜNG HỢP VẼ MỚI
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, newPos);
        }
    }

    // public void UpdateLine(Constants.COLOR color, Vector2Int point)
    // {
    //     if(lines.ContainsKey(color))
    //     {
    //         LineRenderer lr = lines[color].GetComponent<LineRenderer>();

    //         lr.positionCount++;
    //         lr.SetPosition(lr.positionCount - 1, new Vector3(point.x, point.y, -5f));
    //     }
    // }
}
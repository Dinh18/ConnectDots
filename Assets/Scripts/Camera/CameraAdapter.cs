using UnityEngine;

public class CameraAdapter : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [Header("Cài đặt lề")]
    [SerializeField] private float padding = 1f; // Khoảng hở viền cho đẹp (đừng để sát quá)
    [SerializeField] private float topPadding = 2f; // Chừa chỗ cho UI phía trên (Text Level, Button...)

    private void Awake()
    {
        if (mainCam == null) mainCam = GetComponent<Camera>();
    }

    public void AdjustCamera(int boardWidth, int boardHeight)
    {
        // 1. TÍNH VỊ TRÍ TÂM (Center)
        // Vì các ô bắt đầu từ (0,0) đến (width-1, height-1)
        float xCenter = (boardWidth - 1) / 2.0f;
        float yCenter = (boardHeight - 1) / 2.0f;

        // Dời camera ra giữa bảng (giữ nguyên Z = -10)
        transform.position = new Vector3(xCenter, yCenter, -10f);

        // 2. TÍNH ĐỘ ZOOM (Orthographic Size)
        
        // Tính size cần thiết cho chiều cao (Height)
        // (Size = một nửa chiều cao màn hình + lề trên dưới)
        float targetSizeY = (boardHeight / 2.0f) + topPadding;

        // Tính size cần thiết cho chiều rộng (Width)
        // Vì Camera tính theo chiều dọc, nên chiều ngang phải chia cho Aspect Ratio (Tỷ lệ màn hình)
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetSizeX = ((boardWidth / 2.0f) + padding) / screenRatio;

        // Chọn cái nào lớn hơn để đảm bảo bảng luôn nằm trọn trong màn hình
        // (Dù màn hình dọc hay ngang đều không bị mất góc)
        float finalSize = Mathf.Max(targetSizeY, targetSizeX);

        mainCam.orthographicSize = finalSize;
    }
}
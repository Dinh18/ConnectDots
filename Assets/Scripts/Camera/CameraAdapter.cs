using UnityEngine;

public class CameraAdapter : MonoBehaviour
{
    public float padding = 1f; // Khoảng hở ở viền cho thoáng

    public void AdjustCamera(int boardWidth, int boardHeight)
    {
        Camera cam = GetComponent<Camera>();

        // 1. Tính toán vị trí trung tâm để Camera nhìn vào
        // Công thức Offset giống hệt các bài trước
        float centerX = (boardWidth - 1) * 0.5f;
        float centerY = (boardHeight - 1) * 0.5f;

        // Di chuyển camera đến giữa bàn cờ
        transform.position = new Vector3(centerX, centerY, -10f);

        // 2. Tính toán độ Zoom (Orthographic Size)
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = (float)boardWidth / boardHeight;

        if (screenRatio >= targetRatio)
        {
            // Màn hình rộng hơn bàn cờ (hoặc bàn cờ cao) -> Tính theo Chiều Cao
            cam.orthographicSize = (boardHeight / 2f) + padding;
        }
        else
        {
            // Màn hình hẹp hơn bàn cờ (hoặc bàn cờ quá bè ngang) -> Tính theo Chiều Rộng
            // Phải chia cho screenRatio để quy đổi về chiều dọc
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = ((boardHeight / 2f) + padding) * differenceInSize;
        }
    }
}
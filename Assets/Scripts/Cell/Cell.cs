using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]private Constants.COLOR cellColor = Constants.COLOR.WHITE;
    private bool isStartDot = false;
    public bool IsStartDot() => isStartDot;
    public void SetStartDot(bool isStartDot) => this.isStartDot = isStartDot;
    public Constants.COLOR GetCellColor() => cellColor;
    public void SetCellColor(Constants.COLOR color) => cellColor = color;
}

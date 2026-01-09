using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILevel : MonoBehaviour, IMenu
{
    private UIController uIController;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button btnSetting;
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetUp(UIController uIController)
    {
        this.uIController = uIController;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    void Start()
    {
        btnSetting.onClick.AddListener(() => uIController.OnClickSetting());
    }

    void OnEnable()
    {
        text.SetText("Level {0}", uIController.SetLevelText());
        // 2. Reset trạng thái ban đầu:
        // Thu nhỏ về 0 và chỉnh Alpha về 0 (để chắc chắn nó ẩn)
        text.transform.localScale = Vector3.zero; 
        text.alpha = 0;

        // 3. Tạo Sequence (Chuỗi hành động)
        Sequence mySequence = DOTween.Sequence();

        // Bước A: Hiện dần lên (Fade In) trong 0.2s
        mySequence.Join(text.DOFade(1f, 0.2f));

        // Bước B: Phóng to ra với hiệu ứng "OutBack" (Nảy ra như lò xo)
        // 1f là kích thước gốc, 0.5f là thời gian
        mySequence.Join(text.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
    }

}

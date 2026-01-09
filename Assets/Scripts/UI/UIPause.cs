using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour, IMenu
{
    private UIController uIController;
    [SerializeField] Button btnMusic;
    [SerializeField] Button btnSound;
    [SerializeField] Button btnBackHome;
    [SerializeField] Button btnResume;
    [SerializeField] Button btnReplay;
    [SerializeField] Sprite iconMusicOn;
    [SerializeField] Sprite iconMusicOff;
    [SerializeField] private Sprite iconSoundOn;
    [SerializeField] private Sprite iconSoundOff;

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
        if(uIController.GetSoundController().IsPlayBackGround()) btnMusic.GetComponent<Image>().sprite = iconMusicOn;
        else btnMusic.GetComponent<Image>().sprite = iconMusicOff;
        if(uIController.GetSoundController().IsPlaySoundEffect()) btnSound.GetComponent<Image>().sprite = iconSoundOn;
        else btnSound.GetComponent<Image>().sprite = iconSoundOff;
    }

    void Start()
    {
        btnMusic.onClick.AddListener(() => uIController.OnClickMusic(btnMusic,iconMusicOn,iconMusicOff));
        btnSound.onClick.AddListener(() => uIController.OnClickSound(btnSound, iconSoundOn, iconSoundOff));
        btnBackHome.onClick.AddListener(() => uIController.OnClickMainMenu());
        btnResume.onClick.AddListener(() => uIController.OnClickResume());
        btnReplay.onClick.AddListener(() => uIController.OnClickRelay());
    }
}

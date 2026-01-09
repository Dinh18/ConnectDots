using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private SoundController soundController;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelCompletedMenu;
    [SerializeField] private GameObject uiLevel;
    [SerializeField] private GameObject uiSetting;
    [SerializeField] private GameObject uiPause;
    private UIMainMenu mainMenu;
    private UICompletedMenu completedMenu;
    private UILevel level;
    private UISetting setting;
    private UIPause pause;
    void Awake()
    {
        mainMenu = panelMainMenu.GetComponent<UIMainMenu>();
        completedMenu = panelCompletedMenu.GetComponent<UICompletedMenu>();
        level = uiLevel.GetComponent<UILevel>();
        setting = uiSetting.GetComponent<UISetting>();
        pause = uiPause.GetComponent<UIPause>();

        mainMenu.SetUp(this);
        completedMenu.SetUp(this);
        level.SetUp(this);
        setting.SetUp(this); 
        pause.SetUp(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SoundController GetSoundController() => soundController;

    public void UpdateUI(GameController.GameState state)
    {
        mainMenu.Hide();
        completedMenu.Hide();
        level.Hide();
        setting.Hide();
        pause.Hide();
        switch(state)
        {
            case GameController.GameState.MainMenu:
                mainMenu.Show();
                mainMenu.transform.localScale = Vector3.zero;
                mainMenu.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                break;
            case GameController.GameState.Playing:
                level.Show();
                break;
            case GameController.GameState.Setting:
                setting.Show();
                setting.transform.localScale = Vector3.zero;
                setting.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                break;
            case GameController.GameState.Pause:
                pause.Show();
                pause.transform.localScale = Vector3.zero;
                pause.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                break;
            case GameController.GameState.LevelCompleted:
                Camera.main.transform.DOShakePosition(0.5f, 0.3f, 10, 90);
                StartCoroutine(DelayDisplayCompleltedMenu(0.2f));
                break;
        }
    }

    private IEnumerator DelayDisplayCompleltedMenu(float time)
    {
        yield return new WaitForSeconds(time);
        completedMenu.Show();
        panelCompletedMenu.transform.localScale = Vector3.zero;
        panelCompletedMenu.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    public void Setup(GameController gameController, SoundController soundController)
    {
        this.gameController = gameController;
        this.soundController = soundController;
    }

    public void OnclikPlay()
    {
        gameController.LoadLevel();
    }

    public void OnClickNextLevel()
    {
        gameController.LoadNextLevel();
    }
    public void OnClickSetting()
    {
        gameController.SetState(GameController.GameState.Setting);
    }
    public void OnClickMainMenu()
    {
        gameController.SetState(GameController.GameState.MainMenu);
    }

    public void OnClickSound(Button btnSound, Sprite iconOn, Sprite iconOff)
    {
        soundController.ChangeEnableSoundEffectSource();
        if(soundController.IsPlaySoundEffect()) btnSound.GetComponent<Image>().sprite = iconOn;
        else btnSound.GetComponent<Image>().sprite = iconOff;
    }
    public void OnClickMusic(Button btnMusic, Sprite iconOn, Sprite iconOff)
    {
        if(soundController.IsPlayBackGround())
        {
            soundController.MuteBackGroundSound();
            btnMusic.GetComponent<Image>().sprite = iconOff;
        }
        else {
            soundController.UnMuteBackGroundSound();
            btnMusic.GetComponent<Image>().sprite = iconOn;
        }
    }
    public int SetLevelText()
    {
        return gameController.CurrentLevelIndex + 1;
    }
    public void OnClickPause()
    {
        gameController.SetState(GameController.GameState.Pause);
    }
    public void OnClickRelay()
    {
        gameController.LoadLevel();
    }
    public void OnClickResume()
    {
        gameController.SetState(GameController.GameState.Playing);
    }
}

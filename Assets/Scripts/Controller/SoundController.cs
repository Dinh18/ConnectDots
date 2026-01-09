using UnityEngine;

public class SoundController : MonoBehaviour
{
    private bool isPlaySoundEffect = true;
    private bool isPlayBackground = true;
    public AudioSource soundEffectSource;
    public AudioSource backgroundSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip moveClip;      // Tiếng "bloop" hoặc "tink" nhẹ
    [SerializeField] private AudioClip connectClip;   // Tiếng "Ding" khi nối xong
    [SerializeField] private AudioClip cutClip;
    [SerializeField] private AudioClip completedClip;
    [Header("Settings")]
    [SerializeField] private float basePitch = 1.0f;  // Cao độ gốc
    [SerializeField] private float pitchStep = 0.05f; // Mức tăng cao độ mỗi ô
    [SerializeField] private float maxPitch = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        soundEffectSource.playOnAwake = false;
        PlayBackGroundSound();
    }

    public void ChangeEnableSoundEffectSource()
    {
        isPlaySoundEffect = !isPlaySoundEffect;
    }

    void OnEnable()
    {
        LineController.OnLineStep += PlayStepSound;
        LineController.OnLineCompleted += PlayConnectSound;
        LineController.OnLineCut += PlayCutSound;
        LineController.OnLevelCompleted += PlayConpletedSound;
        
    }
    void OnDisable()
    {
        LineController.OnLineStep -= PlayStepSound;
        LineController.OnLineCompleted -= PlayConnectSound;
        LineController.OnLineCut -= PlayCutSound;
        LineController.OnLevelCompleted -= PlayConpletedSound;

    }
    private void PlayStepSound(int lineLength)
    {
        if (moveClip == null || !isPlaySoundEffect) return;

        // Tính toán Pitch dựa trên độ dài dây
        // Dây càng dài, tiếng càng thanh
        float targetPitch = basePitch + (lineLength * pitchStep);
        
        // Kẹp giá trị lại để không vượt quá maxPitch
        soundEffectSource.pitch = Mathf.Clamp(targetPitch, basePitch, maxPitch);
        
        // PlayOneShot cho phép các âm thanh chồng lên nhau (không bị ngắt quãng)
        soundEffectSource.PlayOneShot(moveClip);
    }
    private void PlayConnectSound()
    {
        if (connectClip == null || !isPlaySoundEffect) return;

        // Reset pitch về chuẩn để tiếng Ding nghe hay nhất
        soundEffectSource.pitch = 1.0f;
        soundEffectSource.PlayOneShot(connectClip);
    }
    private void PlayCutSound()
    {
        if (cutClip == null || !isPlaySoundEffect) return;
        // Tiếng cắt thường trầm hơn chút
        soundEffectSource.pitch = 0.8f; 
        soundEffectSource.PlayOneShot(cutClip);
    }
    private void PlayConpletedSound()
    {
        if(soundEffectSource == null || !isPlaySoundEffect) return;
        soundEffectSource.pitch = 1.0f;
        soundEffectSource.PlayOneShot(completedClip);
    }
    private void PlayBackGroundSound()
    {

        backgroundSource.loop = true;
        backgroundSource.clip = backgroundMusic;
        backgroundSource.Play();
        isPlayBackground = true;
        
    }

    public void UnMuteBackGroundSound()
    {
        backgroundSource.UnPause(); // Hát tiếp
        isPlayBackground = true;
    }



    public void MuteBackGroundSound()
    {
        backgroundSource.Pause(); // Dừng và nhớ vị trí
        isPlayBackground = false;
    }

    public bool IsPlayBackGround() => isPlayBackground;
    
}

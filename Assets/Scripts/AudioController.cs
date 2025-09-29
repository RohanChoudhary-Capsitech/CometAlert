using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] Sprite[] musicIcon;
    [SerializeField] Image musicImage;
    [SerializeField] Sprite[] sfxIcon;
    [SerializeField] Image sfxImage;


    private void Start()
    {
        musicImage.GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.MuteMusic(value: !AudioManager.Instance.musicIsInMute);
            musicImage.sprite = AudioManager.Instance.musicIsInMute ? musicIcon[1] : musicIcon[0];
        });
        sfxImage.GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.MuteSfx(value: !AudioManager.Instance.sfxIsInMute);
            sfxImage.sprite = AudioManager.Instance.sfxIsInMute ? sfxIcon[1] : sfxIcon[0];
        });
        UpdateMusicIcon();
        UpdateSfxIcon();
    }

    public void UpdateMusicIcon()
    {
        musicImage.sprite = AudioManager.Instance.musicIsInMute ? musicIcon[1] : musicIcon[0];
    }
    public void UpdateSfxIcon()
    {
        sfxImage.sprite = AudioManager.Instance.sfxIsInMute ? sfxIcon[1] : sfxIcon[0];
    }
    
}
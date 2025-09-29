using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuManager : MonoBehaviour
{
    public const string privacypolicy = "http://www.thegamewise.com/privacy-policy/";
    public Button[] Buttons;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _newBestText;
    [SerializeField] private TMP_Text _highScoreText;
    
    void Update()
    {
        // ButtonTap();
    }
    void Start()
    {
        _highScoreText.text = "Best: "+GameManager.Instance.HighScore.ToString();

        if(!GameManager.Instance.IsInitialized)
        {
            _scoreText.gameObject.SetActive(false);
            _newBestText.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowScore());
        }
        
        AudioManager.Instance.PlayMusic("bg");
    }

    [SerializeField] private float _animationTime;
    [SerializeField] private AnimationCurve _speedCurve;

    private IEnumerator ShowScore()
    {
        int tempScore = 0;
        _scoreText.text = tempScore.ToString();

        int currentScore = GameManager.Instance.CurrentScore;
        int highScore = GameManager.Instance.HighScore;

        if(highScore < currentScore)
        {
            _newBestText.gameObject.SetActive(true);
            GameManager.Instance.HighScore = currentScore;

        }
        else
        {
            _newBestText.gameObject.SetActive(false);
        }

        _highScoreText.text = "Best: "+GameManager.Instance.HighScore.ToString();

        float speed = 1 / _animationTime;
        float timeElapsed = 0f;
        while(timeElapsed < 1f)
        {
            timeElapsed += speed * Time.deltaTime;

            tempScore = (int)(_speedCurve.Evaluate(timeElapsed) * currentScore);
            _scoreText.text = tempScore.ToString();

            yield return null;
        }

        tempScore = currentScore;
        _scoreText.text = tempScore.ToString();

    }
    public void ClickedPlay()
    {
        AudioManager.Instance.PlaySfx("click");
        GameManager.Instance.GoToGameplay();
    }

    public void ButtonTap()
    {
        foreach (var button in  Buttons)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySfx("click");
            }); 
        }
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL(privacypolicy);
    }
    
}

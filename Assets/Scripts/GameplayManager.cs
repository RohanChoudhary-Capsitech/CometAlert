using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    #region Variables
    public List<Color> Colors;
    private bool hasGameFinished;
    [SerializeField] private ScoreEffect _scoreEffect;
    public GameObject Ball;
    public Transform Cannon;
    public GameObject CannonHead;
    private float score;
    [SerializeField] private TMP_Text _scoreText;
    public UnityAction GameEnd;
    [SerializeField] private float _spawnTime;
    [SerializeField] private Score _scorePrefab;
    private Score CurrentScore;
    public GameObject Gameover;
    #endregion
    
    #region START
    private void Awake()
    {
        Instance = this;

        hasGameFinished = false;
        GameManager.Instance.IsInitialized = true;

        score = 0;
        _scoreText.text = ((int)score).ToString();
        StartCoroutine(SpawnScore());
        Gameover.SetActive(false);
        AudioManager.Instance.StopMusic("bg");

    }

    #endregion

    #region GAME_LOGIC
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !hasGameFinished)
        {
            if(CurrentScore == null)
            {
                GameEnded();
                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            int currentScoreId = CurrentScore.ColorId;
            int clickedScoreId = hit.collider.gameObject.GetComponent<Player>().ColorId;


            if(currentScoreId != clickedScoreId)
            {
                GameEnded();
                return;
            }
            
            /***       Cannon rotation and Positioning     ***/
            
            //spawning the bullet
            var ball = Instantiate(Ball, Cannon.position, Cannon.rotation);
            Vector2 targetPos = CurrentScore.transform.position;
            Vector2 cannonPos = Cannon.transform.position;
            //angle calculation for the cannon to rotate
            Vector2 direction = targetPos - cannonPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            CannonHead.transform.rotation = Quaternion.Euler(0, 0, angle-90f); 
            ball.transform.rotation = Quaternion.Euler(0, 0, angle-90f);
            /***       Cannon rotation and Positioning     ***/
            
            ball.transform.DOMove(CurrentScore.transform.position, 0.2f);
            Destroy(ball, 0.2f);
            var t = Instantiate(_scoreEffect, CurrentScore.gameObject.transform.position, Quaternion.identity);
            t.Init(Colors[currentScoreId]);
                
            
            var tempScore = CurrentScore;
            if(CurrentScore.NextScore != null)
            {
                CurrentScore = CurrentScore.NextScore;
            }
            Destroy(tempScore.gameObject);
            UpdateScore();
        }
    }
    
    

    #endregion
    
    #region SCORE
    private void UpdateScore()
    {
        score++;
        AudioManager.Instance.PlaySfx("point");
        _scoreText.text = ((int)score).ToString();
    }

    
    private IEnumerator SpawnScore()
    {
        Score prevScore = null;

        while(!hasGameFinished)
        {
            var tempScore = Instantiate(_scorePrefab);

            if(prevScore == null)
            {
                prevScore = tempScore;
                CurrentScore = prevScore;
            }
            else
            {
                prevScore.NextScore = tempScore;
                prevScore = tempScore;
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    #endregion

    #region GAME_OVER
    public void GameEnded()
    {
        Gameover.SetActive(true);
        hasGameFinished = true;
        GameEnd?.Invoke();
        AudioManager.Instance.PlaySfx("lose");
        GameManager.Instance.CurrentScore = (int)score;
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }
    #endregion
}

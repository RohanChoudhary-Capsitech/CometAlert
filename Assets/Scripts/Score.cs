using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private float _moveSpeed=4f;
    [SerializeField] private List<Vector3> _spawnPos;

    [HideInInspector]
    public int ColorId;

    [HideInInspector]
    public Score NextScore;

    private void Awake()
    {
        _moveSpeed = 3.6f;
        hasGameFinished = false;
        transform.position = _spawnPos[Random.Range(0,_spawnPos.Count)];
        int colorCount = GameplayManager.Instance.Colors.Count;
        ColorId = Random.Range(0, colorCount);
        GetComponent<SpriteRenderer>().color = GameplayManager.Instance.Colors[ColorId];
    }

    private void FixedUpdate()
    {
        if (hasGameFinished) return;
        // if (GameManager.Instance.CurrentScore >= 30)
        // {
        //     _moveSpeed=_moveSpeed + ((_moveSpeed %30)/100);
        // }
        transform.Translate(_moveSpeed * Time.fixedDeltaTime * Vector3.down);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            GameplayManager.Instance.GameEnded();
        }
    }

    private void OnEnable()
    {
        GameplayManager.Instance.GameEnd += GameEnded;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.GameEnd -= GameEnded;
    }

    public void SpeedCounter()
    {
        if (GameManager.Instance.CurrentScore >= 30)
        {
            _moveSpeed=_moveSpeed+((_moveSpeed%4f)/100);
        }
    }

    private bool hasGameFinished;

    private void GameEnded()
    {
        hasGameFinished = true;
    }
}

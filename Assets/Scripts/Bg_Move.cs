using System;
using UnityEngine;

public class Bg_Move : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
[SerializeField] private float _speed;

private void Start()
{
    Debug.Log(""+ Screen.height + " "+ Screen.width);
    Vector2 startPos=transform.position;
}

void Update()
    {
        transform.position=new Vector2(transform.position.x,transform.position.y-_speed*Time.deltaTime);
        if (transform.localPosition.y <=-1920f)
        {
            transform.localPosition = new Vector2(transform.localPosition.x,1920f);
        }
    }
}

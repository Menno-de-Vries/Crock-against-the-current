using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Credits https://www.youtube.com/watch?v=auVq3TSz20o
public class WaterMovement : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeedX;
    [SerializeField]
    private float _scrollSpeedY;

    private void Update()
    {
        float xOffset = _scrollSpeedX * Time.time;
        float yOffset = _scrollSpeedY * Time.time;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
}

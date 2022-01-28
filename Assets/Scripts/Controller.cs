using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] private float timeScale = 0.9f;
    [SerializeField] private Text timeSinceCollision;
    public bool collision = false;
    private float x;
    private float d;

    private void Start()
    {
        Application.targetFrameRate = 300;
    }

    void FixedUpdate()
    {
        Time.timeScale = timeScale;

        if (collision)
        {
            collision = false;
            x = Time.realtimeSinceStartup;
        }

        d = Mathf.Round((Time.realtimeSinceStartup - x) * 100f) / 100f;
        timeSinceCollision.text = d.ToString();
    }
}

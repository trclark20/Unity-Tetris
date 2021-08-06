using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    public float x;
    [SerializeField]
    public float y;
    public bool movX;
    public bool movY;

    // Start is called before the first frame update
    void Start()
    {
        x = 5f;
        y = 1f;
        movX = false;
        movY = false;
    }

    // Update is called once per frame
    void Update()
    {
        x -= Time.deltaTime * 600f;
        y -= Time.deltaTime * 1f;

        if (x <= 0f)
        {
            movX = true;
            x = 20f;
        }

        if (y <= 0f)
        {
            movY = true;
            y = 1f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public BlockSpawnScript.Block player;
    public GameTimer timer;

    GameObject grid;

    GridCollision gridCollisionScript;

    Image left_arrow;
    Image down_arrow;
    Image right_arrow;
    Text uiText;

    Sprite[] sprites;

    float timeToIncrease;

    float startTime, upTime, pressTime = 0;
    float countDown = 0.5f;
    float yCollisionGracePeriod = 0f;
    bool readyX, readyY = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid");
        gridCollisionScript = grid.GetComponent<GridCollision>();

        left_arrow = GameObject.Find("Left_Arrow").GetComponent<Image>();
        down_arrow = GameObject.Find("Down_Arrow").GetComponent<Image>();
        right_arrow = GameObject.Find("Right_Arrow").GetComponent<Image>();
        uiText = GameObject.Find("Text").GetComponent<Text>();

        timer = GameObject.Find("GameTimer").GetComponent<GameTimer>();

        player = new BlockSpawnScript.Block("random", true);
    }

    // Update is called once per frame
    void Update()
    {
        bool xNeg = Input.GetButtonDown("HorizontalNeg");
        bool xPos = Input.GetButtonDown("HorizontalPos");
        bool xPosHeld = Input.GetButton("HorizontalPos");
        bool xNegHeld = Input.GetButton("HorizontalNeg");
        bool xDown = Input.GetButton("Vertical");

        uiText.text = "";
        for (int i = 0; i < 4; i++)
        {
            uiText.text += player.coords[i].x + ", " + player.coords[i].y + "\n";
        }

        if (Input.GetAxisRaw("Vertical") == -1f)
        {
            down_arrow.color = new Color32(247, 255, 99, 100);
        }
        else
        {
            down_arrow.color = new Color32(33, 33, 33, 255);
        }

        if (xPosHeld)
        {
            right_arrow.color = new Color32(247, 255, 99, 100);
        }
        else
        {
            right_arrow.color = new Color32(33, 33, 33, 255);
        }

        if (xNegHeld)
        {
            left_arrow.color = new Color32(247, 255, 99, 100);
        }
        else
        {
            left_arrow.color = new Color32(33, 33, 33, 255);
        }

        #region Vertical Movement Down

        if (xDown)
        {
            VertMoveDown(true);
        }

        if (timer.movY && !CheckCollision(player.coords, true, 1).Contains(1))
        {
            VertMoveDown();
        }
        //if Y collision occurs, block will stop
        else if (timer.movY && CheckCollision(player.coords, true, 1).Contains(1))
        {
            
            //release the child 
            for (int i = 0; i < 4; i++)
            {
                gridCollisionScript.collisionMatrix[player.coords[i].x, player.coords[i].y] = 1;
            }

            //Check for row clear

            for (int i = 0; i < 4; i++)
            {
                if (!CheckLineClear(player.coords[i]).Contains(0))
                {
                    Debug.Log("Good!!");
                }
            }

            player = new BlockSpawnScript.Block();
        }

        #endregion

        #region Horizontal Movement Right
        if (xPos)
        {
            //Move right, ignoring timer
            HorizMoveRight(true);
        }
        if (xPos && !readyX)
        {
            startTime = Time.time;
            pressTime = startTime + countDown;
            readyX = true;
            //player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);

            //timer.movX = false;
        }
        if (Input.GetButtonUp("HorizontalPos"))
        {
            readyX = false;
        }
        if (xPosHeld && Time.time >= pressTime && readyX == true)
        {
            HorizMoveRight();
        }
        #endregion

        #region Horizontal Movement Left
        if (xNeg)
        {
            //Move Left, ignoring timer
            HorizMoveLeft(true);
        }
        if (xNeg && !readyY) 
        {
            startTime = Time.time;
            pressTime = startTime + countDown;
            readyY = true;
            //player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);

            //timer.movX = false;
        }
        if (Input.GetButtonUp("HorizontalNeg"))
        {
            readyY = false;
        }
        if (xNegHeld && Time.time >= pressTime && readyY == true)
        {
            HorizMoveLeft();
        }
        #endregion
    }

    /// <summary>
    /// Moves object 1 block right, horizontally
    /// </summary>
    /// <param name="bypassTimer">Whether to ignore the movement timer or not</param>
    void HorizMoveRight(bool bypassTimer = false)
    {
        //optionally bypass movement timer
        if ((timer.movX || bypassTimer) && !CheckCollision(player.coords, false, 1).Contains(1))
        {
            for (int i = 0; i < 4; i++)
            {
                player.coords[i].x += 1;
            }

            player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);

            timer.movX = false;
        }
    }

    /// <summary>
    /// Moves object 1 block left, horizontally
    /// </summary>
    /// <param name="bypassTimer">Whether to ignore the movement timer or not</param>
    void HorizMoveLeft(bool bypassTimer = false)
    {
        //optionally bypass movement timer
        if ((timer.movX || bypassTimer) && !CheckCollision(player.coords, false, -1).Contains(1))
        {
            for (int i = 0; i < 4; i++)
            {
                player.coords[i].x -= 1;
            }

            player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x - 1f, player.gameBlock.transform.position.y, 0f);

            timer.movX = false;
        }
    }

    /// <summary>
    /// Moves object 1 block down, vertically
    /// </summary>
    /// <param name="bypassTimer">Whether to ignore the movement timer or not</param>
    void VertMoveDown(bool bypassTimer = false)
    {
        //optionally bypass movement timer
        if ((timer.movY || bypassTimer) && !CheckCollision(player.coords, true, 1).Contains(1))
        {
            for (int i = 0; i < 4; i++)
            {
                player.coords[i].y += 1;
            }

            player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x, player.gameBlock.transform.position.y - 1, 0f);

            timer.movY = false;
        }
    }

    /// <summary>
    /// Checks if the player will collide on moving
    /// </summary>
    /// <param name="coords">Vector2 of X and Y coordinates</param>
    /// <param name="vertical">if the movement is veritcal or not</param>
    /// <param name="direction">direction of movement, -1 is up/left and 1 is down/right</param>
    int[] CheckCollision(Vector2Int[] coords, bool vertical, int direction)
    {
        int[] results = { 0, 0, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            if (vertical ? gridCollisionScript.collisionMatrix[coords[i].x, coords[i].y + direction] == 1 :
                gridCollisionScript.collisionMatrix[coords[i].x + direction, coords[i].y] == 1)
            {
                results[i] = 1;
            }
            else
            {
                results[i] = 0;
            }
        }
        return results;
    }

    int[] CheckLineClear(Vector2Int coords)
    {
        int[] results = new int[GridCollision.BOARD_SIZE_X];

        //Loop through game board, check if row is all 1's
            for (int j = 0; j < GridCollision.BOARD_SIZE_X; j++)
            {
                if (gridCollisionScript.collisionMatrix[j, coords.y] == 1)
                {
                    results[j] = 1;
                }
                else
                {
                    results[j] = 0;
                }
            }
        return results;
    }
}

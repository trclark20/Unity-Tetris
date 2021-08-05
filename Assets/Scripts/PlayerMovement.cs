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
    float startTime;

    bool movementCooldown;

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

        if (xPos)
        {
            right_arrow.color = new Color32(247, 255, 99, 100);
        }
        else
        {
            right_arrow.color = new Color32(33, 33, 33, 255);
        }

        if (xNeg)
        {
            left_arrow.color = new Color32(247, 255, 99, 100);
        }
        else
        {
            left_arrow.color = new Color32(33, 33, 33, 255);
        }

        if (timer.movY && !CheckCollision(player.coords, true, 1).Contains(1))
        {
            //Movement: x: 1 y: 1
            player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x, player.gameBlock.transform.position.y - 1, 0f);

            for (int i = 0; i < 4; i++)
            {
                player.coords[i].y += 1;
            }

            timer.movY = false;
        }
        //if Y collision occurs, block will stop
        else if (CheckCollision(player.coords, true, 1).Contains(1))
        {
            //release the child 
            for (int i = 0; i < 4; i++)
            {
                gridCollisionScript.collisionMatrix[player.coords[i].x, player.coords[i].y] = 1;
            }

            player = new BlockSpawnScript.Block();
        }
        
            //Horizontal movement right held
            if (xPosHeld && !xPos)
            {
                if (Time.time - startTime > timeToIncrease)
                {
                    timer.x -= timeToIncrease;
                    startTime = Time.time;
                }

                HorizMoveRight(true);
                timer.movX = false;
            }

            //Horizontal movement right
            if (xPos)
            {
                //player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);
                timeToIncrease = 20000000f;
                startTime = Time.time;

                HorizMoveRight();
                timer.movX = false;
                movementCooldown = true;
            }

            //Horizontal movement left
            else if (xNeg && !CheckCollision(player.coords, false, -1).Contains(1))
            {
                player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x - 1f, player.gameBlock.transform.position.y, 0f);

                for (int i = 0; i < 4; i++)
                {
                    player.coords[i].x -= 1;
                }

                timer.movX = false;
            }
    }

    void HorizMoveRight(bool bypassTimer = false)
    {
        if (bypassTimer)
        {
            if (!CheckCollision(player.coords, false, 1).Contains(1))
            {
                for (int i = 0; i < 4; i++)
                {
                    player.coords[i].x += 1;
                }

                player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);

                timer.movX = false;
            }
        }
        else
        {
            if (timer.movX && !CheckCollision(player.coords, false, 1).Contains(1))
            {
                for (int i = 0; i < 4; i++)
                {
                    player.coords[i].x += 1;
                }

                player.gameBlock.transform.position = new Vector3(player.gameBlock.transform.position.x + 1f, player.gameBlock.transform.position.y, 0f);

                timer.movX = false;
            }
        }
    }

    /// <summary>
    /// Checks if the player will collide on moving
    /// </summary>
    /// <param name="xCoord">X movement</param>
    /// <param name="yCoord">Y movement</param>
    /// <param name="direction">direction of movement, -1 is up/left and 1 is down/right</param>
    /// <param name="vertical">if the movement is veritcal or not</param>
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
}

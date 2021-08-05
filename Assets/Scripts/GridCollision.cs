using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollision : MonoBehaviour
{
    public int[,] collisionMatrix
    {
        get
        {
            return _collisionMatrix;
        }

    }
    private static int[,] _collisionMatrix = new int[BOARD_SIZE_X + 2, BOARD_SIZE_Y + 2];

    const int BOARD_SIZE_X = 10;
    const int BOARD_SIZE_Y = 20;

    // Start is called before the first frame update
    void Start()
    {
        GridCollisionSetup(collisionMatrix);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates 1 block collision border around play area
    /// </summary>
    void GridCollisionSetup(int[,] collisionMatrix)
    {
        for (int i = 0; i <= BOARD_SIZE_X + 1; i++)
        {
            for (int j = 0; j <= BOARD_SIZE_Y + 1; j++)
            {
                if (i == 0 || j == 0 || i == BOARD_SIZE_X + 1 || j == BOARD_SIZE_Y + 1)
                {
                    collisionMatrix[i, j] = 1;
                }
                else
                {
                    collisionMatrix[i, j] = 0;
                }
            }
        }
    }
}

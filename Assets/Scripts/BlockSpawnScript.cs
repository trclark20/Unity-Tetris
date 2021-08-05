using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnScript : MonoBehaviour
{
    static Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();
    static Dictionary<string, int[,]> blockCoordsDict = new Dictionary<string, int[,]>();
    static Dictionary<string, Vector3> blockPositionDict = new Dictionary<string, Vector3>();
    static string[] blockTypes = new string[7];
    static List<string> usedTypes = new List<string>();

    PlayerMovement playerMovementScript;

    SpriteRenderer spriteRenderer;

    public class Block
    {
        [SerializeField]        
        public Vector2Int[] coords;
        public GameObject gameBlock;
        public SpriteRenderer spriteRenderer;


        public Block(string blockType = "random", bool inital = false)
        {
            if (blockType == "random")
            {
                blockType = blockTypes[Random.Range(0, 7)];

                while (usedTypes.Contains(blockType))
                {
                    blockType = blockTypes[Random.Range(0, 7)];

                    if (usedTypes.Count == 7)
                    {
                        usedTypes.Clear();
                    }
                }
            }

            if (!inital)
            {
                gameBlock = new GameObject("Block");
            }
            else
            {
                gameBlock = GameObject.Find("Initial_Block");
            }

            //tracking what types are used so we can ensure a fair spread
            usedTypes.Add(blockType);
            coords = convert2DArrayToVec2Int(blockCoordsDict[blockType]);
            gameBlock.transform.position = blockPositionDict[blockType];

            spriteRenderer = gameBlock.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 2;
            spriteRenderer.sprite = spritesDict[blockType];
        }

        public Vector2Int[] convert2DArrayToVec2Int(int[,] array)
        {
            Vector2Int[] res = new Vector2Int[4];

            for (int i = 0; i < 4; i++)
            {
                res[i].x = array[i, 0];
                res[i].y = array[i, 1];
            }

            return res;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        var sprites = Resources.LoadAll<Sprite>("");
        foreach (var s in sprites)
        {
            spritesDict.Add(s.name, s);
        }

        blockCoordsDict.Add("T_Block", new int[,] { { 1, 1 }, { 2, 1 }, { 2, 0 }, { 3, 1 } });
        blockCoordsDict.Add("Square_Block", new int[,] { { 1, 0 }, { 2, 0 }, { 1, 1 }, { 2, 1 } });
        blockCoordsDict.Add("S_Block", new int[,] { { 1, 1 }, { 2, 1 }, { 2, 0 }, { 3, 0 } });
        blockCoordsDict.Add("R_Block", new int[,] { { 1, 0 }, { 1, 1 }, { 2, 1 }, { 3, 1 } });
        blockCoordsDict.Add("L_Block", new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 3, 0 } });
        blockCoordsDict.Add("I_Block", new int[,] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 } });
        blockCoordsDict.Add("Backwards_S_Block", new int[,] { { 1, 0 }, { 2, 0 }, { 2, 1 }, { 3, 1 } });

        blockPositionDict.Add("T_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("Square_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("S_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("R_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("L_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("I_Block", new Vector3(-5f, 9f, 0f));
        blockPositionDict.Add("Backwards_S_Block", new Vector3(-5f, 9f, 0f));

        blockTypes[0] = "T_Block";
        blockTypes[1] = "Square_Block";
        blockTypes[2] = "S_Block";
        blockTypes[3] = "R_Block";
        blockTypes[4] = "L_Block";
        blockTypes[5] = "I_Block";
        blockTypes[6] = "Backwards_S_Block";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

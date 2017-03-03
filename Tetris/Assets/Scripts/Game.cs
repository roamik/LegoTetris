using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public int scoreOneLine = 35;
    public int scoreTwoLine = 75;
    public int scoreThreeLine = 125;
    public int scoreFourLine = 255;

    public AudioClip clearedLineSound;

    public Text hud_score;

    private int numberOfRowsThisTurn = 0;

    private AudioSource audioSource;

    public static int currentScore = 0;

    private GameObject previewBlock;
    private GameObject nextBlock;

    private bool gameStarted = false;

    private Vector2 previewBlockPosition = new Vector2(14f, 12);


	// Use this for initialization
	void Start () {
        SpawnNextBlock();

        audioSource = GetComponent<AudioSource>();
	}

    void Update()
    {
        UpdateScore();
        UpdateUI();
    }

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
    }

    public void UpdateScore()
    {
        if (numberOfRowsThisTurn > 0)
        {
            if (numberOfRowsThisTurn == 1)
            {
                ClearedOneLine();
            }
            else if (numberOfRowsThisTurn == 2)
            {
                ClearedTwoLines();
            }
            else if (numberOfRowsThisTurn == 3)
            {
                ClearedThreeLines();
            }
            else if (numberOfRowsThisTurn == 4)
            {
                ClearedFourLines();
            }

            numberOfRowsThisTurn = 0;

            PlayLineClearedSound();
        }
    }

    public void ClearedOneLine()
    {
        currentScore += scoreOneLine;
    }

    public void ClearedTwoLines()
    {
        currentScore += scoreTwoLine;
    }

    public void ClearedThreeLines()
    {
        currentScore += scoreThreeLine;
    }

    public void ClearedFourLines()
    {
        currentScore += scoreFourLine;
    }

    public void PlayLineClearedSound()
    {
        audioSource.PlayOneShot(clearedLineSound);
    }

    public bool CheckIsAboveGrid(Block block)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            foreach (Transform mino in block.transform)
            {
                Vector2 pos = Round(mino.position);

                if(pos.y > gridHeight -1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFullRow (int y)
    {
        // the param. y, is the row we will iterate over in the grid array to check each x pos for a transform
        for (int x = 0; x < gridWidth; x++)
        {
            // -if we find a position that returns NULL instead of a transform, then we know that the row is not full
            if (grid [x,y] == null)
            {
                //So we return false
                return false;
            }
        }
        // since we found a full row, we increment the full row variable
        numberOfRowsThisTurn++;

        // -if we iterated over the entire loop and didn't encounter any NULL positions, then we return true
        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;
        } 
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];

                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; i++)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow ()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (IsFullRow(y))
            {
                DeleteMinoAt(y);

                MoveAllRowsDown(y + 1);

                --y;
            }
        }
    }

    public void UpdateGrid(Block block)
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x =0; x < gridWidth; ++x)
            {
                if (grid[x,y] != null)
                {
                    if(grid[x,y].parent == block.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in block.transform)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformGridPosition (Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public void SpawnNextBlock()
    {
        if(!gameStarted)
        {
            gameStarted = true;
            nextBlock = (GameObject)Instantiate(Resources.Load(GetRandomBlock(), typeof(GameObject)), new Vector2(5.0f, 19.0f), Quaternion.identity);
            previewBlock = (GameObject)Instantiate(Resources.Load(GetRandomBlock(), typeof(GameObject)), previewBlockPosition, Quaternion.identity);
            previewBlock.GetComponent<Block>().enabled = false;
        }
        else
        {
            previewBlock.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextBlock = previewBlock;
            nextBlock.GetComponent<Block>().enabled = true;

            previewBlock = (GameObject)Instantiate(Resources.Load(GetRandomBlock(), typeof(GameObject)), previewBlockPosition, Quaternion.identity);
            previewBlock.GetComponent<Block>().enabled = false;
        } 
    }

    public bool CheckIsInsideGrid (Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round (Vector2 pos) // round the coordinates
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    string GetRandomBlock ()
    {
        int randomBlock = Random.Range(1, 8);

        string randomBlockName = "Prefabs/Block_j";

        switch (randomBlock)
        {
            case 1:
                randomBlockName = "Prefabs/Block_j";
                break;
            case 2:
                randomBlockName = "Prefabs/Block_l";
                break;
            case 3:
                randomBlockName = "Prefabs/Block_long";
                break;
            case 4:
                randomBlockName = "Prefabs/Block_s";
                break;
            case 5:
                randomBlockName = "Prefabs/Block_square";
                break;
            case 6:
                randomBlockName = "Prefabs/Block_t";
                break;
            case 7:
                randomBlockName = "Prefabs/Block_z";
                break;
        }
        return randomBlockName;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}

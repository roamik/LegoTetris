﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    float fallTimer = 0; //countdown timer for fall speed
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;

    private float continuousVerticalSpeed = 0.05f; // the speed at wich the block will move when the down button is held down
    private float continuousHorizontalSpeed = 0.1f; // the speed at wich the block will move when the left or right buttons are held down
    private float buttonDownWaitMax = 0.2f;         // how long to wait before the block recognizes that a button is being held down

    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimer = 0;

    private bool movedImmediateHorizontal = false;
    private bool movedImmediateVertical = false;

    public int individualPoints = 50;

    private AudioSource audioSource;
    private float individualScoreTime;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckUserInput();
        UpdateIndividualScore();
	}

    void UpdateIndividualScore()
    {
        if (individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;
        }
        else
        {
            individualScoreTime = 0;

            individualPoints = Mathf.Max(individualPoints - 10, 0); //individual points if they are below 0 (example -10) will return 0 , or the largest value 
        }
    }

    void CheckUserInput ()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            movedImmediateHorizontal = false;
            movedImmediateVertical = false;

            horizontalTimer = 0;
            verticalTimer = 0;
            buttonDownWaitTimer = 0;
        }

        #region RightArrow movement
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(movedImmediateHorizontal)
            {
                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (horizontalTimer < continuousHorizontalSpeed)
                {
                    horizontalTimer += Time.deltaTime;
                    return; //return out of the method
                }
            }

            if (!movedImmediateHorizontal)
            {
                movedImmediateHorizontal = true;
            }

            horizontalTimer = 0;

            transform.position += new Vector3(1,0,0);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this); // if it is then we call the UpdateGrid method which records this block new position
                PlayMoveAudio();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        #endregion

        #region LeftArrow movement
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (movedImmediateHorizontal)
            {
                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (horizontalTimer < continuousHorizontalSpeed)
                {
                    horizontalTimer += Time.deltaTime;
                    return; //return out of the method
                }
            }

            if(!movedImmediateHorizontal)
            {
                movedImmediateHorizontal = true;
            }

            horizontalTimer = 0;

            transform.position += new Vector3(-1, 0, 0);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                PlayMoveAudio();
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        #endregion

        #region UpArrow movement // The up movement of the block
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                    if (CheckIsValidPosition())
                    {
                        FindObjectOfType<Game>().UpdateGrid(this);
                    PlayRotateAudio();
                    }
                    else
                    {
                        if (limitRotation)
                        {
                            if (transform.rotation.eulerAngles.z >= 90)
                            {
                                transform.Rotate(0, 0, -90);
                            }
                            else
                            {
                                transform.Rotate(0, 0, 90);
                            }
                        }
                        else
                        {
                            transform.Rotate(0, 0, -90);
                        }
                    }
                }
        }

        #endregion

        #region DownArrow movement // the down movement of the block
        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fallTimer >= fallSpeed) // moving the block rather by user or by timer
        {
            if (movedImmediateVertical)
            {

                if (buttonDownWaitTimer < buttonDownWaitMax)
                {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }

                if (verticalTimer < continuousVerticalSpeed)
                {
                    verticalTimer += Time.deltaTime;
                    return;
                }
            }

            if(!movedImmediateVertical)
            {
                movedImmediateVertical = true;
            }

            verticalTimer = 0;

            transform.position += new Vector3(0, -1, 0);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    PlayMoveAudio();
                }
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);

                FindObjectOfType<Game>().DeleteRow();

                if(FindObjectOfType<Game>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }

                PlayLandAudio(); // plays land sound when the block landed
                FindObjectOfType<Game>().SpawnNextBlock();

                Game.currentScore += individualPoints;

                enabled = false;

            }

            fallTimer = Time.time;
        }
        #endregion
    }

    void PlayMoveAudio()
    {
        audioSource.PlayOneShot(moveSound);
    }

    void PlayRotateAudio()
    {
        audioSource.PlayOneShot(rotateSound);
    }

    void PlayLandAudio()
    {
        audioSource.PlayOneShot(landSound);
    }

    bool CheckIsValidPosition ()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position); // the pos variable will contain a rounded value of the 'mino' current position for 1 iteration

            if (FindObjectOfType<Game>().CheckIsInsideGrid (pos) == false) //check on the current iteration if the position of the mino is inside the grid
            {
                return false;
            }

            if (FindObjectOfType<Game>().GetTransformGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}

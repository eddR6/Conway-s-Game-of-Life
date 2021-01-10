using System.Collections;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private int matrixSize;
    private int[,] currentState;
    private int[,] nextState;
    private SpriteRenderer[,] grid;
    [SerializeField]
    private Sprite activeBlock;  //iron man image as active block
    [SerializeField]
    private Sprite inactiveBlock; //grass image as inactive block
    [SerializeField]
    private BoardSetup boardSetup;
    [SerializeField]
    private float waitTime=1f;  //delay between 2 consecutive steps of the game
    private bool goToNextState;

    private void Start()
    {
        grid = boardSetup.SetBoard(); //setting base board
        goToNextState = true;
        currentState = new int[matrixSize, matrixSize];
        nextState = new int[matrixSize, matrixSize];
        SetRandomBoard();
        
    }

    public void StartGame() //linked to Start button
    {
        StartCoroutine(NextState());
    }

    IEnumerator NextState()    //nested coroutine to apply delay between 2 consecutive steps of the game
    {
        UpdateNextState();
        goToNextState = CheckAndShiftState();
        if (goToNextState)
        {
            yield return new WaitForSeconds(waitTime);
            UpdateBoard();
            StartCoroutine(NextState());
        }
        
    }

    private void UpdateNextState() //getting nextStete based on the rules provided
    {
        int count;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                count = CountNearbyLiveCells(i, j);
                if (currentState[i, j] == 1)  //Active block rules
                {
                    if (count < 2)  //Underpopulated
                    {
                        nextState[i, j] = 0;
                    }
                    else if (count == 2 || count == 3) //Lives
                    {
                        nextState[i, j] = 1;
                    }
                    else if (count > 3)   //Overpopulated
                    {
                        nextState[i, j] = 0;
                    }
                }
                else if (currentState[i, j] == 0) //Inactive block rules
                {
                    if (count == 3) //Reproduction
                    {
                        nextState[i, j] = 1;
                    }
                    else
                    {
                        nextState[i, j] = 0;
                    }
                }

            }
        }
    }

    private void UpdateBoard() //Update current board according to the values in currentState
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if (currentState[i, j] == 1)
                {
                    grid[i, j].sprite = activeBlock;
                }
                else if (currentState[i, j] == 0)
                {
                    grid[i, j].sprite = inactiveBlock;
                }
            }
        }
    }

    private bool CheckAndShiftState() //Check if next state generated is equal to current state. Shifting nextState values to currentState
    {
        int count = 0;
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                if(currentState[i, j] == nextState[i, j])
                {
                    count++;
                }
                currentState[i, j] = nextState[i, j];
            }
        }
        return count == matrixSize * matrixSize ? false : true;
    }

    public void SetRandomBoard() //linked to Random Board button
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                currentState[i, j] = UnityEngine.Random.Range(0, 2);
                if (currentState[i, j] == 1)
                {
                    grid[i, j].sprite = activeBlock;
                }
                else
                {
                    grid[i, j].sprite = inactiveBlock;
                }
            }
        }
    }

    private int CountNearbyLiveCells(int i, int j)
    {
        int count = 0;
        //i-1,j
        if (i - 1 >= 0 && currentState[i - 1, j] == 1)
        {
            count++;
        }
        //i-1,j+1
        if (i - 1 >= 0 && j + 1 < matrixSize && currentState[i - 1, j + 1] == 1)
        {
            count++;
        }
        //i,j+1
        if (j + 1 < matrixSize && currentState[i, j + 1] == 1)
        {
            count++;
        }
        //i+1,j+1
        if (i + 1 < matrixSize && j + 1 < matrixSize && currentState[i + 1, j + 1] == 1)
        {
            count++;
        }
        //i+1,j
        if (i + 1 < matrixSize && currentState[i + 1, j] == 1)
        {
            count++;
        }
        //i+1,j-1
        if (i + 1 < matrixSize && j - 1 >= 0 && currentState[i + 1, j - 1] == 1)
        {
            count++;
        }
        //i,j-1
        if (j - 1 >= 0 && currentState[i, j - 1] == 1)
        {
            count++;
        }
        //i-1,j-1
        if (i - 1 >= 0 && j - 1 >= 0 && currentState[i - 1, j - 1] == 1)
        {
            count++;
        }
        return count;
    }
}

using UnityEngine;

public class BoardSetup: MonoBehaviour
{
    [SerializeField]
    private int matrixSize;
    [SerializeField]
    private SpriteRenderer block; //tile
   
    public SpriteRenderer[,] SetBoard()
    {
        SpriteRenderer[,] grid;  //To store matrix of tiles
        float startX = -(matrixSize / 2 - 0.5f); //getting initial x pos. 
        float startY = matrixSize / 2 - 0.5f;    //getting initial y pos, top left
        grid = new SpriteRenderer[matrixSize, matrixSize];
        for (int i = 0; i < matrixSize; i++)
        {
            for(int j = 0; j < matrixSize; j++)
            {
                grid[i, j] = Instantiate(block);
                grid[i, j].transform.position = new Vector2(startX + j, startY - i); //placing empty (no sprite for now) SpriteRenderers (tiles)
            }                                                                         // gameobject at correct position
        }
        return grid; 
        
    }
 
}

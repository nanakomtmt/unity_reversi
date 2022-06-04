using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private float SQUARE_SIZE = 50f;
    private List<Square> _squares = new List<Square>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(Transform parentTransform)
    {
        float xPosition = parentTransform.localPosition.x - 200;
        float yPosition = parentTransform.localPosition.y + 200;


        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                float xPos = xPosition + SQUARE_SIZE * j;
                float yPos = yPosition - SQUARE_SIZE * i;
                GameObject go = Utils.InstantiatePrefab("Prefabs/Square", parentTransform);
                Square square = go.GetComponent<Square>();
                square.Setup(parentTransform, xPos, yPos, i, j);
                _squares.Add(square);
            }
        }
    }

    public void ResetBoard()
    {
        
    }

    public Square GetSquare(Vector3 pos)
    {
        foreach (var square in _squares)
        {
            if (square.HitTest(pos))
            {
                return square;
            }
        }

        return null;
    }
}
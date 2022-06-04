using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private float SQUARE_SIZE=50f;
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
        // int screenWidth = Screen.width;
        // var squarePrefab = (GameObject)Resources.Load("Assets/Prefabs/Cube");
        float xPosition = 0;
        float yPosition = 0;
        xPosition = parentTransform.localPosition.x-200;
        yPosition = parentTransform.localPosition.y+200;
        
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                xPosition = parentTransform.localPosition.x-200+SQUARE_SIZE * j;
                yPosition = parentTransform.localPosition.y+200-SQUARE_SIZE * i;
                GameObject go = Utils.InstantiatePrefab("Prefabs/Square", parentTransform);
                Square square = go.GetComponent<Square>();
                square.Setup(parentTransform,xPosition,yPosition,i,j);
                
                
            }
           

        }
    }
}
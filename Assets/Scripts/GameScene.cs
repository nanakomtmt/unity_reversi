using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    SquareManager _squareManager;
    [SerializeField] GameObject board=null;
    // Start is called before the first frame update
    void Start()
    {
        _squareManager=board.GetComponent<SquareManager>();
        _squareManager.Setup(board.transform);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

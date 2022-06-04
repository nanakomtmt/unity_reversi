using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private int _x;

    private int _y;

    [SerializeField] private GameObject _black, _white;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Transform parent,float xPosition,float yPosition,int x,int y)
    {
        _x = x;
        _y = y;
        Debug.Log((xPosition,yPosition));
        this.transform.localPosition = new Vector3(xPosition, yPosition, 0);
        this.transform.SetParent(parent, true);
        
        _black.SetActive(x == 3&&y==3 ||x==4&&y==4);
        _white.SetActive(x == 3&&y==4 ||x==4&&y==3);
    }

    
}

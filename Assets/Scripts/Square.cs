using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Square : MonoBehaviour
{
    private int _x;

    public int x
    {
        get { return _x; }
    }

    public int y
    {
        get { return _y; }
    }


    public Const.COLOR Color
    {
        get
        {
            if (_white.activeSelf)
            {
                return Const.COLOR.WHITE;
            }
            else
            {
                return Const.COLOR.BLACK;
            }
        }
    }


    public bool isWhite
    {
        get { return _white.activeSelf; }
    }

    public bool isBlack
    {
        get { return _black.activeSelf; }
    }


    private int _y;
    private float SQUARE_SIZE = 70f;

    [SerializeField] private GameObject _black, _white;

    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(Transform parent, float xPosition, float yPosition, int x, int y)
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _x = x;
        _y = y;

        this.transform.localPosition = new Vector3(xPosition, yPosition, 0);
        this.transform.SetParent(parent, true);
        ResetBoard();
    }

    public void ResetBoard()
    {
        _black.SetActive(_x == 3 && _y == 3 || _x == 4 && _y == 4);
        _white.SetActive(_x == 3 && _y == 4 || _x == 4 && _y == 3);
    }

    public bool HitTest(Vector3 pos)
    {
        if (Mathf.Abs(pos.x - this.transform.position.x) <
            SQUARE_SIZE / 2 * _rectTransform.lossyScale.x)
        {
            if (Mathf.Abs(pos.y - this.transform.position.y) <
                SQUARE_SIZE / 2 * _rectTransform.lossyScale.y)
                return true;
        }

        return false;
    }

    public bool IsAlreadyPut()
    {
        if (this._black.activeSelf || this._white.activeSelf)
        {
            return true;
        }

        return false;
    }


    public void SetColor(Const.PLAYER_TURN turn)
    {
        _black.SetActive(turn == Const.PLAYER_TURN.BLACK);
        _white.SetActive(turn == Const.PLAYER_TURN.WHITE);
    }
}
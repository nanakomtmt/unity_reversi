using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private int _x;

    private int _y;
    private float SQUARE_SIZE = 50f;

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
        Debug.Log((xPosition, yPosition));
        this.transform.localPosition = new Vector3(xPosition, yPosition, 0);
        this.transform.SetParent(parent, true);

        _black.SetActive(x == 3 && y == 3 || x == 4 && y == 4);
        _white.SetActive(x == 3 && y == 4 || x == 4 && y == 3);
    }

    public bool HitTest(Vector3 pos)
    {
        float hitRate = 1.0f;
        if (Mathf.Abs(pos.x - this.transform.position.x) <
            SQUARE_SIZE * hitRate * _rectTransform.lossyScale.x)
        {
            if (Mathf.Abs(pos.y - this.transform.position.y) <
                SQUARE_SIZE * hitRate * _rectTransform.lossyScale.y)
                return true;
        }

        return false;
    }

    public void SetColor(PLAYER_TURN turn)
    {
        _black.SetActive(turn == PLAYER_TURN.BLACK);
        _white.SetActive(turn == PLAYER_TURN.WHITE);
    }
}
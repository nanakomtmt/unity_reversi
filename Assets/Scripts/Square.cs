using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private GameObject _black, _white, _frame;

    private RectTransform _rectTransform;
    public int x { get; private set; }

    public int y { get; private set; }


    public Const.PLAYER Color
    {
        get
        {
            if (_white.activeSelf)
                return Const.PLAYER.WHITE;
            return Const.PLAYER.BLACK;
        }
    }


    public bool isWhite => _white.activeSelf;

    public bool isBlack => _black.activeSelf;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Setup(Transform parent, float xPosition, float yPosition, int x, int y)
    {
        _rectTransform = GetComponent<RectTransform>();
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(xPosition, yPosition, 0);
        transform.SetParent(parent, true);
        ResetBoard();
    }

    public void ResetBoard()
    {
        _black.SetActive((x == 3 && y == 3) || (x == 4 && y == 4));
        _white.SetActive((x == 3 && y == 4) || (x == 4 && y == 3));
    }

    public bool HitTest(Vector3 pos)
    {
        if (Mathf.Abs(pos.x - transform.position.x) <
            Const.SQUARE_SIZE / 2 * _rectTransform.lossyScale.x)
            if (Mathf.Abs(pos.y - transform.position.y) <
                Const.SQUARE_SIZE / 2 * _rectTransform.lossyScale.y)
                return true;

        return false;
    }

    public bool IsAlreadyPut()
    {
        if (_black.activeSelf || _white.activeSelf) return true;

        return false;
    }


    public void SetColor(Const.PLAYER turn)
    {
        _black.SetActive(turn == Const.PLAYER.BLACK);
        _white.SetActive(turn == Const.PLAYER.WHITE);
    }

    public void SetFrame(bool isShow)
    {
        _frame.SetActive(isShow);
    }
}
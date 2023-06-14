using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private readonly List<Square> _squares = new();

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Setup(Transform parentTransform, Const.PLAYER turn)
    {
        var boardSize = Const.SQUARE_SIZE * Const.SQUARE_NUMBERS;
        var xPosition = parentTransform.localPosition.x - boardSize * 2;
        // Const.SQUARE_SIZE / 2;
        var yPosition = parentTransform.localPosition.y + boardSize / 2 -
                        Const.SQUARE_SIZE / 2;

        for (var i = 0; i < Const.SQUARE_NUMBERS; i++)
        for (var j = 0; j < Const.SQUARE_NUMBERS; j++)
        {
            var xPos = xPosition + Const.SQUARE_SIZE * j;
            var yPos = yPosition - Const.SQUARE_SIZE * i;

            var go = Utils.InstantiatePrefab(Const.SQUARE_PREFAB_PATH, parentTransform);
            var square = go.GetComponent<Square>();
            square.Setup(parentTransform, xPos, yPos, j, i);
            _squares.Add(square);
        }

        SetFrame(turn);
    }

    public void ResetBoard()
    {
        foreach (var square in _squares) square.ResetBoard();
    }

    public bool CanPutSquare(Square targetSquare, Const.PLAYER turn)
    {
        var isAlreadyPut = targetSquare.IsAlreadyPut();
        if (isAlreadyPut) return false;
        return IsExistSquareInDirection(targetSquare, turn);
    }

    private bool IsExistSquareInDirection(Square targetSquare,
        Const.PLAYER turn)
    {
        foreach (var (nextX, nextY, condition) in Utils.directions(targetSquare))
        foreach (var square in _squares)
        {
            var color = square.Color;
            if ((int)color != (int)turn && square.IsAlreadyPut() && condition(square))
            {
                var x = nextX(square.x);
                var y = nextY(square.y);
                ;
                while (GetSquareFromXY(x, y) != null && GetSquareFromXY(x, y).IsAlreadyPut())
                {
                    if ((int)GetSquareFromXY(x, y).Color == (int)turn)
                        return true;
                    x = nextX(x);
                    y = nextY(y);
                }
            }
        }

        return false;
    }


    public void SetFrame(Const.PLAYER turn)
    {
        foreach (var square in _squares) square.SetFrame(CanPutSquare(square, turn));
    }

    public void ChangeSquaresColor(Square targetSquare,
        Const.PLAYER turn)
    {
        var changeList = new List<Square>();
        foreach (var (nextX, nextY, condition) in Utils.directions(targetSquare))
        {
            foreach (var square in _squares)
            {
                var color = square.Color;
                if ((int)color != (int)turn && square.IsAlreadyPut() && condition(square))
                {
                    var x = nextX(square.x);
                    var y = nextY(square.y);
                    changeList.Add(square);
                    while (GetSquareFromXY(x, y) != null && GetSquareFromXY(x, y).IsAlreadyPut())
                    {
                        var s = GetSquareFromXY(x, y);
                        changeList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var ss in changeList) ss.SetColor(turn);
                            break;
                        }

                        x = nextX(x);
                        y = nextY(y);
                    }
                }
            }

            changeList.Clear();
        }
    }

    public Square GetSquareFromXY(int x, int y)
    {
        foreach (var square in _squares)
            if (square.x == x && square.y == y)
                return square;
        return null;
    }

    public Square GetSquareFromPosition(Vector3 pos)
    {
        foreach (var square in _squares)
            if (square.HitTest(pos))
                return square;
        return null;
    }

    public Dictionary<Const.PLAYER, int> CountBlackAndWhite()
    {
        var count = new Dictionary<Const.PLAYER, int>();
        count[Const.PLAYER.WHITE] = 0;
        count[Const.PLAYER.BLACK] = 0;

        foreach (var square in _squares)
        {
            if (square.isWhite) count[Const.PLAYER.WHITE]++;

            if (square.isBlack) count[Const.PLAYER.BLACK]++;
        }

        return count;
    }
}
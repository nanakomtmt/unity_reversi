using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private readonly List<Square> _changeColorList = new();
    private readonly List<Square> _diagonalSquares = new();
    private readonly List<Square> _squares = new();
    private readonly int SQUARE_NUMBERS = 8;
    private readonly float SQUARE_SIZE = 70f;
    private List<Square> _horizontalSquares = new();
    private List<Square> _verticalSquares = new();

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Setup(Transform parentTransform)
    {
        var xPosition = parentTransform.localPosition.x - 560 - 70 / 2;
        var yPosition = parentTransform.localPosition.y + 280 - 70 / 2;


        for (var i = 0; i < SQUARE_NUMBERS; i++)
        for (var j = 0; j < SQUARE_NUMBERS; j++)
        {
            var xPos = xPosition - SQUARE_SIZE * j;
            var yPos = yPosition - SQUARE_SIZE * i;
            var go = Utils.InstantiatePrefab("Prefabs/Square", parentTransform);
            var square = go.GetComponent<Square>();
            square.Setup(parentTransform, xPos, yPos, j, i);
            _squares.Add(square);
        }
    }

    public void ResetBoard()
    {
        foreach (var square in _squares) square.ResetBoard();
    }

    public bool CanPutSquare(Square targetSquare, Const.PLAYER_TURN turn)
    {
        var canput = false;
        //もう置かれているところかどうかのチェック
        var ret = targetSquare.IsAlreadyPut();
        if (ret) return false;

        //隣接箇所に違う色があるかどうかのチェック
        var targetSquareX = targetSquare.x;
        var targetSquareY = targetSquare.y;
        _verticalSquares = _squares.FindAll(square => { return square.x == targetSquareX; });
        //上
        foreach (var square in _verticalSquares)
        {
            _changeColorList.Clear();
            Debug.Log($"x{square.x},y{square.y}");
            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.y - targetSquareY == 1.0)
                {
                    _changeColorList.Add(square);
                    var y = square.y + 1;

                    while (GetSquareFromXY(targetSquareX, y) != null)
                    {
                        // Debug.Log($"y:{y}");
                        var s = GetSquareFromXY(targetSquareX, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        y++;
                    }
                }
        }

//下
        foreach (var square in _verticalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (targetSquareY - square.y == 1.0)
                {
                    _changeColorList.Add(square);
                    var y = square.y - 1;

                    while (GetSquareFromXY(targetSquareX, y) != null)
                    {
                        var s = GetSquareFromXY(targetSquareX, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        y--;
                    }
                }
        }


        _horizontalSquares = _squares.FindAll(square => { return square.y == targetSquareY; });

        // 左
        foreach (var square in _horizontalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (targetSquareX - square.x == 1.0)
                {
                    _changeColorList.Add(square);
                    var x = square.x - 1;

                    while (GetSquareFromXY(x, targetSquareY) != null)
                    {
                        var s = GetSquareFromXY(x, targetSquareY);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x--;
                    }
                }
        }

//右
        foreach (var square in _horizontalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.x - targetSquareX == 1.0)
                {
                    _changeColorList.Add(square);
                    var x = square.x + 1;

                    while (GetSquareFromXY(x, targetSquareY) != null)
                    {
                        var s = GetSquareFromXY(x, targetSquareY);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x++;
                    }
                }
        }

        _diagonalSquares.Clear();
        foreach (var square in _squares)
            if (square.x + square.y == targetSquareX + targetSquareY ||
                square.x - square.y == targetSquareX - targetSquareY)
                _diagonalSquares.Add(square);


        //右上
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.x - targetSquareX == 1.0 && square.y - targetSquareY == -1)
                {
                    _changeColorList.Add(square);
                    var x = square.x + 1;
                    var y = square.y - 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x++;
                        y--;
                    }
                }
        }

        //右下
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.x - targetSquareX == 1.0 && square.y - targetSquareY == 1)
                {
                    _changeColorList.Add(square);
                    var x = square.x + 1;
                    var y = square.y + 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x++;
                        y++;
                    }
                }
        }

        //左上
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.x - targetSquareX == -1 && square.y - targetSquareY == -1)
                {
                    _changeColorList.Add(square);
                    var x = square.x - 1;
                    var y = square.y - 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x--;
                        y--;
                    }
                }
        }

        //左下
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            var color = square.Color;

            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (square.x - targetSquareX == -1 && square.y - targetSquareY == 1)
                {
                    _changeColorList.Add(square);
                    var x = square.x - 1;
                    var y = square.y + 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int)s.Color == (int)turn)
                        {
                            foreach (var sq in _changeColorList) sq.SetColor(turn);

                            canput = true;
                            break;
                        }


                        x--;
                        y++;
                    }
                }
        }

        return canput;
    }

    public void ChangeSquaresColor(Square targetSquare, Const.PLAYER_TURN turn)
    {
        float x = targetSquare.x;
        float y = targetSquare.y;
        foreach (var square in _verticalSquares)
        {
            var color = square.Color;


            if ((int)color != (int)turn && square.IsAlreadyPut())
                if (Math.Abs(square.y - y) == 1.0)
                {
                    // while ()
                    // {
                    // }
                }
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

    public Dictionary<Const.COLOR, int> CountBlackAndWhite()
    {
        var count = new Dictionary<Const.COLOR, int>();
        count[Const.COLOR.WHITE] = 0;
        count[Const.COLOR.BLACK] = 0;

        foreach (var square in _squares)
        {
            if (square.isWhite) count[Const.COLOR.WHITE]++;

            if (square.isBlack) count[Const.COLOR.BLACK]++;
        }

        return count;
    }
}
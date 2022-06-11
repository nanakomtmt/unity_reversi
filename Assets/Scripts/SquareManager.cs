using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    private float SQUARE_SIZE = 50f;
    private List<Square> _squares = new List<Square>();
    private int SQUARE_NUMBERS = 8;
    List<Square> _diagonalSquares = new List<Square>();
    private List<Square> _verticalSquares = new List<Square>();
    private List<Square> _horizontalSquares = new List<Square>();
    List<Square> _changeColorList = new List<Square>();

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


        for (int i = 0; i < SQUARE_NUMBERS; i++)
        {
            for (int j = 0; j < SQUARE_NUMBERS; j++)
            {
                float xPos = xPosition + SQUARE_SIZE * j;
                float yPos = yPosition - SQUARE_SIZE * i;
                GameObject go = Utils.InstantiatePrefab("Prefabs/Square", parentTransform);
                Square square = go.GetComponent<Square>();
                square.Setup(parentTransform, xPos, yPos, j, i);
                _squares.Add(square);
            }
        }
    }

    public void ResetBoard()
    {
        foreach (var square in _squares)
        {
            square.ResetBoard();
        }
    }

    public bool CanPutSquare(Square targetSquare, Const.PLAYER_TURN turn)
    {
        bool canput = false;
        //もう置かれているところかどうかのチェック
        var ret = targetSquare.IsAlreadyPut();
        if (ret)
        {
            return false;
        }

        //隣接箇所に違う色があるかどうかのチェック
        int targetSquareX = targetSquare.x;
        int targetSquareY = targetSquare.y;
        _verticalSquares = _squares.FindAll((square) => { return square.x == targetSquareX; });
        //上
        foreach (var square in _verticalSquares)
        {
            _changeColorList.Clear();
            Debug.Log($"x{square.x},y{square.y}");
            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.y - targetSquareY == 1.0)
                {
                    _changeColorList.Add(square);
                    int y = square.y + 1;

                    while (GetSquareFromXY(targetSquareX, y) != null)
                    {
                        // Debug.Log($"y:{y}");
                        var s = GetSquareFromXY(targetSquareX, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        y++;
                    }
                }
            }
        }

//下
        foreach (var square in _verticalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (targetSquareY - square.y == 1.0)
                {
                    _changeColorList.Add(square);
                    int y = square.y - 1;

                    while (GetSquareFromXY(targetSquareX, y) != null)
                    {
                        var s = GetSquareFromXY(targetSquareX, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        y--;
                    }
                }
            }
        }


        _horizontalSquares = _squares.FindAll((square) => { return square.y == targetSquareY; });

        // 左
        foreach (var square in _horizontalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (targetSquareX - square.x == 1.0)
                {
                    _changeColorList.Add(square);
                    int x = square.x - 1;

                    while (GetSquareFromXY(x, targetSquareY) != null)
                    {
                        var s = GetSquareFromXY(x, targetSquareY);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x--;
                    }
                }
            }
        }

//右
        foreach (var square in _horizontalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.x - targetSquareX == 1.0)
                {
                    _changeColorList.Add(square);
                    int x = square.x + 1;

                    while (GetSquareFromXY(x, targetSquareY) != null)
                    {
                        var s = GetSquareFromXY(x, targetSquareY);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x++;
                    }
                }
            }
        }

        _diagonalSquares.Clear();
        foreach (var square in _squares)
        {
            if (square.x + square.y == targetSquareX + targetSquareY ||
                square.x - square.y == targetSquareX - targetSquareY)
            {
                _diagonalSquares.Add(square);
            }
        }


        //右上
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.x - targetSquareX == 1.0 && square.y - targetSquareY == -1)
                {
                    _changeColorList.Add(square);
                    int x = square.x + 1;
                    int y = square.y - 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x++;
                        y--;
                    }
                }
            }
        }

        //右下
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.x - targetSquareX == 1.0 && square.y - targetSquareY == 1)
                {
                    _changeColorList.Add(square);
                    int x = square.x + 1;
                    int y = square.y + 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x++;
                        y++;
                    }
                }
            }
        }

        //左上
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.x - targetSquareX == -1 && square.y - targetSquareY == -1)
                {
                    _changeColorList.Add(square);
                    int x = square.x - 1;
                    int y = square.y - 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x--;
                        y--;
                    }
                }
            }
        }

        //左下
        foreach (var square in _diagonalSquares)
        {
            _changeColorList.Clear();

            Const.COLOR color = square.Color;

            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (square.x - targetSquareX == -1 && square.y - targetSquareY == 1)
                {
                    _changeColorList.Add(square);
                    int x = square.x - 1;
                    int y = square.y + 1;
                    while (GetSquareFromXY(x, y) != null)
                    {
                        var s = GetSquareFromXY(x, y);
                        _changeColorList.Add(s);
                        if (s.IsAlreadyPut() && (int) s.Color == (int) turn)
                        {
                            foreach (var sq in _changeColorList)
                            {
                                sq.SetColor(turn);
                            }

                            canput = true;
                            break;
                        }


                        x--;
                        y++;
                    }
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
            Const.COLOR color = square.Color;


            if ((int) color != (int) turn && square.IsAlreadyPut())
            {
                if (Math.Abs(square.y - y) == 1.0)
                {
                    // while ()
                    // {
                    // }
                }
            }
        }
    }

    public Square GetSquareFromXY(int x, int y)
    {
        foreach (var square in _squares)
        {
            if (square.x == x && square.y == y)
            {
                return square;
            }
        }

        return null;
    }

    public Square GetSquareFromPosition(Vector3 pos)
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

    public Dictionary<Const.COLOR, int> CountBlackAndWhite()
    {
        Dictionary<Const.COLOR, int> count = new Dictionary<Const.COLOR, int>();
        count[Const.COLOR.WHITE] = 0;
        count[Const.COLOR.BLACK] = 0;

        foreach (var square in _squares)
        {
            if (square.isWhite)
            {
                count[Const.COLOR.WHITE]++;
            }

            if (square.isBlack)
            {
                count[Const.COLOR.BLACK]++;
            }
        }

        return count;
    }
}
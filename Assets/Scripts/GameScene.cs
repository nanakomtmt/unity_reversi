using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PLAYER_TURN
{
    WHITE,
    BLACK,
}

public class GameScene : MonoBehaviour
{
    SquareManager _squareManager;
    [SerializeField] GameObject board;
    [SerializeField] private GameObject _canvas;

    [SerializeField] private Text _playerTurn;
    private String WHITE = "白";
    private String BLACK = "黒";
    private PLAYER_TURN _nowTurn;


    // Start is called before the first frame update
    void Start()
    {
        _squareManager = board.GetComponent<SquareManager>();
        _squareManager.Setup(board.transform);
        _playerTurn.text = WHITE;
        _nowTurn = PLAYER_TURN.WHITE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = Input.mousePosition;
            Square square = _squareManager.GetSquare(position);
            if (square != null && square.IsNotAlreadyPut())
            {
                //そもそもおけるとこか置けないところかの判定がいる
                square.SetColor(_nowTurn);

                OnChangeTurn();
            }
        }
    }

    void OnChangeTurn()
    {
        if (_nowTurn == PLAYER_TURN.WHITE)
        {
            _playerTurn.text = BLACK;
            _nowTurn = PLAYER_TURN.BLACK;
        }
        else
        {
            _playerTurn.text = WHITE;
            _nowTurn = PLAYER_TURN.WHITE;
        }
    }

    public async void OnClickResetButton()
    {
        await CommonDialog.Open(_canvas.transform, "リセット", "ゲームをリセットしてもいいですか?", ((result) =>
        {
            if (result == CommonDialog.Result.OK)
            {
                _playerTurn.text = WHITE;
                _nowTurn = PLAYER_TURN.WHITE;
                _squareManager.ResetBoard();
            }
        }), CommonDialog.Mode.OK_CANCEL);
    }
}
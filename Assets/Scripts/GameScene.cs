using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScene : MonoBehaviour
{
    SquareManager _squareManager;
    [SerializeField] GameObject board;
    [SerializeField] private GameObject _canvas;

    [SerializeField] private Text _playerTurn, _whiteScore, _blackScore;
    private String WHITE = "白";
    private String BLACK = "黒";
    private Const.PLAYER_TURN _nowTurn;


    // Start is called before the first frame update
    void Start()
    {
        _squareManager = board.GetComponent<SquareManager>();
        _squareManager.Setup(board.transform);
        _playerTurn.text = WHITE;
        _nowTurn = Const.PLAYER_TURN.WHITE;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = Input.mousePosition;
            Square square = _squareManager.GetSquareFromPosition(position);
            if (square != null)
            {
                if (_squareManager.CanPutSquare(square, _nowTurn))
                {
                    //そもそもおけるとこか置けないところかの判定がいる
                    square.SetColor(_nowTurn);
                    OnChangeTurn();
                    ChangeScoreBoard();
                }
            }
        }
    }

    async void ChangeScoreBoard()
    {
        var count = _squareManager.CountBlackAndWhite();
        _blackScore.text = count[Const.COLOR.BLACK].ToString();
        _whiteScore.text = count[Const.COLOR.WHITE].ToString();
        if (count[Const.COLOR.BLACK] + count[Const.COLOR.WHITE] == 64)
        {
            var winner = "白";
            if (count[Const.COLOR.BLACK] > count[Const.COLOR.WHITE])
            {
                winner = "黒";
            }

            var text = $"{winner}の勝ちです！！";

            if (count[Const.COLOR.BLACK] == count[Const.COLOR.WHITE])
            {
                text = "引き分けです！";
            }

            await CommonDialog.Open(_canvas.transform, "結果", text, (result => { OnClickResetButton(); }));
        }
    }

    void OnChangeTurn()
    {
        if (_nowTurn == Const.PLAYER_TURN.WHITE)
        {
            _playerTurn.text = BLACK;
            _nowTurn = Const.PLAYER_TURN.BLACK;
        }
        else
        {
            _playerTurn.text = WHITE;
            _nowTurn = Const.PLAYER_TURN.WHITE;
        }
    }

    public async void OnClickResetButton()
    {
        await CommonDialog.Open(_canvas.transform, "リセット", "ゲームをリセットしてもいいですか?", ((result) =>
        {
            if (result == CommonDialog.Result.OK)
            {
                _playerTurn.text = WHITE;
                _nowTurn = Const.PLAYER_TURN.WHITE;
                _squareManager.ResetBoard();
                _blackScore.text = "2";
                _whiteScore.text = "2";
            }
        }), CommonDialog.Mode.OK_CANCEL);
    }

    public async void OnClickSkipButton()
    {
        await CommonDialog.Open(_canvas.transform, "スキップ", "次の人の番にスキップしてもいいですか?", ((result) =>
        {
            if (result == CommonDialog.Result.OK)
            {
                OnChangeTurn();
            }
        }), CommonDialog.Mode.OK_CANCEL);
    }
}
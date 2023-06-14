using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject _canvas;

    [SerializeField] private Text _playerTurn, _whiteScore, _blackScore;
    [SerializeField] private SquareManager _squareManager;
    private readonly string BLACK = "黒";
    private readonly string WHITE = "白";
    private Const.PLAYER _nowTurn = Const.PLAYER.WHITE;


    // Start is called before the first frame update
    private void Start()
    {
        _playerTurn.text = WHITE;

        _squareManager.Setup(board.transform, _nowTurn);
        // _squareManager.OnChangeTurn(_nowTurn);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = Input.mousePosition;
            var square = _squareManager.GetSquareFromPosition(position);
            if (square != null)
                if (_squareManager.CanPutSquare(square, _nowTurn))
                {
                    //そもそもおけるとこか置けないところかの判定がいる
                    square.SetColor(_nowTurn);
                    _squareManager.ChangeSquaresColor(square, _nowTurn);
                    OnChangeTurn();
                    ChangeScoreBoard();

                    _squareManager.SetFrame(_nowTurn);
                }
        }
    }

    private async void ChangeScoreBoard()
    {
        var count = _squareManager.CountBlackAndWhite();
        _blackScore.text = count[Const.PLAYER.BLACK].ToString();
        _whiteScore.text = count[Const.PLAYER.WHITE].ToString();
        if (count[Const.PLAYER.BLACK] + count[Const.PLAYER.WHITE] == Const.SQUARE_NUMBERS * Const.SQUARE_NUMBERS)
        {
            var winner = WHITE;
            if (count[Const.PLAYER.BLACK] > count[Const.PLAYER.WHITE]) winner = BLACK;

            var text = $"{winner}の勝ちです！！";

            if (count[Const.PLAYER.BLACK] == count[Const.PLAYER.WHITE]) text = "引き分けです！";

            await CommonDialog.Open(_canvas.transform, "結果", text, result => { OnClickResetButton(); });
        }
    }

    private void OnChangeTurn()
    {
        if (_nowTurn == Const.PLAYER.WHITE)
        {
            _playerTurn.text = BLACK;
            _nowTurn = Const.PLAYER.BLACK;
        }
        else
        {
            _playerTurn.text = WHITE;
            _nowTurn = Const.PLAYER.WHITE;
        }
    }

    public async void OnClickResetButton()
    {
        await CommonDialog.Open(_canvas.transform, "リセット", "ゲームをリセットしてもいいですか?", result =>
        {
            if (result == CommonDialog.Result.OK)
            {
                _playerTurn.text = WHITE;
                _nowTurn = Const.PLAYER.WHITE;
                _squareManager.ResetBoard();
                _blackScore.text = "2";
                _whiteScore.text = "2";
                _squareManager.SetFrame(_nowTurn);
            }
        }, CommonDialog.Mode.OK_CANCEL);
    }

    public async void OnClickSkipButton()
    {
        await CommonDialog.Open(_canvas.transform, "スキップ", "次の人の番にスキップしてもいいですか?", result =>
        {
            if (result == CommonDialog.Result.OK) OnChangeTurn();
        }, CommonDialog.Mode.OK_CANCEL);
    }
}
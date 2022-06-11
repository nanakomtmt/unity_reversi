using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CommonDialog : DialogBaseListener
{
    [SerializeField] Text _title = null;
    [SerializeField] Text _text = null;
    [SerializeField] private Button _cancel;

    public enum Mode
    {
        OK,
        OK_CANCEL
    }

    public string text
    {
        set { _text.text = value; }
    }

    public string title
    {
        set { _title.text = value; }
    }

    public static async UniTask Open(Transform parent, string title, string text, Action<Result> onClose,
        Mode mode = Mode.OK)
    {
        var go = Utils.InstantiatePrefab("Prefabs/CommonDialog", parent);
        CommonDialog cd = go.GetComponent<CommonDialog>();
        cd.Setup(title, text, onClose, mode);
    }

    private void Setup(string title, string text, Action<Result> onClose, Mode mode)
    {
        this.title = title;
        this.text = text;
        this.OnClose = onClose;
        _cancel.gameObject.SetActive(mode == Mode.OK_CANCEL);
    }

    #region 列挙型

    public enum Result
    {
        OK,
        Cancel,
    }

    #endregion

    #region プロパティ

    /// <summary>ダイアログが操作されたときに発生するイベント</summary>
    public Action<Result> OnClose { get; set; }

    #endregion


    public CommonDialog()
    {
    }


    /// <summary>
    /// OKボタンが押されたとき
    /// </summary>
    public void OnClickOK()
    {
        OnClose?.Invoke(Result.OK);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Cancelボタンが押されたとき/ダイアログの外側をクリックした時
    /// </summary>
    public void OnClickCancel()
    {
        // イベント通知先があれば通知してダイアログを破棄
        OnClose?.Invoke(Result.Cancel);
        Destroy(this.gameObject);
    }

    public override bool OnClickBlocker()
    {
        return true;
    }
}
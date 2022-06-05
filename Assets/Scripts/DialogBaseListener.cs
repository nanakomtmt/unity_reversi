using UnityEngine;

public abstract class DialogBaseListener : MonoBehaviour
{
    public abstract bool OnClickBlocker();

    public void Close()
    {
        this.GetComponentInParent<DialogBase>().Close();
    }

    public void CloseNow()
    {
        this.GetComponentInParent<DialogBase>().CloseNow();
    }
}
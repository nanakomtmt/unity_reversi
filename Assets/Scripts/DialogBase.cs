using UnityEngine;

public class DialogBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickBlocker()
    {
        var listener = transform.GetComponentInChildren<DialogBaseListener>();
        if (listener != null)
        {
            if (listener.OnClickBlocker())
            {
                Close();
            }
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        Destroy();
        // var animator = GetComponentInChildren<Animator>();
        // animator.SetTrigger("Close");
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void CloseNow()
    {
        Destroy();
    }
}
//using System;

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public static class Utils
{
    static System.Random _random = new System.Random();
    public static int Rand(int max)
    {
        return _random.Next(0, max);
    }
    public static bool Rand(float percent)
    {
        int threashold = (int)(100.0f * percent);
        return _random.Next(0, 100) < threashold;
    }
    public static string MakeStorageImagePath(string imgName)
    {
        if (!imgName.StartsWith("/"))
        {
            imgName = "/" + imgName;
        }
        return Application.persistentDataPath + imgName;
    }
    public static string MakeStreamingAssetsPath(string filename)
    {
        if (filename.StartsWith("/"))
        {
            filename = filename.Substring(1);
        }
        return System.IO.Path.Combine(Application.streamingAssetsPath, filename);
    }

    public static string GetImagePath(string imgName)
    {
        string stragePath = MakeStorageImagePath(imgName);
        if (File.Exists(stragePath))
        {
            return stragePath;
        }
        string assetsPath = MakeStreamingAssetsPath(imgName);
        if (File.Exists(assetsPath))
        {
            return assetsPath;
        }
        return null;
    }

   
  

    public static void FixFontSize(Text text, float maxWidth)
    {
        float textWidth = text.preferredWidth;

        if (maxWidth < textWidth)
        {
            float fixNum = maxWidth / textWidth;

            text.GetComponent<RectTransform>().localScale = new Vector3(fixNum, 1.0f, 1.0f);
        }
    }

    public static long Clamp(long src, long min, long max)
    {
        long ret = src;
        if(src < min)
        {
            ret = min;
        }
        if(src > max)
        {
            ret = max;
        }
        return ret;
    }

    public static void MultiplyLocalScale(Transform transform, Vector3 vec3)
    {
        var scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * vec3.x, scale.y * vec3.y, scale.z * vec3.z);
    }
    public static float LimitVal(float val, float min, float max)
    {
        if(val < min)
        {
            return min;
        }
        if(val > max)
        {
            return max;
        }
        return val;
    }
    public static void DestroyAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    private static Vector3 TouchPosition = Vector3.zero;

    /// <summary>
    /// タッチ情報を取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public static TouchInfo GetTouch()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
            if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
            if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                return (TouchInfo)((int)Input.GetTouch(0).phase);
            }
        }
        return TouchInfo.None;
    }

    /// <summary>
    /// タッチポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchPosition()
    {
        if (Application.isEditor)
        {
            TouchInfo touch = Utils.GetTouch();
            if (touch != TouchInfo.None) { return Input.mousePosition; }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }
        return Vector3.zero;
    }

    /// <summary>
    /// タッチワールドポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <param name='camera'>カメラ</param>
    /// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }
    /// <summary>
    /// ダイアログにアニメーションを追加
    /// </summary>
    /// <param name="dialogPrefab">ダイアログのプレハブ名</param>
    /// <param name="parentTransform">アニメーションプレハブをセットする親オブジェクト</param>
    /// <return name="childObj">ダイアログオブジェクト</return>
    public static GameObject OpenDialog(string dialogPrefab, Transform parentTransform)
    {
        // アニメーションプレハブを生成し、親オブジェクトにセット
        GameObject openDialogPrefabObj = (GameObject)Resources.Load("Prefabs/Common/DialogBase");
        GameObject parentObj = Object.Instantiate(openDialogPrefabObj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        parentObj.transform.SetParent(parentTransform, false);

        // ダイアログプレハブを生成し、アニメーションプレハブにセット
        GameObject dialogPrefabObj = (GameObject)Resources.Load(dialogPrefab);
        GameObject childObj = Object.Instantiate(dialogPrefabObj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        dialogPrefabObj.transform.localPosition = new Vector3();
        var dialog = parentObj.transform.Find("Dialog");
        childObj.transform.SetParent(dialog.transform, false);

        return childObj;
    }
    public static DirectoryInfo SafeCreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return null;
        }
        return Directory.CreateDirectory(path);
    }
    public static GameObject InstantiatePrefab(string prefabPath, Transform parent = null)
    {
        GameObject resource = (GameObject)Resources.Load(prefabPath);
        if(resource == null)
        {
            Debug.LogError($"Prefab Resources load failed({prefabPath})");
        }
        GameObject gameObject = MonoBehaviour.Instantiate(resource, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        if (parent)
        {
            gameObject.transform.SetParent(parent, false);
        }
        return gameObject;
    }
    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
    public static Transform SetX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        return transform;
    }
    public static Transform SetY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        return transform;
    }

    public static void AdjustScreenArea(RectTransform panel, Rect safeArea)
    {
        if (safeArea.x != 0)
        {
            var fixScreenWidth = safeArea.width / Screen.width;
            panel.localScale = new Vector3(fixScreenWidth, fixScreenWidth, 1);
        }
    }

    public static int GetIntFromXmlElement(XElement element, string key, int defaultVal = 0)
    {
        var val = element.Descendants(key).FirstOrDefault();
        if (val != null)
        {
            return int.Parse(val.Value);
        }
        return defaultVal;
    }
    public static int? GetIntFromXmlElementOrNull(XElement element, string key)
    {
        var val = element.Descendants(key).FirstOrDefault();
        if (val != null)
        {
            return int.Parse(val.Value);
        }
        return null;
    }

    public static long DateTimeToUnixTime(DateTime dt)
    {
        var dto = new DateTimeOffset(dt.ToUniversalTime().Ticks, new TimeSpan(+00, 00, 00));
        return dto.ToUnixTimeSeconds();
    }

    public static DateTime UnixTimeToDateTime(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
    }

}
public enum TouchInfo
{
    /// <summary>
    /// タッチなし
    /// </summary>
    None = 99,

    // 以下は UnityEngine.TouchPhase の値に対応
    /// <summary>
    /// タッチ開始
    /// </summary>
    Began = 0,
    /// <summary>
    /// タッチ移動
    /// </summary>
    Moved = 1,
    /// <summary>
    /// タッチ静止
    /// </summary>
    Stationary = 2,
    /// <summary>
    /// タッチ終了
    /// </summary>
    Ended = 3,
    /// <summary>
    /// タッチキャンセル
    /// </summary>
    Canceled = 4,
}
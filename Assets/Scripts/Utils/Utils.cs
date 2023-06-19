//using System;

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

public static class Utils
{
    private static readonly Random _random = new();

    private static Vector3 TouchPosition = Vector3.zero;

    public static Const.PLAYER getOtherColor(Const.PLAYER color)
    {
        return color == Const.PLAYER.BLACK ? Const.PLAYER.WHITE : Const.PLAYER.BLACK;
    }


    public static int Rand(int max)
    {
        return _random.Next(0, max);
    }

    public static bool Rand(float percent)
    {
        var threashold = (int)(100.0f * percent);
        return _random.Next(0, 100) < threashold;
    }

    public static string MakeStorageImagePath(string imgName)
    {
        if (!imgName.StartsWith("/")) imgName = "/" + imgName;

        return Application.persistentDataPath + imgName;
    }

    public static string MakeStreamingAssetsPath(string filename)
    {
        if (filename.StartsWith("/")) filename = filename.Substring(1);

        return Path.Combine(Application.streamingAssetsPath, filename);
    }

    public static string GetImagePath(string imgName)
    {
        var stragePath = MakeStorageImagePath(imgName);
        if (File.Exists(stragePath)) return stragePath;

        var assetsPath = MakeStreamingAssetsPath(imgName);
        if (File.Exists(assetsPath)) return assetsPath;

        return null;
    }


    public static void FixFontSize(Text text, float maxWidth)
    {
        var textWidth = text.preferredWidth;

        if (maxWidth < textWidth)
        {
            var fixNum = maxWidth / textWidth;

            text.GetComponent<RectTransform>().localScale = new Vector3(fixNum, 1.0f, 1.0f);
        }
    }

    public static long Clamp(long src, long min, long max)
    {
        var ret = src;
        if (src < min) ret = min;

        if (src > max) ret = max;

        return ret;
    }

    public static void MultiplyLocalScale(Transform transform, Vector3 vec3)
    {
        var scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * vec3.x, scale.y * vec3.y, scale.z * vec3.z);
    }

    public static float LimitVal(float val, float min, float max)
    {
        if (val < min) return min;

        if (val > max) return max;

        return val;
    }

    public static void DestroyAllChildren(Transform transform)
    {
        foreach (Transform child in transform) Object.Destroy(child.gameObject);
    }

    /// <summary>
    ///     タッチ情報を取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public static TouchInfo GetTouch()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0)) return TouchInfo.Began;

            if (Input.GetMouseButton(0)) return TouchInfo.Moved;

            if (Input.GetMouseButtonUp(0)) return TouchInfo.Ended;
        }
        else
        {
            if (Input.touchCount > 0) return (TouchInfo)(int)Input.GetTouch(0).phase;
        }

        return TouchInfo.None;
    }

    /// <summary>
    ///     タッチポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchPosition()
    {
        if (Application.isEditor)
        {
            var touch = GetTouch();
            if (touch != TouchInfo.None) return Input.mousePosition;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }

        return Vector3.zero;
    }

    /// <summary>
    ///     タッチワールドポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <param name='camera'>カメラ</param>
    /// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }

    /// <summary>
    ///     ダイアログにアニメーションを追加
    /// </summary>
    /// <param name="dialogPrefab">ダイアログのプレハブ名</param>
    /// <param name="parentTransform">アニメーションプレハブをセットする親オブジェクト</param>
    /// <return name="childObj">ダイアログオブジェクト</return>
    public static GameObject OpenDialog(string dialogPrefab, Transform parentTransform)
    {
        // アニメーションプレハブを生成し、親オブジェクトにセット
        var openDialogPrefabObj = (GameObject)Resources.Load("Prefabs/DialogBase");
        var parentObj =
            Object.Instantiate(openDialogPrefabObj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        parentObj.transform.SetParent(parentTransform, false);

        // ダイアログプレハブを生成し、アニメーションプレハブにセット
        var dialogPrefabObj = (GameObject)Resources.Load(dialogPrefab);
        var childObj = Object.Instantiate(dialogPrefabObj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        dialogPrefabObj.transform.localPosition = new Vector3();
        var dialog = parentObj.transform.Find("Dialog");
        childObj.transform.SetParent(dialog.transform, false);

        return childObj;
    }

    public static DirectoryInfo SafeCreateDirectory(string path)
    {
        if (Directory.Exists(path)) return null;

        return Directory.CreateDirectory(path);
    }

    public static GameObject InstantiatePrefab(string prefabPath, Transform parent = null)
    {
        var resource = (GameObject)Resources.Load(prefabPath);
        if (resource == null) Debug.LogError($"Prefab Resources load failed({prefabPath})");

        var gameObject = Object.Instantiate(resource, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        if (parent) gameObject.transform.SetParent(parent, false);

        return gameObject;
    }

    public static Transform Clear(this Transform transform)
    {
        foreach (Transform child in transform) Object.Destroy(child.gameObject);

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
        if (val != null) return int.Parse(val.Value);

        return defaultVal;
    }

    public static int? GetIntFromXmlElementOrNull(XElement element, string key)
    {
        var val = element.Descendants(key).FirstOrDefault();
        if (val != null) return int.Parse(val.Value);

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

    public static (Func<int, int> nextX, Func<int, int> nextY, Func<Square, bool> condition)[] directions(
        Square targetSquare)
    {
        return new (Func<int, int> nextX, Func<int, int> nextY, Func<Square, bool> condition)[]
            {
                (x => x, y => y + 1, square => square.y - targetSquare.y == 1.0 && square.x == targetSquare.x), // 上
                (x => x, y => y - 1, square => targetSquare.y - square.y == 1.0 && square.x == targetSquare.x), // 下
                (x => x - 1, y => y, square => targetSquare.x - square.x == 1.0 && square.y == targetSquare.y), // 左
                (x => x + 1, y => y, square => square.x - targetSquare.x == 1.0 && square.y == targetSquare.y), // 右
                (x => x + 1, y => y - 1,
                    square => square.x - targetSquare.x == 1.0 && square.y - targetSquare.y == -1), // 右上
                (x => x + 1, y => y + 1,
                    square => square.x - targetSquare.x == 1.0 && square.y - targetSquare.y == 1), // 右下
                (x => x - 1, y => y - 1,
                    square => square.x - targetSquare.x == -1 && square.y - targetSquare.y == -1), // 左上
                (x => x - 1, y => y + 1,
                    square => square.x - targetSquare.x == -1 && square.y - targetSquare.y == 1) // 左下
            }
            ;
    }
}

public enum TouchInfo
{
    /// <summary>
    ///     タッチなし
    /// </summary>
    None = 99,

    // 以下は UnityEngine.TouchPhase の値に対応
    /// <summary>
    ///     タッチ開始
    /// </summary>
    Began = 0,

    /// <summary>
    ///     タッチ移動
    /// </summary>
    Moved = 1,

    /// <summary>
    ///     タッチ静止
    /// </summary>
    Stationary = 2,

    /// <summary>
    ///     タッチ終了
    /// </summary>
    Ended = 3,

    /// <summary>
    ///     タッチキャンセル
    /// </summary>
    Canceled = 4
}
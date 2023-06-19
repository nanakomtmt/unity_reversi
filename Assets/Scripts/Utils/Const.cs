using System.Collections.Generic;

public class Const
{
    public enum PLAYER
    {
        WHITE,
        BLACK
    }

    public static readonly int SQUARE_NUMBERS = 8;

    public static readonly float SQUARE_SIZE = 70;
    public static string SQUARE_PREFAB_PATH = "Prefabs/Square";

    public static Dictionary<PLAYER, string> PLAYER_TURN_STR = new()
    {
        { PLAYER.WHITE, "白" },
        { PLAYER.BLACK, "黒" }
    };
}
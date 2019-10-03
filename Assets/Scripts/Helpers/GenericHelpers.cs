using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericHelpers
{
    public static bool FloatEqualsPrecision(float float1, float float2, float precision)
    {
        return float1 > float2 - precision && float1 < float2 + precision;           
    }

    public static void ResetGameInProgress()
    {
        PlayerPrefs.SetInt("GameInProgress", 0);
        PlayerPrefs.SetInt("GameInProgressScoreP1", 0);
        PlayerPrefs.SetInt("GameInProgressScoreP2", 0);
        PlayerPrefs.SetInt("GameInProgressSetP1", 0);
        PlayerPrefs.SetInt("GameInProgressSetP2", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal1P1", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal2P1", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal3P1", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal1P2", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal2P2", 0);
        PlayerPrefs.SetInt("GameInProgressFrozenGoal3P2", 0);
        PlayerPrefs.SetString("GameInProgressServingPlayerName", CurrentPlayer.PlayerOne.ToString());
    }
}

/*
 *  @file   UserData.cs
 *  @brief  ユーザーデータ
 *  @author Seki
 *  @date   2025/7/30
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData{
    // ステージ攻略データ
    public int clearStageNum = 0;
    // BGM音量データ
    public float bgmVolume = 5;
    // SE音量データ
    public float seVolume = 5;
}

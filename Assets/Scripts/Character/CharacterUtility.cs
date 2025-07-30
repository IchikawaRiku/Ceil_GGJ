/*
 *  @file   CharacterUtility.cs
 *  @brief  キャラクター関連実行処理
 *  @author Seki
 *  @date   2025/7/30
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtility{
    /// <summary>
    /// プレイヤーの生成
    /// </summary>
    public static void UsePlayer() {
        CharacterManager.instance.UsePlayer();
    }
    /// <summary>
    /// プレイヤーの未使用化
    /// </summary>
    public static void UnusePlayer() {
        CharacterManager.instance.UnusePlayer();
    }
    /// <summary>
    /// 幽霊の生成
    /// </summary>
    public static void UseSpirit() {
        CharacterManager.instance.UseSpirit();
    }
    /// <summary>
    /// 幽霊の未使用化
    /// </summary>
    public static void UnuseSpirit() {
        CharacterManager.instance.UnuseSpirit();
    }
}

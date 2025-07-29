/*
 *  @file   SystemObject.cs
 *  @brief  ゲーム全体で使用する機能の基底
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemObject : MonoBehaviour {
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Initialize();
}

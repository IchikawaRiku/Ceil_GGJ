/*
 *  @file   PartMainGame.cs
 *  @brief  メインゲームパート
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
    [SerializeField]
    private CharacterManager _characterManager = null;

    public override async UniTask Execute() {
        _characterManager.Initialize();
        await UniTask.CompletedTask;
    }
}

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
    public override async UniTask Execute() {
        await MenuManager.instance.Get<MenuSetting>().Open();
        await UniTask.CompletedTask;
    }
}

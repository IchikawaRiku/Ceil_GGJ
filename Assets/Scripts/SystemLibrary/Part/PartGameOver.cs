/*
 *  @file   PartGameOver.cs
 *  @brief  ゲームオーバーパート
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGameOver : PartBase {
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuGameOver>("Prefab/Menu/CanvasGameOver").Initialize();
    }

    public override async UniTask Execute() {
        await MenuManager.instance.Get<MenuGameOver>().Open();
    }
}

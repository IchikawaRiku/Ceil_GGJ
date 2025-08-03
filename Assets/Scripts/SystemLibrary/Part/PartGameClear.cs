/*
 *  @file   PartEnding.cs
 *  @brief  エンディングパート
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGameClear : PartBase {
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuGameClear>("Prefab/Menu/CanvasGameClear").Initialize();
    }
    public override async UniTask Execute() {
        await MenuManager.instance.Get<MenuGameClear>().Open();
    }
}

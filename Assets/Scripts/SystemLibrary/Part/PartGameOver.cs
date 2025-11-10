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
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuGameOver>("Prefab/Menu/CanvasGameOver").Initialize();
    }
    /// <summary>
    /// 実行処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Execute() {
        SoundManager.instance.PlayBGM(2);
        await MenuManager.instance.Get<MenuGameOver>().Open();
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    /// <returns></returns>
    public override async UniTask Teardown() {
        await base.Teardown();
        SoundManager.instance.StopBGM();
    }
}

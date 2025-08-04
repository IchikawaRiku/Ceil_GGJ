/*
 *  @file   PartGameOver.cs
 *  @brief  �Q�[���I�[�o�[�p�[�g
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
        SoundManager.instance.PlayBGM(2);
        await MenuManager.instance.Get<MenuGameOver>().Open();
    }

    public override async UniTask Teardown() {
        await base.Teardown();
        SoundManager.instance.StopBGM();
    }
}

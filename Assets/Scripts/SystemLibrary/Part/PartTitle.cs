/*
 *  @file   PartTitle.cs
 *  @brief  タイトルパート
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuTitle>("Prefab/Menu/CanvasTitle").Initialize();
        await MenuManager.instance.Get<MenuStageSelect>("Prefab/Menu/CanvasStageSelect").Initialize();
        await MenuManager.instance.Get<MenuSetting>("Prefab/Menu/CanvasSettings").Initialize();
        await MenuManager.instance.Get<MenuCredit>("Prefab/Menu/CanvasCredit").Initialize();
    }
    public override async UniTask Execute() {
        SoundManager.instance.PlayBGM(0);
        // タイトルメニュー表示]
        if (!MenuGameClear.isTitleSkip) await MenuManager.instance.Get<MenuTitle>().Open();
        await MenuManager.instance.Get<MenuStageSelect>().Open();
        // パート遷移
        eStageStage stage = MenuManager.instance.Get<MenuStageSelect>().stageNum;
        if (stage == eStageStage.Max) {
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        } else {
            await StageManager.instance.TransitionStage(stage);
            UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        }
    }

    public override async UniTask Teardown() {
        await base.Teardown();
        SoundManager.instance.StopBGM();
    }
}

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
    [SerializeField]
    private StageManager _stageManager = null;
    //入力受付
    private MainGameProcessor _mainProcessor = null;
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuInGameMenu>("Prefab/Menu/CanvasInGameMenu").Initialize();
        _mainProcessor = new MainGameProcessor();
        await _characterManager.Initialize();
        await _stageManager.Initialize();
        _mainProcessor = new MainGameProcessor();
        _mainProcessor.Initialize();
    }
    public override async UniTask Setup() {
        await base.Setup();
        _mainProcessor.Setup();
    }
    public override async UniTask Execute() {
        eEndReason endReason = await _mainProcessor.Execute();
        switch (endReason) {
            case eEndReason.Dead:
                UniTask task = PartManager.instance.TransitionPart(eGamePart.GameOver);
                break;
            case eEndReason.Clear:
                task = PartManager.instance.TransitionPart(eGamePart.GameClear);
                break;
            case eEndReason.Return:
                task = PartManager.instance.TransitionPart(eGamePart.Title);
                break;
        }
    }
    public override async UniTask Teardown() {
        await base.Teardown();
        _mainProcessor.Teardown();
        _characterManager.Teardown();
    }
}

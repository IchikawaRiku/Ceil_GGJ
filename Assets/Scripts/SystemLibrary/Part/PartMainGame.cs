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
    private AcceptMenu _acceptMenu = null;
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuInGameMenu>("Prefab/Menu/CanvasInGameMenu").Initialize();
        _acceptMenu = new AcceptMenu();
    }

    public override async UniTask Execute() {
        await _characterManager.Initialize();
        await _stageManager.Initialize();
        await _stageManager.Setup();
        await _acceptMenu.AcceptInput();
    }
}

/*
 *  @file   AcceptOpenMenuInput.cs
 *  @brief  メニューを開く入力受付
 *  @author Seki
 *  @date   2025/7/30
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameProcessor {
    private MyInput _inputAction = null;

    public void Initialize() {
        _inputAction = MyInputManager.inputAction;
    }
    public void Setup() {
        _inputAction.Player.Pause.Enable();
    }
    /// <summary>
    /// メインゲームの実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        // 入力受付
        while (true) {
            if (_inputAction.Player.Pause.WasPressedThisFrame()) {
                await MenuManager.instance.Get<MenuInGameMenu>().Open();
                _inputAction.Player.Pause.Enable();
            }
            UniTask task =  CharacterManager.instance.Execute();

            await UniTask.DelayFrame(1);
        }
    }
    public void Teardown() {
        _inputAction.Disable();
    }
}

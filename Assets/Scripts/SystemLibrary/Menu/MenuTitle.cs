/*
 *  @file   MenuTitle.cs
 *  @brief  タイトルメニュー
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTitle : MenuBase {
    private MyInput _inputAction = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        _inputAction = MyInputManager.inputAction;
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        _inputAction.Player.Enter.Enable();
        while (true) {
            if(_inputAction.Player.Enter.WasPressedThisFrame()) break;

            await UniTask.DelayFrame(1);
        }
        _inputAction.Player.Enter.Disable();
        await FadeManager.instance.FadeOut();
        await Close();
    }
}

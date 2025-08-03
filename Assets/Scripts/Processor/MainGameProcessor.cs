/*
 *  @file   AcceptOpenMenuInput.cs
 *  @brief  ���j���[���J�����͎�t
 *  @author Seki
 *  @date   2025/7/30
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameProcessor {
    private MyInput _inputAction = null;
    public static eEndReason _eEndReason = eEndReason.Invalid;

    public void Initialize() {
        _inputAction = MyInputManager.inputAction;
    }
    public void Setup() {
        _inputAction.Player.Pause.Enable();
        _eEndReason = eEndReason.Invalid;
    }
    /// <summary>
    /// ���C���Q�[���̎��s����
    /// </summary>
    /// <returns></returns>
    public async UniTask<eEndReason> Execute() {
        // ���͎�t
        while (_eEndReason == eEndReason.Invalid) {
            if (_inputAction.Player.Pause.WasPressedThisFrame()) {
                await MenuManager.instance.Get<MenuInGameMenu>().Open();
                _inputAction.Player.Pause.Enable();
            }
            UniTask task = CharacterManager.instance.Execute();

            await UniTask.DelayFrame(1);
        }
        return _eEndReason;
    }
    public void Teardown() {
        _inputAction.Disable();
    }

    public static void EndGameReason(eEndReason endReason) {
        _eEndReason = endReason;
    }

}

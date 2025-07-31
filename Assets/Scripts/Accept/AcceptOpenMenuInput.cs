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

public class AcceptOpenMenuInput {
    /// <summary>
    /// ���j���[�̎�t
    /// </summary>
    /// <returns></returns>
    public async UniTask AcceptInput() {
        // ���͎�t
        while (true) {
            if(Input.GetKeyDown(KeyCode.Escape)) await MenuManager.instance.Get<MenuInGameMenu>().Open();

            await UniTask.DelayFrame(1);
        }
    }
}

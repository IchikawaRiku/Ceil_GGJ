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
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        while (true) {
            if(Input.GetKeyDown(KeyCode.Space)) break;

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
}

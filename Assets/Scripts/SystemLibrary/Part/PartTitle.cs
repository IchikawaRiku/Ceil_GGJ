/*
 *  @file   PartTitle.cs
 *  @brief  �^�C�g���p�[�g
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
        await MenuManager.instance.Get<MenuSetting>("Prefab/Menu/CanvasSettings").Initialize();
    }
    public override async UniTask Execute() {
        // �^�C�g�����j���[�\��
        await MenuManager.instance.Get<MenuTitle>().Open();
        // ���C���p�[�g�J��
        UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        await UniTask.CompletedTask;
    }
}

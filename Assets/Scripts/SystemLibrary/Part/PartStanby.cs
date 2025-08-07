/*
 *  @file   PartStanby.cs
 *  @brief  €”õƒp[ƒg
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartStanby : PartBase {
    public override async UniTask Execute() {
        await FadeManager.instance.FadeOut();
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        await UniTask.CompletedTask;
    }
}

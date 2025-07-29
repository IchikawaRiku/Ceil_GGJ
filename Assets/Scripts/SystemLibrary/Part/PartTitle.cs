using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {
    public override async UniTask Execute() {
        UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        await UniTask.CompletedTask;
    }
}

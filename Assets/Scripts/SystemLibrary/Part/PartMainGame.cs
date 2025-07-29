using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
    public override async UniTask Execute() {
        //UniTask task = PartManager.instance.TransitionPart(eGamePart.Ending);
        await UniTask.CompletedTask;
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGameOver : PartBase {
    public override async UniTask Execute() {
        await UniTask.CompletedTask;
    }
}

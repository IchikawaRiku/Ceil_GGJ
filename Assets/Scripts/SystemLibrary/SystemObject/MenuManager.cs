using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SystemObject {
    public static MenuManager instance { get; private set; } = null;
    //�Ǘ����Ă��郁�j���[���X�g
    //private List<>
    public override async UniTask Initialize() {
        instance = this;

        await UniTask.CompletedTask;
    }
}

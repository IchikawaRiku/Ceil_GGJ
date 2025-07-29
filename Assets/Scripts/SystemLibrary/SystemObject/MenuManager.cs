using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SystemObject {
    public static MenuManager instance { get; private set; } = null;
    //管理しているメニューリスト
    //private List<>
    public override async UniTask Initialize() {
        instance = this;

        await UniTask.CompletedTask;
    }
}

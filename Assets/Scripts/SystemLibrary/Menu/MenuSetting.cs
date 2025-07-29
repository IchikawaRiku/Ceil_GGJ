using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSetting : MenuBase {

    public override async UniTask Initialize() {
        await base.Initialize();
    }

    public override async UniTask Open() {
        await base.Open();
        while (true) { 
            if(Input.GetKeyDown(KeyCode.Escape)) break;

            await UniTask.DelayFrame(1);
        }
        await Close();
    }

    public override async UniTask Close() {
        await base.Close();
    }
}

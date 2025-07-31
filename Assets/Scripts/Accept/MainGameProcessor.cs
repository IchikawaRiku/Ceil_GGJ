using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameProcessor {
    /// <summary>
    /// メインゲームの実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        // 入力受付
        while (true) {
            if(Input.GetKeyDown(KeyCode.Escape)) await MenuManager.instance.Get<MenuInGameMenu>().Open();

            await UniTask.DelayFrame(1);
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameProcessor {
    /// <summary>
    /// ���C���Q�[���̎��s����
    /// </summary>
    /// <returns></returns>
    public async UniTask Execute() {
        // ���͎�t
        while (true) {
            if(Input.GetKeyDown(KeyCode.Escape)) await MenuManager.instance.Get<MenuInGameMenu>().Open();

            await UniTask.DelayFrame(1);
        }
    }
}

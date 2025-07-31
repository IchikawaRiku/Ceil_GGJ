/*
 *  @file   SystemManager.cs
 *  @brief  ゲーム全体で使用する機能の管理
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour {
    [SerializeField]
    private SystemObject[] _systemObjectList = null;
    // 固定フレームレート
    private const int _FPS = 60;

    private void Start() {
        // フレームレート固定
        Application.targetFrameRate = _FPS;
        // システムオブジェクトの生成、初期化
        UniTask task = Initialize();
    }

    private async UniTask Initialize() {
        for (int i = 0, max = _systemObjectList.Length; i < max; i++) {
            if (_systemObjectList[i] == null) continue;
            // システムオブジェクトの生成
            SystemObject createObject = Instantiate(_systemObjectList[i], transform);
            // 初期化
            await createObject.Initialize();
        }
        // パートの遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Stanby);
    }
}

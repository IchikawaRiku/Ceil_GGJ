/*
 *  @file   PartManager.cs
 *  @brief  パート管理
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class PartManager : SystemObject {
    public static PartManager instance { get; private set; } = null;

    [SerializeField]
    private PartBase[] _partOriginList = null;

    private PartBase[] _partList = null;

    private PartBase _currentPart = null;

    public override async UniTask Initialize() {
        instance = this;
        int partMax = (int)eGamePart.Max;
        _partList = new PartBase[partMax];

        List<UniTask> taskList = new List<UniTask>(partMax);
        for (int i = 0; i < partMax; i++) {
            _partList[i] = Instantiate(_partOriginList[i], transform);
            taskList.Add(_partList[i].Initialize());
        }
        await WaitTask(taskList);
    }
    /// <summary>
    /// パートの遷移
    /// </summary>
    /// <param name="nextPart"></param>
    /// <returns></returns>
    public async UniTask TransitionPart(eGamePart nextPart) {
        // 現在のパートの片付け
        if (_currentPart != null) await _currentPart.Teardown();
        // パートの切り替え
        _currentPart = _partList[(int)nextPart];
        await _currentPart.Setup();
        // 次のパートの実行処理
        UniTask task = _currentPart.Execute();
    }
}

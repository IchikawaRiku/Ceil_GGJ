/*
 *  @file   PartManager.cs
 *  @brief  �p�[�g�Ǘ�
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
    /// �p�[�g�̑J��
    /// </summary>
    /// <param name="nextPart"></param>
    /// <returns></returns>
    public async UniTask TransitionPart(eGamePart nextPart) {
        // ���݂̃p�[�g�̕Еt��
        if (_currentPart != null) await _currentPart.Teardown();
        // �p�[�g�̐؂�ւ�
        _currentPart = _partList[(int)nextPart];
        await _currentPart.Setup();
        // ���̃p�[�g�̎��s����
        UniTask task = _currentPart.Execute();
    }
}

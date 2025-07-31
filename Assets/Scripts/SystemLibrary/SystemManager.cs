/*
 *  @file   SystemManager.cs
 *  @brief  �Q�[���S�̂Ŏg�p����@�\�̊Ǘ�
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
    // �Œ�t���[�����[�g
    private const int _FPS = 60;

    private void Start() {
        // �t���[�����[�g�Œ�
        Application.targetFrameRate = _FPS;
        // �V�X�e���I�u�W�F�N�g�̐����A������
        UniTask task = Initialize();
    }

    private async UniTask Initialize() {
        for (int i = 0, max = _systemObjectList.Length; i < max; i++) {
            if (_systemObjectList[i] == null) continue;
            // �V�X�e���I�u�W�F�N�g�̐���
            SystemObject createObject = Instantiate(_systemObjectList[i], transform);
            // ������
            await createObject.Initialize();
        }
        // �p�[�g�̑J��
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Stanby);
    }
}

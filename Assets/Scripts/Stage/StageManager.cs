using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;
/// <summary>
/// �X�e�[�W�̊Ǘ����s���N���X
/// </summary>
public class StageManager : MonoBehaviour {
    // ���g�ւ̎Q��
    public static StageManager instance { get; private set; } = null;
    [SerializeField]
    private StageBase[] _stageOriginList = null;
    private StageBase[] _stageList = null;
    // ���݂̃X�e�[�W
    private StageBase _currentStage = null;

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public async UniTask Initialize() {
        instance = this;
        int stageMax = (int)eStageStage.Max;
        _stageList = new StageBase[stageMax];

        List<UniTask> taskList = new List<UniTask>(stageMax);
        for (int i = 0; i < stageMax; i++) {
            _stageList[i] = Instantiate(_stageOriginList[i], transform);
            taskList.Add(_stageList[i].Initialize());
        }
        await WaitTask(taskList);
    }
    /// <summary>
    /// �X�e�[�W�̑J��
    /// </summary>
    /// <param name="nextStage"></param>
    /// <returns></returns>
    public async UniTask TransitionStage(eStageStage nextStage) {
        // ���݂̃X�e�[�W�̕Еt��
        if (_currentStage != null) await _currentStage.Teardown();
        // �X�e�[�W�̐؂�ւ�
        _currentStage = _stageList[(int)nextStage];
        await _currentStage.SetUp();
        // ���̃X�e�[�W�̎��s����
        UniTask task = _currentStage.Execute();
    }

}

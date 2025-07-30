using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : StageBase {

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public override async UniTask Initialize() {
        await base.Initialize();
        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].Initialize();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public override async UniTask SetUp() {
        await base.SetUp();

        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].SetUp();
        }

    }

    /// <summary>
    /// �Еt������
    /// </summary>
    /// <returns></returns>
    public override async UniTask Teardown() {
        await base.Teardown();
        for (int i = 0, max = _gimmickBases.Length; i < max; i++) {
            if (_gimmickBases[i] == null) continue;
            _gimmickBases[i].Teardown();
        }
    }

    /// <summary>
    /// ���s����
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override async UniTask Execute() {
        await UniTask.CompletedTask;
    }


}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageBase : MonoBehaviour {

    [SerializeField]
    protected GimmickBase[] _gimmickBases = null;

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }

    public virtual async UniTask SetUp() {
        gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ���s����
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Execute();

    /// <summary>
    /// �Еt������
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Teardown() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}

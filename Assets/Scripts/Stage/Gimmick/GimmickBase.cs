using UnityEngine;

/// <summary>
/// �X�e�[�W�M�~�b�N�̊��N���X
/// </summary>
public abstract class GimmickBase : MonoBehaviour {
    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// 
    /// </summary>
    public virtual void SetUp() { }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected virtual void Update() {
        OnUpdate();
    }

    /// <summary>
    /// �Еt������
    /// </summary>
    public virtual void Teardown() { }

    /// <summary>
    /// �h����ł̍X�V����
    /// </summary>
    protected abstract void OnUpdate();
}
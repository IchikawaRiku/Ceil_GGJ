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
    /// �X�V����
    /// </summary>
    protected virtual void Update() {
        OnUpdate();
    }

    /// <summary>
    /// �h����ł̍X�V����
    /// </summary>
    protected abstract void OnUpdate();
}
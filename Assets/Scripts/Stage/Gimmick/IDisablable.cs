
/// <summary>
/// ��~�\�ȃM�~�b�N�ɋ��ʂ���C���^�[�t�F�[�X
/// </summary>
public interface IDisablable {
    
    // �M�~�b�N���~����
    void Disable();
}


/// <summary>
/// �j��\�ȃM�~�b�N�ɋ��ʂ���C���^�[�t�F�[�X
/// </summary>
public interface IDestroyable {
    // �j�󂷂�
    void DestroyGimmick();
}

/// <summary>
/// ����Ԃ�؂�ւ��\�ȃM�~�b�N�ɋ��ʂ���C���^�[�t�F�[�X
/// </summary>
public interface IVisibleToggleable {
    // �݂��Ⴄ
    void ToggleVisibility();
}
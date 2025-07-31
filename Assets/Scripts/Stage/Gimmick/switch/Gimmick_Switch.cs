using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�C�b�`�M�~�b�N�F�������Ƌ߂��̃M�~�b�N���~����
/// </summary>
public class Gimmick_Switch : GimmickBase {
    [SerializeField] private float disableRadius; // ��~�͈͂̔��a
    private bool _isPressed = false;                    // �����ꂽ���ǂ����̃t���O

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        base.Initialize();
        _isPressed = false;
    }

    /// <summary>
    /// ���񏉊���
    /// </summary>
    public override void SetUp() {
        // �����ꂽ���ǂ���
        _isPressed = false;
    }
    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // ���Ɏg�p���Ȃ�
    }

    /// <summary>
    /// �O������X�C�b�`�������ꂽ���Ƃ�ʒm����
    /// </summary>
    public void Press() {
        // ������Ă��邩
        if (_isPressed) return;
        // ����������ɂ���
        _isPressed = true;

        // �͈͓��̑SCollider���擾
        Collider[] hits = Physics.OverlapSphere(transform.position, disableRadius);

        // �Ԃ�܂�`��
        foreach (var hit in hits) {
            // ��~�Ώ�
            if (hit.TryGetComponent(out IDisablable disablable)) {
                disablable.Disable();
            }

            // �j��Ώ�
            if (hit.TryGetComponent(out IDestroyable destroyable)) {
                destroyable.DestroyGimmick();
            }

            // ����Ԑ֑ؑΏ�
            if (hit.TryGetComponent(out IVisibleToggleable toggleable)) {
                toggleable.ToggleVisibility();
            }
        }

    }

    /// <summary>
    /// �M�Y���Œ�~�͈͂������iScene�r���[�p�j
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, disableRadius);
    }


    /// <summary>
    /// �v���C���[���G���A���ɂ���Ƃ�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("ghost")) {
            // �X�C�b�`���������Ώۂɓo�^
            SwitchUtility.Register(this);
        }
    }

    /// <summary>
    /// �v���C���[�����ꂽ�Ƃ�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("ghost")) {
            // �v���C���[�����ꂽ�����
            SwitchUtility.Clear();
        }
    }
}
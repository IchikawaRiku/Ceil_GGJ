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
        // �f�o�b�O�p�̎���
        if (Input.GetKeyDown(KeyCode.Y)) {
            Press();
        }
    }

    /// <summary>
    /// �O������X�C�b�`�������ꂽ���Ƃ�ʒm����
    /// </summary>
    public void Press() {
        // ���łɉ�����Ă��邩
        if (_isPressed) return;
        _isPressed = true;

        // �͈͓��̑SCollider���擾
        Collider[] hits = Physics.OverlapSphere(transform.position, disableRadius);

        foreach (var hit in hits) {
            // IDisablable ���������Ă���M�~�b�N������
            IDisablable target = hit.GetComponent<IDisablable>();
            if (target != null) {
                target.Disable(); // ��~�������Ăяo��
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
        if (other.CompareTag("Player")) {
            // �X�C�b�`���������Ώۂɓo�^
            SwitchUtility.Register(this);
        }
    }

    /// <summary>
    /// �v���C���[�����ꂽ�Ƃ�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // �v���C���[�����ꂽ�����
            SwitchUtility.Clear();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�C�b�`�M�~�b�N�F�������Ƌ߂��̃M�~�b�N���~����
/// </summary>
public class Gimmick_Switch : GimmickBase {
    [SerializeField] private float disableRadius = 10f; // ��~�͈͂̔��a
    private bool _isPressed = false;                    // �����ꂽ���ǂ����̃t���O

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        base.Initialize();
        _isPressed = false;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
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
}
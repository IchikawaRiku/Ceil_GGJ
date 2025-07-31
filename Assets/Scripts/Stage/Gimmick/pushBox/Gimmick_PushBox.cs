using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;

    // �����Ă���t���O
    private bool isPushing = false;

    // �v���C���[��Transform�Q��
    private Transform playerTransform;

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        if (isPushing && playerTransform != null) {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // ����������X���������߂̎������ړ��iX�����ɂ����������Ȃ������j
            if (Mathf.Abs(direction.x) > 0.7f) {
                float pushDir = Mathf.Sign(direction.x);
                transform.position += new Vector3(pushDir * pushSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    /// <summary>
    /// �v���C���[�����������Ă����
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        if (!collision.collider.CompareTag("Player")) return;

        isPushing = true;
        playerTransform = collision.transform;
    }

    /// <summary>
    /// �v���C���[�Ɨ��ꂽ�Ƃ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            isPushing = false;
            playerTransform = null;
        }
    }
}

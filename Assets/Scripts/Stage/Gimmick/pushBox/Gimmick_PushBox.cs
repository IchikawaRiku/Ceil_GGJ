using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;

    // �����Ă���t���O
    private bool isPushing = false;

    // �v���C���[��Transform
    private Transform playerTransform;

    // �����ʒu�ۑ��p
    private Vector3 boxPosition;

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        // ���������ʒu���L��
        boxPosition = transform.position;
    }

    /// <summary>
    /// �g�p�O����
    /// </summary>
    public override void SetUp() {
        // �����ʒu�A�e�t���O��������
        transform.position = boxPosition;
        isPushing = false;
        playerTransform = null;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        if (isPushing && playerTransform != null) {
            // �v���C���[�Ɣ��̋����𐳋K�����đ��
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

    /// <summary>
    /// ���̈ʒu��������
    /// </summary>
    public void ResetBox() {
        transform.position = boxPosition;
        isPushing = false;
        playerTransform = null;

        // �K�v�Ȃ畨�����������Z�b�g�iRigidbody�t���̏ꍇ�j
        var rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

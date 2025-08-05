using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick_PushBox : GimmickBase {
    [SerializeField]
    private float pushSpeed = 2f;  // ���������X�s�[�h

    [SerializeField]
    private float groundCheckDistance = 0.1f;  // �n�ʃ`�F�b�N�p��Raycast����

    private bool isPushing = false;  // �v���C���[�������Ă��邩
    private Transform playerTransform;  // �v���C���[��Transform�Q��
    private Vector3 boxPosition;  // �����ʒu�ۑ��p
    private Rigidbody rb;  // Rigidbody�L���b�V��
    private Vector3 lastPlayerPos;  // �O�t���[���̃v���C���[�ʒu

    private bool isGrounded = true;  // �n�ʂɐڂ��Ă��邩�ǂ���

    // ����������
    public override void Initialize() {
        boxPosition = transform.position;  // �����ʒu��ۑ�
        rb = GetComponent<Rigidbody>();    // Rigidbody�擾
        if (rb == null) {
        }
    }

    // ��������
    public override void SetUp() {
        transform.position = boxPosition;  // ���������ʒu�ɖ߂�
        isPushing = false;
        playerTransform = null;
        lastPlayerPos = Vector3.zero;

        // Rigidbody�̑��x�����Z�b�g
        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // ���͎g���Ă��Ȃ�
    protected override void OnUpdate() {
    }

    // ���������p�i�Œ莞�Ԃ��ƂɌĂ΂��j
    private void FixedUpdate() {
        CheckGround();  // �n�ʔ���𖈉�s��

        if (!isGrounded) {
            // �n�ʂ��Ȃ���Η����ɔC����iRigidbody�̏d�͂Ŏ��R�����j
            return;
        }

        // �v���C���[�������Ă��āA�Q�Ƃ��L���ŁARigidbody������ꍇ
        if (isPushing && playerTransform != null && rb != null) {
            // �O�t���[���̃v���C���[�ʒu�����������Ȃ珉�����������Ĕ�����
            if (lastPlayerPos == Vector3.zero) {
                lastPlayerPos = playerTransform.position;
                return;
            }

            // �v���C���[�̈ړ��ʂ��擾
            Vector3 playerDelta = playerTransform.position - lastPlayerPos;
            float moveX = playerDelta.x * pushSpeed;  // ���������Ɨ�

            // ���̐V�����ʒu���v�Z
            Vector3 nextPos = rb.position + new Vector3(moveX, 0, 0);

            // Rigidbody��MovePosition�ňړ�
            rb.MovePosition(nextPos);

            // �v���C���[�ʒu���X�V
            lastPlayerPos = playerTransform.position;
        }
        else {
            // �����Ă��Ȃ��Ƃ��͏ꏊ�����Z�b�g
            lastPlayerPos = Vector3.zero;
        }
    }

    /// <summary>
    /// �n�ʂɐڒn���Ă��邩�ǂ����ARayCast�Œ��ׂ�
    /// </summary>
    private void CheckGround() {
        Vector3 origin = transform.position + Vector3.up * 0.1f;  // �����ォ��Ray���΂�
        Ray ray = new Ray(origin, Vector3.down);

        // Raycast�ŉ������ɃR���C�_�[�����邩����
        isGrounded = Physics.Raycast(ray, groundCheckDistance + 0.1f);

    }

    /// <summary>
    /// �v���C���[�����Ɛڒn���Ă�����
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) {
        if (!collision.collider.CompareTag("Player")) return;
        // �����Ă����Ԃɂ���
        isPushing = true;
        playerTransform = collision.transform;
    }

    /// <summary>
    /// �v���C���[�������痣�ꂽ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            isPushing = false;
            playerTransform = null;
        }
    }

    // �������Z�b�g����
    public void ResetBox() {
        // �����ʒu�ɖ߂�
        transform.position = boxPosition;
        // �t���O��������
        isPushing = false;
        playerTransform = null;
        // �v���C���[�̍Ō�̏ꏊ��������
        lastPlayerPos = Vector3.zero;

        if (rb != null) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
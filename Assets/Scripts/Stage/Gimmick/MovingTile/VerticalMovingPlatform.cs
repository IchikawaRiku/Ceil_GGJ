using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask ���g�p���邽��

/// <summary>
/// �㉺�Ɉړ����鏰
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // �����ړ����鋗��
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // �ړ����x
    [SerializeField] private float waitTime = 2f;              // �܂�Ԃ��n�_�̑ҋ@����

    private Vector3 _startPosition;     // ���̏����ʒu
    private bool _movingUp = true;      // ���݂̈ړ�����
    private bool _isWaiting = false;    // �ҋ@�����ǂ���

    [SerializeField] private Rigidbody rigidBody = null; // �����g�� Rigidbody

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        // �����ʒu���w��
        _startPosition = transform.position;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition; // ���������ʒu�ɖ߂�
        _movingUp = true;                    // ������Ԃ͏�����ֈړ�
        _isWaiting = false;                  // �ҋ@�t���O�����Z�b�g
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // �ړ�
        if (!_isWaiting) MovePlatform();
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void MovePlatform() {
        float currentY = transform.position.y; // ���݂̍���
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y; // �㉺�̏���l
        float stepY = moveSpeed.y * Time.deltaTime * (_movingUp ? 1f : -1f); // 1�t���[���ňړ������

        float nextY = currentY + stepY; // ���̍������v�Z
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z)); // �����ړ�

        // �܂�Ԃ�����
        if (Mathf.Abs(targetY - nextY) > Mathf.Abs(targetY - currentY)) {
            // �ҋ@���Ă���܂�Ԃ�
            WaitAndTurnAsync(targetY).Forget();
        }
    }

    /// <summary>
    /// �܂�Ԃ�����
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetY) {
        if (_isWaiting) return;
        // �ҋ@���ɂ���
        _isWaiting = true;

        // ����ڕW�ʒu�Œ�~
        rigidBody.MovePosition(new Vector3(transform.position.x, targetY, transform.position.z));
        //rigidBody.velocity = Vector3.zero;

        // �ҋ@
        await UniTask.Delay((int)(waitTime * 1000));
        // �ړ������𔽓]
        _movingUp = !_movingUp;
        // �ҋ@�I��
        _isWaiting = false;
    }

    /// <summary>
    /// �q�I�u�W�F�N�g�Ɏw��
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // Rigidbody �������Ă���I�u�W�F�N�g�����Ώۂɂ���
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// �q�I�u�W�F�N�g����O��
    /// </summary>
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(null);
        }
    }

    /// <summary>
    /// �Еt������
    /// </summary>
    public override void Teardown() {
    
    }
}
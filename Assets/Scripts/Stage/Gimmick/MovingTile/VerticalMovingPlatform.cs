using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTask ���g�p���邽��

/// <summary>
/// ������Ɉړ����A�����ʒu�ɖ߂�㉺�ړ����i�[�őҋ@���Ă���܂�Ԃ��j
/// �q�I�u�W�F�N�g�������ŏ�ɏ�����I�u�W�F�N�g���ꏏ�ɓ�����
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;   // �����ړ����鋗���i�����ʒu����̍����j
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // �ړ����x�iY���̂ݎg�p�j
    [SerializeField] private float waitTime = 2f;       // �܂�Ԃ��n�_�ł̑ҋ@���ԁi�b�j

    private Vector3 _startPosition;     // ���̏����ʒu
    private bool _movingUp = true;      // ���݂̈ړ������itrue=�㏸, false=���~�j
    private bool _isWaiting = false;    // ���ݑҋ@�����ǂ���

    [SerializeField] private Rigidbody rigidBody = null; // �����g�� Rigidbody

    /// <summary>
    /// �����������i�Q�[���J�n���ȂǂɌĂ΂��j
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position; // �����ʒu���L�^
    }

    /// <summary>
    /// ���Z�b�g�����i�ăX�^�[�g���ȂǂɌĂ΂��j
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition; // ���������ʒu�ɖ߂�
        _movingUp = true;                    // ������Ԃ͏�����ֈړ�
        _isWaiting = false;                  // �ҋ@�t���O�����Z�b�g
    }

    /// <summary>
    /// ���t���[���Ă΂�鏈��
    /// </summary>
    protected override void OnUpdate() {
        if (!_isWaiting) { // �ҋ@���łȂ���Έړ����������s
            MovePlatform();
        }
    }

    /// <summary>
    /// �����㉺�ɓ���������
    /// </summary>
    private void MovePlatform() {
        float currentY = transform.position.y; // ���݂̍���
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y; // �ڕW�����i��[ or ���[�j
        float stepY = moveSpeed.y * Time.deltaTime * (_movingUp ? 1f : -1f); // 1�t���[���ňړ������

        float nextY = currentY + stepY; // ���̍������v�Z
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z)); // �����ړ�

        // �܂�Ԃ�����i�ڕW�n�_��ʂ�߂�����ҋ@�����ցj
        if (Mathf.Abs(targetY - nextY) > Mathf.Abs(targetY - currentY)) {
            WaitAndTurnAsync(targetY).Forget(); // UniTask�őҋ@������񓯊����s
        }
    }

    /// <summary>
    /// �܂�Ԃ��n�_�ň�莞�ԑ҂��Ă�������𔽓]���鏈��
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetY) {
        if (_isWaiting) return; // ���łɑҋ@���Ȃ牽�����Ȃ�

        _isWaiting = true; // �ҋ@�t���O���I��

        // ����ڕW�ʒu�Ƀs�b�^���␳���Ď~�߂�
        rigidBody.MovePosition(new Vector3(transform.position.x, targetY, transform.position.z));
        rigidBody.velocity = Vector3.zero; // �O�̂��ߑ��x�����Z�b�g

        // �w�莞�ԑҋ@�i�V�[���I�����Ɏ����L�����Z�������j
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTime),
                            cancellationToken: this.GetCancellationTokenOnDestroy());

        // �ړ������𔽓]
        _movingUp = !_movingUp;
        _isWaiting = false; // �ҋ@�I��
    }

    /// <summary>
    /// ���ɃI�u�W�F�N�g��������Ƃ��̏���
    /// �� �q�I�u�W�F�N�g�����Ĉꏏ�ɓ�����
    /// </summary>
    private void OnTriggerEnter(Collider other) {
        // Rigidbody �������Ă���I�u�W�F�N�g�����Ώۂɂ���
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(transform); // ���̎q�ɂ��邱�Ƃňꏏ�ɓ���
        }
    }

    /// <summary>
    /// ������I�u�W�F�N�g�����ꂽ�Ƃ��̏���
    /// �� �q�I�u�W�F�N�g�֌W������
    /// </summary>
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null) {
            other.transform.SetParent(null); // �e�q�֌W������
        }
    }

    /// <summary>
    /// �I�������i���Z�b�g����V�[���I�����ɌĂ΂��j
    /// </summary>
    public override void Teardown() {
        // ���Ɏq�I�u�W�F�N�g���������͕s�v�iOnTriggerExit �ŊO��邽�߁j
    }
}
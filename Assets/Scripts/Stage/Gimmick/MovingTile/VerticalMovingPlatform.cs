using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


/// <summary>
/// �㉺�Ɉړ����鏰
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // �����ړ����鋗��
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // ���̈ړ����x
    [SerializeField] private float waitTime = 2f;              // �܂�Ԃ��n�_�ł̑ҋ@����
    [SerializeField] private LayerMask attachableLayers;       // ���ɏ��郌�C���[
    [SerializeField] private Rigidbody rigidBody = null;       // �����g�� Rigidbody

    private Vector3 _startPosition;     // ���̏����ʒu
    private bool _movingUp = true;      // ���݂̈ړ������i��������ǂ����j
    private bool _isWaiting = false;    // �ҋ@�����ǂ���
    private Vector3 _prevPos;           // �O�t���[���̏��̈ʒu
    private Vector3 _velocity;          // ���̈ړ��ʁi�v���C���[�␳�p�j

    // ����������
    public override void Initialize() {
        _startPosition = transform.position;
    }

    // ��������
    public override void SetUp() {
        transform.position = _startPosition;
        _movingUp = true;
        _isWaiting = false;
        _prevPos = transform.position;
        _velocity = Vector3.zero;
    }

    protected override void OnUpdate() {
    }

    // �X�V����
    private void FixedUpdate() {
        Vector3 prevPosition = transform.position;

        if (!_isWaiting) {
            MovePlatform(); // �����ړ�������
        }

        // ���̈ړ��ʂ��v�Z
        Vector3 rawVelocity = transform.position - prevPosition;

        // ������̈ړ��ł̓v���C���[�������グ�Ȃ�
        // �������ł͂��̂܂ܒǏ]������
        if (_movingUp) {
            _velocity = new Vector3(rawVelocity.x, 0f, rawVelocity.z);
        }
        else {
            _velocity = rawVelocity;
        }

        _prevPos = transform.position;
    }

    // ���̈ړ�����
    private void MovePlatform() {
        float currentY = transform.position.y;
        float targetY = _movingUp ? _startPosition.y + moveDistance : _startPosition.y;

        // MoveTowards �Ŋ��炩�Ɉړ�
        float nextY = Mathf.MoveTowards(currentY, targetY, moveSpeed.y * Time.deltaTime);
        rigidBody.MovePosition(new Vector3(transform.position.x, nextY, transform.position.z));

        // �ڕW�ʒu�ɓ��B������ҋ@���ĕ�����؂�ւ���
        if (Mathf.Approximately(nextY, targetY)) {
            WaitAndTurnAsync().Forget();
        }
    }

    // �܂�Ԃ��n�_�ł̑ҋ@����
    private async UniTaskVoid WaitAndTurnAsync() {
        if (_isWaiting) return;

        _isWaiting = true;
        await UniTask.Delay((int)(waitTime * 1000)); // �ҋ@����

        _movingUp = !_movingUp; // �ړ������𔽓]
        _isWaiting = false;
    }

    // ���̏�ɂ���v���C���[���ړ��ʂɍ��킹�ĒǏ]������
    private void OnTriggerStay(Collider other) {
        if (other.attachedRigidbody != null && ((attachableLayers.value & (1 << other.gameObject.layer)) > 0)) {
            Rigidbody rb = other.attachedRigidbody;
            rb.MovePosition(rb.position + _velocity);
        }
    }

    // �����痣�ꂽ�Ƃ��̏���
    private void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody != null && ((attachableLayers.value & (1 << other.gameObject.layer)) > 0)) {
            // other.transform.SetParent(null);
        }
    }

    // ��Еt������
    public override void Teardown() {
    }
}
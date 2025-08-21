using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// ���E�Ɉړ����鏰
/// </summary>
public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;          // �ړ�����
    [SerializeField] private Vector3 moveSpeed = Vector3.zero; // �ړ����x
    [SerializeField] private float waitTime = 2f;              // �ҋ@����

    [SerializeField] private LayerMask attachableLayers;       // �q�I�u�W�F�N�g�ɂ���Layer

    [SerializeField] private Rigidbody rigidBody = null;       // ���g��RigitBody
    [SerializeField] private PlatformDetector detector;        // ���㌟�m�p

    private Vector3 _startPosition;                            // �����ʒu
    private bool _movingRight = true;                          // �ړ�����
    private bool _isWaiting = false;                           // �ҋ@�����ǂ���

    private HashSet<Transform> childrenOnPlatform = new();     // ���ɏ���Ă���I�u�W�F�N�g���Ǘ�

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        // �����ʒu��ۑ�
        _startPosition = transform.position;

        // Detector�̃C�x���g����
        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public override void SetUp() {
        // ���������ʒu�ɖ߂�
        transform.position = _startPosition;

        // �ړ���ԃ��Z�b�g
        _movingRight = true;
        _isWaiting = false;

        // Detector�̃C�x���g����
        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // �ҋ@���łȂ���Έړ�����
        if (!_isWaiting) MovePlatform();
    }

    /// <summary>
    /// ���̈ړ�����
    /// </summary>
    private void MovePlatform() {
        // �ړ�����w��
        float targetX = _movingRight ? _startPosition.x + moveDistance : _startPosition.x;
        float direction = _movingRight ? 1f : -1f;

        // �ړ�
        rigidBody.velocity = new Vector3(moveSpeed.x * direction, 0f, 0f);

        // �܂�Ԃ�����
        if ((_movingRight && transform.position.x >= targetX) ||
            (!_movingRight && transform.position.x <= targetX)) {
            // �܂�Ԃ����������s
            WaitAndTurnAsync(targetX).Forget();
        }
    }

    /// <summary>
    /// �܂�Ԃ����̑ҋ@����
    /// </summary>
    private async UniTaskVoid WaitAndTurnAsync(float targetX) {
        // ���łɑҋ@���Ȃ珈�����Ȃ�
        if (_isWaiting) return;
        _isWaiting = true;

        // �ړ����~���Ĉʒu��␳
        rigidBody.velocity = Vector3.zero;
        rigidBody.MovePosition(new Vector3(targetX, transform.position.y, transform.position.z));

        // �w�莞�ԑҋ@
        await UniTask.Delay((int)(waitTime * 1000));

        // �ړ������𔽓]
        _movingRight = !_movingRight;

        // �ҋ@�I��
        _isWaiting = false;
    }

    /// <summary>
    /// ���ɃI�u�W�F�N�g���悹�鏈��
    /// </summary>
    private void AttachToPlatform(Rigidbody rb) {
        if (rb != null) {
            // �w�肵�� Layer �̂ݏ���
            if ((attachableLayers.value & (1 << rb.gameObject.layer)) != 0) {
                rb.transform.SetParent(transform, true);
                childrenOnPlatform.Add(rb.transform);    // �Ǘ����X�g�ɒǉ�
            }
        }
    }

    /// <summary>
    /// ������I�u�W�F�N�g���O������
    /// </summary>
    private void DetachFromPlatform(Rigidbody rb) {
        if (rb != null && childrenOnPlatform.Contains(rb.transform)) {
            rb.transform.SetParent(null, true);         // �q�I�u�W�F�N�g����
            childrenOnPlatform.Remove(rb.transform);    // �Ǘ����X�g����폜
        }
    }

    /// <summary>
    /// �R���C�_�[�N�����̏���
    /// </summary>
    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        AttachToPlatform(rb);
    }

    /// <summary>
    /// �R���C�_�[�ޏo���̏���
    /// </summary>
    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        DetachFromPlatform(rb);
    }

    /// <summary>
    /// �I�����̕Еt������
    /// </summary>
    public override void Teardown() {
        // ���ɏ���Ă����I�u�W�F�N�g�����ׂĉ���
        foreach (var child in childrenOnPlatform) {
            if (child != null) child.SetParent(null, true);
        }
        childrenOnPlatform.Clear();
    }
}
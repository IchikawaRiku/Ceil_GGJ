using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���E�����Ɉړ����鏰
/// </summary>
public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;               // �E�����Ɉړ����鋗��
    [SerializeField] private Vector3 moveSpeed = Vector3.zero;      // �ړ����x (X���̂ݎg�p)

    private Vector3 _startPosition;                                 // �����ʒu
    private bool _movingRight = true;                               // ���݂̈ړ�����
    [SerializeField] private Rigidbody rigidBody = null;            // ���̏��� Rigidbody
    private List<Rigidbody> rigidBodys = new();                     // ��ɏ�����I�u�W�F�N�g�� Rigidbody
    [SerializeField] private PlatformDetector detector;             // �q�I�u�W�F�N�g�̃X�N���v�g��Inspector�ŃZ�b�g

    /// <summary>
    /// ����������
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;

        if (detector != null) {
            detector.OnDetectedEnter += rb => {
                if (!rigidBodys.Contains(rb)) rigidBodys.Add(rb);
            };
            detector.OnDetectedExit += rb => {
                rigidBodys.Remove(rb);
            };
        }
    }

    /// <summary>
    /// �Z�b�g�A�b�v����
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition;
        _movingRight = true;
   
        if (detector != null) {
            detector.OnDetectedEnter += rb => {
                if (!rigidBodys.Contains(rb)) rigidBodys.Add(rb);
            };
            detector.OnDetectedExit += rb => {
                rigidBodys.Remove(rb);
            };
        }
    }

    /// <summary>
    /// ���t���[���X�V����
    /// </summary>
    protected override void OnUpdate() {
        MovePlatform();
    }

    private void FixedUpdate() {
        AddVelocity();

    }

    /// <summary>
    /// �v���b�g�t�H�[�������E�ɓ���������
    /// </summary>
    private void MovePlatform() {
        float currentX = transform.position.x;
        float targetX = _movingRight ? _startPosition.x + moveDistance : _startPosition.x;
        float stepX = moveSpeed.x * Time.deltaTime * (_movingRight ? 1f : -1f);

        float nextX = currentX + stepX;
        rigidBody.MovePosition(new Vector3(nextX, transform.position.y, transform.position.z));

        // �܂�Ԃ�����i�ڕW�n�_��ʂ�߂���������𔽓]�j
        if (Mathf.Abs(targetX - nextX) > Mathf.Abs(targetX - currentX)) {
            _movingRight = !_movingRight;
        }
    }

    /// <summary>
    /// �g���K�[�ɓ������I�u�W�F�N�g��Rigidbody��o�^
    /// </summary>
    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null && !rigidBodys.Contains(rb)) {
            rigidBodys.Add(rb);
        }
    }

    /// <summary>
    /// �g���K�[����o���I�u�W�F�N�g��Rigidbody���폜
    /// </summary>
    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            rigidBodys.Remove(rb);
        }
    }

    /// <summary>
    /// �ړ����̑��x����ɏ���Ă���I�u�W�F�N�g�ɔ��f
    /// </summary>
    void AddVelocity() {
        if (rigidBody == null || rigidBody.velocity.sqrMagnitude <= 0.001f) return;

        Vector3 platformVelocity = rigidBody.velocity;

        foreach (var rb in rigidBodys) {
            if (rb != null) {
                Vector3 nextPos = rb.position + platformVelocity * Time.fixedDeltaTime;
                rb.MovePosition(nextPos);
            }
        }
    }

    /// <summary>
    /// �Еt������
    /// </summary>
    public override void Teardown() {
        // ���X�g����폜
        rigidBodys.Clear();


    }
}
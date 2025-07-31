using UnityEngine;

/// <summary>
/// �E�����Ɉړ����A�����ʒu�ɖ߂鍶�E�ړ���
/// </summary>
public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;   // �E�����Ɉړ����鋗��
    [SerializeField] private float moveSpeed = 1f;      // �ړ����x

    private Vector3 _startPosition;                     // ���������ʒu��ۑ�
    private bool _movingRight = true;                   // �E�����Ɉړ������ǂ���

    /// <summary>
    /// �����������i���������ʒu���L�^�j
    /// </summary>
    public override void Initialize() {
        _startPosition = transform.position;
    }

    /// <summary>
    /// ���������i�ʒu�Ə�Ԃ��������j
    /// </summary>
    public override void SetUp() {
        transform.position = _startPosition;
        _movingRight = true;
    }

    /// <summary>
    /// ���t���[���̍X�V����
    /// </summary>
    protected override void OnUpdate() {
        float step = moveSpeed * Time.deltaTime;
        Vector3 target;

        // �E�Ɉړ������ǂ����ŖڕW�n�_��ݒ�
        if (_movingRight) {
            target = _startPosition + Vector3.right * moveDistance;
        }
        else {
            target = _startPosition;
        }

        // ���݈ʒu��ڕW�n�_�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // �ڕW�n�_�ɓ��B����������𔽓]
        if (Vector3.Distance(transform.position, target) < 0.01f) {
            _movingRight = !_movingRight;
        }
    }
}
using UnityEngine;

/// <summary>
/// ������Ɉړ����A�����ʒu�ɖ߂�㉺�ړ���
/// </summary>
public class VerticalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;   // ������Ɉړ����鋗��
    [SerializeField] private float moveSpeed = 1f;      // �ړ����x

    private Vector3 _startPosition;                     // ���������ʒu��ۑ�
    private bool _movingUp = true;                      // ������Ɉړ������ǂ���

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
        _movingUp = true;
    }

    /// <summary>
    /// ���t���[���̍X�V����
    /// </summary>
    protected override void OnUpdate() {
        float step = moveSpeed * Time.deltaTime;
        Vector3 target;

        // �㏸�������~�����ŖڕW�n�_��ݒ�
        if (_movingUp) {
            target = _startPosition + Vector3.up * moveDistance;
        }
        else {
            target = _startPosition;
        }

        // ���݈ʒu��ڕW�n�_�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // �ڕW�n�_�ɓ��B����������𔽓]
        if (Vector3.Distance(transform.position, target) < 0.01f) {
            _movingUp = !_movingUp;
        }
    }
}
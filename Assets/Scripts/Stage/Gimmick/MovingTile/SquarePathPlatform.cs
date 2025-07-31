using UnityEngine;

/// <summary>
/// �l�p�`�̃p�X���ړ����鏰
/// </summary>
public class SquarePathPlatform : GimmickBase {
    [SerializeField] private float sideLength = 2f;     // �e�ӂ̒���
    [SerializeField] private float moveSpeed = 1f;      // �ړ����x

    private Vector3[] _waypoints;                       // �l�p�`�̒��_
    private int _currentIndex = 0;                      // ���݂̈ړ���C���f�b�N�X
    private Vector3 _startPosition;                     // �����ʒu

    /// <summary>
    /// ������
    /// </summary>
    public override void Initialize() {
        // ���������ʒu���L��
        _startPosition = transform.position;

        // �E�F�C�|�C���g���v�Z
        GenerateWaypoints();
    }

    /// <summary>
    /// ����
    /// </summary>
    public override void SetUp() {
        // �����ʒu��������
        transform.position = _startPosition;

        // �E�F�C�|�C���g���Đ���
        GenerateWaypoints();

        // ���݂̈ړ��C���f�b�N�X�����Z�b�g
        _currentIndex = 0;
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    protected override void OnUpdate() {
        // ���݂̖ڕW�n�_���擾
        Vector3 target = _waypoints[_currentIndex];
        float step = moveSpeed * Time.deltaTime;

        // �ړ�
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // ���B�����玟�̃|�C���g��
        if (Vector3.Distance(transform.position, target) < 0.01f) {
            _currentIndex = (_currentIndex + 1) % _waypoints.Length;
        }
    }

    /// <summary>
    /// �l�p�`�̃E�F�C�|�C���g�𐶐�����
    /// </summary>
    private void GenerateWaypoints() {
        _waypoints = new Vector3[4];
        _waypoints[0] = _startPosition;
        _waypoints[1] = _startPosition + Vector3.right * sideLength;
        _waypoints[2] = _waypoints[1] + Vector3.up * sideLength;
        _waypoints[3] = _waypoints[2] - Vector3.right * sideLength;
    }
}
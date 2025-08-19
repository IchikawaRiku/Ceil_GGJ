using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HorizontalMovingPlatform : GimmickBase {
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private Vector3 moveSpeed = Vector3.zero;
    [SerializeField] private float waitTime = 2f;

    private Vector3 _startPosition;
    private bool _movingRight = true;
    private bool _isWaiting = false;

    [SerializeField] private Rigidbody rigidBody = null;
    [SerializeField] private PlatformDetector detector;

    private HashSet<Transform> childrenOnPlatform = new();

    public override void Initialize() {
        _startPosition = transform.position;

        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    public override void SetUp() {
        transform.position = _startPosition;
        _movingRight = true;
        _isWaiting = false;

        if (detector != null) {
            detector.OnDetectedEnter += AttachToPlatform;
            detector.OnDetectedExit += DetachFromPlatform;
        }
    }

    protected override void OnUpdate() {
        if (!_isWaiting) MovePlatform();
    }

    private void FixedUpdate() {
        // Rigidbody ‚ð’¼Ú“®‚©‚·‚Ì‚Í‚»‚Ì‚Ü‚Ü
    }

    private void MovePlatform() {
        float targetX = _movingRight ? _startPosition.x + moveDistance : _startPosition.x;
        float direction = _movingRight ? 1f : -1f;

        rigidBody.velocity = new Vector3(moveSpeed.x * direction, 0f, 0f);

        if ((_movingRight && transform.position.x >= targetX) ||
            (!_movingRight && transform.position.x <= targetX)) {
            WaitAndTurnAsync(targetX).Forget();
        }
    }

    private async UniTaskVoid WaitAndTurnAsync(float targetX) {
        if (_isWaiting) return;
        _isWaiting = true;

        rigidBody.velocity = Vector3.zero;
        rigidBody.MovePosition(new Vector3(targetX, transform.position.y, transform.position.z));

        await UniTask.Delay((int)(waitTime * 1000));

        _movingRight = !_movingRight;
        _isWaiting = false;
    }

    private void AttachToPlatform(Rigidbody rb) {
        if (rb != null) {
            rb.transform.SetParent(transform, true);
            childrenOnPlatform.Add(rb.transform);
        }
    }

    private void DetachFromPlatform(Rigidbody rb) {
        if (rb != null && childrenOnPlatform.Contains(rb.transform)) {
            rb.transform.SetParent(null, true);
            childrenOnPlatform.Remove(rb.transform);
        }
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        AttachToPlatform(rb);
    }

    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        DetachFromPlatform(rb);
    }

    public override void Teardown() {
        foreach (var child in childrenOnPlatform) {
            if (child != null) child.SetParent(null, true);
        }
        childrenOnPlatform.Clear();
    }
}
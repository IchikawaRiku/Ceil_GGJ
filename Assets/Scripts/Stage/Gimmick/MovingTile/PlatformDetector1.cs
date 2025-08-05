using UnityEngine;
using System;

public class PlatformDetector1 : MonoBehaviour {
    public event Action<Rigidbody> OnDetectedEnter;
    public event Action<Rigidbody> OnDetectedExit;

    [SerializeField] private string targetLayerName = "Player";

    void OnTriggerEnter(Collider other) {
        if (IsTarget(other)) {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) {
                OnDetectedEnter?.Invoke(rb);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (IsTarget(other)) {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) {
                OnDetectedExit?.Invoke(rb);
            }
        }
    }

    bool IsTarget(Collider other) {
        return other.gameObject.layer == LayerMask.NameToLayer(targetLayerName);
    }
}
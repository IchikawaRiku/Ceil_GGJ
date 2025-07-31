using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class StageObject : BaseObject {
    public virtual void Initialize() {
        gameObject.SetActive(false);
    }
    public virtual void Setup() {
        gameObject.SetActive(true);
    }
    public virtual void Teardown() {
        gameObject.SetActive(false);
    }
    public override Vector3 GetPosition() {
        return position;
    }

    public override Quaternion GetRotation() {
        return rotation;
    }

    public override void SetPosition(Vector3 _position) {
        position = _position;
        transform.position = position;
    }

    public override void SetRotation(Quaternion _rotation) {
        rotation = _rotation;
        transform.rotation = rotation;
    }
}

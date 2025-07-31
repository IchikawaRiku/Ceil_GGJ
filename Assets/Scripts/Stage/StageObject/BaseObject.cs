using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : MonoBehaviour {
    protected Vector3 position = Vector3.zero;
    protected Quaternion rotation = Quaternion.identity;


    public abstract Vector3 GetPosition();
    public abstract Quaternion GetRotation();

    public abstract void SetPosition(Vector3 _position);
    public abstract void SetRotation(Quaternion _rotation);
}

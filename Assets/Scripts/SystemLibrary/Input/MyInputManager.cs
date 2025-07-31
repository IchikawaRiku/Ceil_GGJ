using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputManager{
    public static MyInput inputAction { get; private set; } = null;
    public static void Initialize() {
        inputAction = new MyInput();
    }
}

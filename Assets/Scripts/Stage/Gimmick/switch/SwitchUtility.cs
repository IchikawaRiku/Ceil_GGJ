using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スイッチを押させる
/// </summary>
public static class SwitchUtility {
    // 今スイッチが押せる状態のスイッチ
    private static Gimmick_Switch _currentSwitch;

    /// <summary>
    /// 押すことができるスイッチを登録
    /// </summary>
    /// <param name="currentSwitch"></param>
    public static void Register(Gimmick_Switch currentSwitch) {
        _currentSwitch = currentSwitch;
    }

    /// <summary>
    /// スイッチの状態を元に戻す
    /// </summary>
    public static void Clear() {
        _currentSwitch = null;
    }

    /// <summary>
    /// スイッチを押す
    /// </summary>
    public static void Press() {
        _currentSwitch?.Press();  // 登録されていれば押す
    }
}

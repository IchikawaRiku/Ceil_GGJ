/*
 *  @file   CommonModule.cs
 *  @brief  汎用処理クラス
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonModule{
    /// <summary>
    /// 配列が空か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(T[] array) {
        return array == null || array.Length <= 0;
    }
    /// <summary>
    /// リストが空か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> list) {
        return list == null || list.Count <= 0;
    }
    /// <summary>
    /// 配列に対して有効なインデクスか判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(T[] array, int index) {
        if(IsEmpty(array)) return false;

        return index >= 0 && array.Length > index;
    }
    /// <summary>
    /// リストに対して有効なインデクスか判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(List<T> list, int index) {
        if (IsEmpty(list)) return false;

        return index >= 0 && list.Count > index;
    }
    /// <summary>
    /// リストの初期化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="capacity"></param>
    public static void InitializeList<T>(List<T> list, int capacity = -1) {
        if(list == null) {
            if(capacity < 1) {
                list = new List<T>();
            } else {
                list = new List<T>(capacity);
            }
        } else {
            if(list.Capacity < capacity) list.Capacity = capacity;

            list.Clear();
        }
    }
    /// <summary>
    /// 複数のタスクの終了待ち
    /// </summary>
    /// <param name="taskList"></param>
    /// <returns></returns>
    public static async UniTask WaitTask(List<UniTask> taskList) {
        //タスクリストが空になるまで待つ
        while(!IsEmpty(taskList)) {
            for (int i = taskList.Count - 1; i >= 0; i--) {
                if (!taskList[i].Status.IsCompleted()) continue;
                //完了したタスクを取り除く
                taskList.RemoveAt(i);
            }
            await UniTask.DelayFrame(1);
        }
    }
}

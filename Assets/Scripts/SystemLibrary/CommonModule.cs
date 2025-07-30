/*
 *  @file   CommonModule.cs
 *  @brief  �ėp�����N���X
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonModule{
    /// <summary>
    /// �z�񂪋󂩔���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(T[] array) {
        return array == null || array.Length <= 0;
    }
    /// <summary>
    /// ���X�g���󂩔���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> list) {
        return list == null || list.Count <= 0;
    }
    /// <summary>
    /// �z��ɑ΂��ėL���ȃC���f�N�X������
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
    /// ���X�g�ɑ΂��ėL���ȃC���f�N�X������
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
    /// ���X�g�̏�����
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
    /// �����̃^�X�N�̏I���҂�
    /// </summary>
    /// <param name="taskList"></param>
    /// <returns></returns>
    public static async UniTask WaitTask(List<UniTask> taskList) {
        //�^�X�N���X�g����ɂȂ�܂ő҂�
        while(!IsEmpty(taskList)) {
            for (int i = taskList.Count - 1; i >= 0; i--) {
                if (!taskList[i].Status.IsCompleted()) continue;
                //���������^�X�N����菜��
                taskList.RemoveAt(i);
            }
            await UniTask.DelayFrame(1);
        }
    }
}

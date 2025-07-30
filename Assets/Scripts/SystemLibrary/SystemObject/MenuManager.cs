/*
 *  @file   MenuManager.cs
 *  @brief  ���j���[�̊Ǘ�
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SystemObject {
    public static MenuManager instance { get; private set; } = null;
    //�Ǘ����Ă��郁�j���[���X�g
    private List<MenuBase> _menuList = null;

    private const int _INIT_MENU_NUM = 256;

    public override async UniTask Initialize() {
        instance = this;
        _menuList = new List<MenuBase>(_INIT_MENU_NUM);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ���j���[�̎擾
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Get<T>(string name = null) where T : MenuBase {
        for (int i = 0, max = _menuList.Count; i < max; i++) {
            T menu = _menuList[i] as T;
            if(menu == null) continue;

            return menu;
        }
        //������Ȃ���ΐ���
        return Load<T>(name);
    }
    /// <summary>
    /// ���j���[�̐���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    private T Load<T>(string name) where T : MenuBase {
        //���j���[�̓ǂݍ���
        T menu = Resources.Load<T>(name);
        if(menu == null) return null;
        //���j���[�̐���
        T createMenu = Instantiate(menu, transform);
        //��\���ɂ��Ă���
        createMenu.gameObject.SetActive(false);
        _menuList.Add(createMenu);
        return createMenu;
    }
}

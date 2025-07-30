/*
 *  @file   MenuManager.cs
 *  @brief  メニューの管理
 *  @author Seki
 *  @date   2025/7/29
 */
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SystemObject {
    public static MenuManager instance { get; private set; } = null;
    //管理しているメニューリスト
    private List<MenuBase> _menuList = null;

    private const int _INIT_MENU_NUM = 256;

    public override async UniTask Initialize() {
        instance = this;
        _menuList = new List<MenuBase>(_INIT_MENU_NUM);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// メニューの取得
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
        //見つからなければ生成
        return Load<T>(name);
    }
    /// <summary>
    /// メニューの生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    private T Load<T>(string name) where T : MenuBase {
        //メニューの読み込み
        T menu = Resources.Load<T>(name);
        if(menu == null) return null;
        //メニューの生成
        T createMenu = Instantiate(menu, transform);
        //非表示にしておく
        createMenu.gameObject.SetActive(false);
        _menuList.Add(createMenu);
        return createMenu;
    }
}

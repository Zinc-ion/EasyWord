﻿using Microsoft.AspNetCore.Components;

namespace EasyWord.Library.Services.Impl;

//导航Service，用于页面跳转 xj实现
public class NavigationService : INavigationService {
    private readonly NavigationManager _navigationManager;
    private readonly IParcelBoxService _parcelBoxService;

    public NavigationService(NavigationManager navigationManager,
        IParcelBoxService parcelBoxService) {
        _navigationManager = navigationManager;
        _parcelBoxService = parcelBoxService;
    }

    public void NavigateTo(string uri) => _navigationManager.NavigateTo(uri);

    public void NavigateTo(string uri, object parameter) {
        var token = _parcelBoxService.Put(parameter);
        _navigationManager.NavigateTo($"{uri}/{token}");
    }
}
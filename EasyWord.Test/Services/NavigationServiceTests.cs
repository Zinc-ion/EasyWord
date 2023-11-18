namespace EasyWord.Test.Services;

// 引入测试框架和需要的命名空间
using Xunit;
using Moq;
using EasyWord.Library.Services.Impl;
using EasyWord.Library.Services;
using Microsoft.AspNetCore.Components;

//测试导航服务 xj实现
public class NavigationServiceTests
{

    internal class TestNav : NavigationManager
    {
        public TestNav()
        {
            Initialize("https://unit-test.example/", "https://unit-test.example/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            NotifyLocationChanged(false);
        }
    }

    [Fact]
    public void NavigateTo_CallsNavigationManagerNavigateTo()
    {
        var parcelBoxServiceMock = new Mock<IParcelBoxService>();
        var navigationManager = new TestNav();

        // 使用实际对象进行测试
        var navigationService = new NavigationService(navigationManager, parcelBoxServiceMock.Object);

        // 执行
        navigationService.NavigateTo("https://unit-test.example/");

        // 断言
        // 验证 NavigationManager 方法是否调用
        Assert.Equal("https://unit-test.example/", navigationManager.Uri); // 根据你的需求进行验证
    }


    [Fact]
    public void NavigateTo_WithParameter_CallsParcelBoxServicePutAndNavigationManagerNavigateTo()
    {
        // 准备测试数据
        var navigationManager = new TestNav();
        var parcelBoxServiceMock = new Mock<IParcelBoxService>();
        var navigationService = new NavigationService(navigationManager, parcelBoxServiceMock.Object);
        var parameter = new object();

        // 执行测试
        navigationService.NavigateTo("https://unit-test.example/", parameter);

        // 验证ParcelBoxService的Put方法是否被调用
        parcelBoxServiceMock.Verify(parcelBoxService => parcelBoxService.Put(parameter), Times.Once);

        // 验证 NavigationManager 方法是否调用
        Assert.Equal("https://unit-test.example/", navigationManager.Uri); // 根据你的需求进行验证
    }
}

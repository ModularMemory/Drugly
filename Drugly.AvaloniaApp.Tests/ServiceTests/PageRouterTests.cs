using Drugly.AvaloniaApp.Services;
using Drugly.AvaloniaApp.Services.Interfaces;
using Drugly.AvaloniaApp.Tests.Models;
using Drugly.AvaloniaApp.ViewModels;
using Serilog.Core;

namespace Drugly.AvaloniaApp.Tests.ServiceTests;

/// <summary>Tests for <see cref="PageRouter"/>.</summary>
public class PageRouterTests
{
    private static IPageRouter PageRouterFactory => new PageRouter(Logger.None);

    [Fact]
    public void RootPageSetter_NotifiesNavigationSubscribers_WhenHistoryEmpty()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        var navigated = false;
        pageRouter.PageNavigate += (s, e) => navigated = true;

        // Act
        pageRouter.RootPage = new DummyViewModel();

        // Assert
        Assert.True(navigated);
    }

    [Fact]
    public void RootPageSetter_DoesNotNotifyNavigationSubscribers_WhenHistoryNotEmpty()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();
        pageRouter.PushPage(new DummyViewModel());

        var navigated = false;
        pageRouter.PageNavigate += (s, e) => navigated = true;

        // Act
        pageRouter.RootPage = new DummyViewModel();

        // Assert
        Assert.False(navigated);
    }

    [Fact]
    public void PushPage_DoesNotCreateHistory_WhenNull()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        // Act
        pageRouter.PushPage(null);

        // Assert
        Assert.True(pageRouter.HistoryEmpty);
    }

    [Fact]
    public void PushPage_CreatesHistory_WhenNotNull()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        // Act
        pageRouter.PushPage(new DummyViewModel());

        // Assert
        Assert.False(pageRouter.HistoryEmpty);
    }

    [Fact]
    public void PopPage_WhenHistoryEmpty_ReturnsRootPage()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        var expected = new DummyViewModel();
        pageRouter.RootPage = expected;

        // Act
        ViewModelBase? actual = null;
        pageRouter.PageNavigate += (s, e) => actual = e;
        pageRouter.PopPage();

        // Assert
        Assert.NotNull(actual);
        Assert.Same(expected, actual);
    }

    [Fact]
    public void PopPage_WhenHistoryOne_ReturnsRootPage()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();
        pageRouter.PushPage(new DummyViewModel());

        var expected = new DummyViewModel();
        pageRouter.RootPage = expected;

        // Act
        ViewModelBase? actual = null;
        pageRouter.PageNavigate += (s, e) => actual = e;
        pageRouter.PopPage();

        // Assert
        Assert.NotNull(actual);
        Assert.Same(expected, actual);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void PopPage_WhenHistoryGreaterThanOne_ReturnsLastPage(int count)
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        ViewModelBase? expected = null;
        ViewModelBase? last = null;
        for (var i = 0; i < count; i++)
        {
            expected = last;
            last = new DummyViewModel();
            pageRouter.PushPage(last);
        }

        // Act
        ViewModelBase? actual = null;
        pageRouter.PageNavigate += (s, e) => actual = e;
        pageRouter.PopPage();

        // Assert
        Assert.NotNull(actual);
        Assert.Same(expected, actual);
    }

    [Fact]
    public void ReshowPage_WhenHistoryEmpty_ReturnsRootPage()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        var expected = new DummyViewModel();
        pageRouter.RootPage = expected;

        // Act
        ViewModelBase? actual = null;
        pageRouter.PageNavigate += (s, e) => actual = e;
        pageRouter.ReshowPage();

        // Assert
        Assert.NotNull(actual);
        Assert.Same(expected, actual);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void ReshowPage_WhenHistoryNotEmpty_ReturnsCurrentPage(int count)
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        ViewModelBase? expected = null;
        for (var i = 0; i < count; i++)
        {
            expected = new DummyViewModel();
            pageRouter.PushPage(expected);
        }

        // Act
        ViewModelBase? actualCur = null;
        ViewModelBase? actualLast = null;
        pageRouter.PageNavigate += (s, e) =>
        {
            actualLast = actualCur;
            actualCur = e;
        };
        pageRouter.ReshowPage();

        // Assert
        Assert.Null(actualLast); // Reshow presents null to refresh the current page
        Assert.NotNull(actualCur);
        Assert.Same(expected, actualCur);
    }

    [Fact]
    public void ResetPageHistory_SetsHistoryEmpty()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        pageRouter.PushPage(new DummyViewModel());

        // Act
        var before = pageRouter.HistoryEmpty;
        pageRouter.ResetPageHistory();
        var after = pageRouter.HistoryEmpty;

        // Assert
        Assert.False(before);
        Assert.True(after);
    }

    [Fact]
    public void ResetPageHistory_DoesNotInvokeNavigation()
    {
        // Arrange
        var pageRouter = PageRouterFactory;
        pageRouter.ResetPageHistory();

        pageRouter.PushPage(new DummyViewModel());

        // Act
        var navigated = false;
        pageRouter.PageNavigate += (s, e) => navigated = true;
        pageRouter.ResetPageHistory();

        // Assert
        Assert.False(navigated);
    }
}
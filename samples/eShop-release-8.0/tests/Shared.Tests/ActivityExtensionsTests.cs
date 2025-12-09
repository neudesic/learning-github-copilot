namespace eShop.Shared.Tests;

using System.Diagnostics;
using Xunit;

public class ActivityExtensionsTests
{
    [Fact]
    public void SetExceptionTags_WithValidActivity_AddsExceptionTags()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new ArgumentException("Test error message");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.NotNull(activity);
        Assert.Equal("Test error message", activity.GetTagItem("exception.message"));
        Assert.Equal(ex.GetType().FullName, activity.GetTagItem("exception.type"));
        Assert.NotNull(activity.GetTagItem("exception.stacktrace"));
        Assert.Equal(ActivityStatusCode.Error, activity.Status);

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithNullActivity_DoesNotThrow()
    {
        // Arrange
        Activity? activity = null;
        var ex = new InvalidOperationException("Test error");

        // Act & Assert - should not throw
#pragma warning disable CS8604
        activity.SetExceptionTags(ex);
#pragma warning restore CS8604
    }

    [Fact]
    public void SetExceptionTags_SetsExceptionMessage()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var messageText = "This is a test exception message";
        var ex = new Exception(messageText);

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(messageText, activity.GetTagItem("exception.message"));

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_SetsExceptionType()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new InvalidOperationException("Test");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(typeof(InvalidOperationException).FullName, activity.GetTagItem("exception.type"));

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_SetsStackTrace()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new Exception("Test");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        var stackTrace = activity.GetTagItem("exception.stacktrace");
        Assert.NotNull(stackTrace);
        Assert.Contains("System.Exception: Test", stackTrace.ToString()!);

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_SetsActivityStatusToError()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new Exception("Test error");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(ActivityStatusCode.Error, activity.Status);

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithInnerException_IncludesInnerExceptionInStackTrace()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var innerEx = new InvalidOperationException("Inner exception");
        var ex = new Exception("Outer exception", innerEx);

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        var stackTrace = activity.GetTagItem("exception.stacktrace")?.ToString();
        Assert.NotNull(stackTrace);
        Assert.Contains("Outer exception", stackTrace);
        Assert.Contains("Inner exception", stackTrace);

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithCustomException_AddsCorrectType()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new CustomTestException("Custom error");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(typeof(CustomTestException).FullName, activity.GetTagItem("exception.type"));

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithEmptyExceptionMessage_AddsEmptyMessageTag()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new Exception("");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal("", activity.GetTagItem("exception.message"));

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithExceptionAndVerifyAllTags_SetsAllTagsCorrectly()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = new ArgumentException("Test exception");

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.NotNull(activity.GetTagItem("exception.message"));
        Assert.NotNull(activity.GetTagItem("exception.type"));
        Assert.NotNull(activity.GetTagItem("exception.stacktrace"));
        Assert.Equal(ActivityStatusCode.Error, activity.Status);

        activity.Stop();
    }

    [Fact]
    public void SetExceptionTags_WithVeryLongExceptionMessage_AddsFullMessage()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var longMessage = new string('x', 1000);
        var ex = new Exception(longMessage);

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(longMessage, activity.GetTagItem("exception.message"));

        activity.Stop();
    }

    [Theory]
    [InlineData(typeof(ArgumentException))]
    [InlineData(typeof(InvalidOperationException))]
    [InlineData(typeof(NullReferenceException))]
    [InlineData(typeof(IndexOutOfRangeException))]
    public void SetExceptionTags_WithVariousExceptionTypes_SetsCorrectType(Type exceptionType)
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var ex = (Exception)Activator.CreateInstance(exceptionType, "Test message")!;

        // Act
        activity.SetExceptionTags(ex);

        // Assert
        Assert.Equal(exceptionType.FullName, activity.GetTagItem("exception.type"));

        activity.Stop();
    }

    // Custom exception class for testing
    private class CustomTestException : Exception
    {
        public CustomTestException(string message) : base(message)
        {
        }
    }
}

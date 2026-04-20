using System;
using Logging;
using Moq;
using Retries;
using Server;
using Server.Models;
using Xunit;

namespace Tests;

public class RetryPolicyTests
{
    private readonly Mock<ILogger> _mockLogger = new();
    private readonly Mock<IServerInterface> _mockServer = new();

    [Fact]
    public void DoOperation_SucceedsOnFirstAttempt_ReturnsSuccess()
    {
        _mockServer.Setup(s => s.DoOperation()).Returns(OperationResult.Success);
        RetryPolicy policy = new(_mockServer.Object, 3, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        Assert.True(result.IsSuccessful);
        _mockServer.Verify(s => s.DoOperation(), Times.Once);
    }

    [Fact]
    public void DoOperation_FailsThenSucceeds_ReturnsSuccess()
    {
        int callCount = 0;
        _mockServer.Setup(s => s.DoOperation()).Returns(() =>
        {
            callCount++;
            return callCount < 3 ? OperationResult.Error("fail") : OperationResult.Success;
        });

        RetryPolicy policy = new(_mockServer.Object, 3, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        Assert.True(result.IsSuccessful);
        Assert.Equal(3, callCount);
    }

    [Fact]
    public void DoOperation_AllAttemptsFail_ReturnsError()
    {
        _mockServer.Setup(s => s.DoOperation()).Returns(OperationResult.Error("fail"));
        RetryPolicy policy = new(_mockServer.Object, 3, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        Assert.False(result.IsSuccessful);
        _mockServer.Verify(s => s.DoOperation(), Times.Exactly(3));
        Assert.Equal("All retry attempts exhausted.", result.ErrorMsg);
    }

    [Fact]
    public void DoOperation_OperationThrowsException_RetriesAndReturnsError()
    {
        _mockServer.Setup(s => s.DoOperation()).Throws(new InvalidOperationException("boom"));
        RetryPolicy policy = new(_mockServer.Object, 2, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        Assert.False(result.IsSuccessful);
        _mockServer.Verify(s => s.DoOperation(), Times.Exactly(2));
    }

    [Fact]
    public void DoOperation_OperationThrowsThenSucceeds_ReturnsSuccess()
    {
        int callCount = 0;
        _mockServer.Setup(s => s.DoOperation()).Returns(() =>
        {
            callCount++;
            return callCount == 1 ? throw new InvalidOperationException("boom") : OperationResult.Success;
        });

        RetryPolicy policy = new(_mockServer.Object, 3, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        Assert.True(result.IsSuccessful);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void DoOperation_LogsEachFailedAttempt()
    {
        _mockServer.Setup(s => s.DoOperation()).Returns(OperationResult.Error("fail"));
        RetryPolicy policy = new(_mockServer.Object, 3, _mockLogger.Object);

        policy.DoOperation();

        _mockLogger.Verify(l => l.LogMessage(It.Is<string>(s => s.Contains("Attempt 1"))), Times.Once);
        _mockLogger.Verify(l => l.LogMessage(It.Is<string>(s => s.Contains("Attempt 2"))), Times.Once);
        _mockLogger.Verify(l => l.LogMessage(It.Is<string>(s => s.Contains("Attempt 3"))), Times.Once);
    }

    [Fact]
    public void DoOperation_AllAttemptsFail_LogsExhaustedMessage()
    {
        _mockServer.Setup(s => s.DoOperation()).Returns(OperationResult.Error("fail"));
        RetryPolicy policy = new(_mockServer.Object, 2, _mockLogger.Object);

        policy.DoOperation();

        _mockLogger.Verify(l => l.LogMessage("All retry attempts exhausted."), Times.Once);
    }

    [Fact]
    public void DoOperation_MaxRetriesOne_OnlyTriesOnce()
    {
        _mockServer.Setup(s => s.DoOperation()).Returns(OperationResult.Error("fail"));
        RetryPolicy policy = new(_mockServer.Object, 1, _mockLogger.Object);

        OperationResult result = policy.DoOperation();

        _mockServer.Verify(s => s.DoOperation(), Times.Once);
        Assert.True(result.HasError);
    }
}

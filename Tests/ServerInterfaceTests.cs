using Logging;
using Moq;
using Server;
using Server.Models;
using Xunit;

namespace Tests;

public class ServerInterfaceTests
{
    private readonly Mock<ILogger> _mockLogger = new();

    [Fact]
    public void DoOperation_ReturnsSuccess()
    {
        ServerInterface server = new(_mockLogger.Object);

        OperationResult result = server.DoOperation();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DoOperation_LogsMessages()
    {
        ServerInterface server = new(_mockLogger.Object);

        server.DoOperation();

        _mockLogger.Verify(l => l.LogMessage("Going to do the operation now."), Times.Once);
        _mockLogger.Verify(l => l.LogMessage("Did the operation successfully?"), Times.Once);
    }
}

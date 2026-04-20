using Server.Models;
using Xunit;

namespace Tests;

public class OperationResultTests
{
    [Fact]
    public void Success_IsSuccessful_ReturnsTrue()
    {
        OperationResult result = OperationResult.Success;

        Assert.True(result.IsSuccessful);
        Assert.False(result.HasError);
    }

    [Fact]
    public void Error_IsSuccessful_ReturnsFalse()
    {
        OperationResult result = OperationResult.Error("something went wrong");

        Assert.False(result.IsSuccessful);
        Assert.True(result.HasError);
    }

    [Fact]
    public void Error_ErrorMsg_ContainsMessage()
    {
        OperationResult result = OperationResult.Error("timeout");

        Assert.Equal("timeout", result.ErrorMsg);
    }

    [Fact]
    public void Success_ErrorMsg_IsEmpty()
    {
        OperationResult result = OperationResult.Success;

        Assert.Equal(string.Empty, result.ErrorMsg);
    }
}

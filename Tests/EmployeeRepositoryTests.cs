using System;
using System.Collections.Generic;
using Import.Common.Data;
using Import.Common.Models;
using Logging;
using Moq;
using Xunit;

namespace Tests;

public class EmployeeRepositoryTests
{
    private readonly Mock<IEmployeeDataContext> _mockContext = new();
    private readonly Mock<ILogger> _mockLogger = new();

    [Fact]
    public void GetEmployees_Empty_ReturnsEmptyAndLogs()
    {
        _mockContext.Setup(c => c.GetEmployees()).Returns(new List<Employee>());
        EmployeeRepository repository = CreateRepository();

        IReadOnlyList<Employee> result = repository.SelectEmployees();

        Assert.Empty(result);

        _mockLogger.Verify(l => l.LogMessage("0 employee(s) retrieved."), Times.Once);
    }

    [Fact]
    public void GetEmployees_Failure_LogsAndThrows()
    {
        _mockContext.Setup(c => c.GetEmployees()).Throws(new Exception("DB error"));
        EmployeeRepository repository = CreateRepository();

        Assert.Throws<Exception>(repository.SelectEmployees);

        _mockLogger.Verify(l => l.LogMessage("Select failed: DB error"), Times.Once);
    }

    [Fact]
    public void GetEmployees_Success_ReturnsEmployeesAndLogs()
    {
        List<Employee> expected = [new() { FirstName = "John" }, new() { FirstName = "Jane" }];
        _mockContext.Setup(c => c.GetEmployees()).Returns(expected);
        EmployeeRepository repository = CreateRepository();

        IReadOnlyList<Employee> result = repository.SelectEmployees();

        Assert.Equal(2, result.Count);
        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Jane", result[1].FirstName);

        _mockLogger.Verify(l => l.LogMessage("2 employee(s) retrieved."), Times.Once);
    }

    [Fact]
    public void InsertEmployees_Failure_LogsAndThrows()
    {
        _mockContext.Setup(c => c.SaveChanges()).Throws(new Exception("DB error"));
        EmployeeRepository repository = CreateRepository();

        Assert.Throws<Exception>(() => repository.InsertEmployees([new Employee()]));

        _mockLogger.Verify(l => l.LogMessage("Insert failed: DB error"), Times.Once);
    }

    [Fact]
    public void InsertEmployees_Success_LogsRowCount()
    {
        _mockContext.Setup(c => c.SaveChanges()).Returns(3);
        List<Employee> employees = [new(), new(), new()];
        EmployeeRepository repository = CreateRepository();

        repository.InsertEmployees(employees);

        _mockContext.Verify(c => c.AddEmployees(employees), Times.Once);
        _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        _mockLogger.Verify(l => l.LogMessage("3 row(s) inserted successfully."), Times.Once);
    }

    private EmployeeRepository CreateRepository() => new(_mockContext.Object, _mockLogger.Object);
}

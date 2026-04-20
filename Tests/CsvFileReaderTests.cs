using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Import.Common.Models;
using Import.Common.Readers;
using Logging;
using Moq;
using Xunit;

namespace Tests;

public class CsvFileReaderTests
{
    private readonly Mock<ILogger> _mockLogger = new();

    private static MemoryStream CreateCsvStream(string content) => new(Encoding.UTF8.GetBytes(content));

    [Fact]
    public void GetEmployees_ValidFile_ReturnsEmployees()
    {
        string csv =
            "FirstName,LastName,BirthDate,CurrentSalary\nJohn,Doe,1990-01-15,50000\nJane,Smith,1985-06-20,60000";
        using MemoryStream stream = CreateCsvStream(csv);
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(stream);

        Assert.Equal(2, employees.Count);
        Assert.Equal("John", employees[0].FirstName);
        Assert.Equal("Doe", employees[0].LastName);
        Assert.Equal(new DateTime(1990, 1, 15), employees[0].BirthDate);
        Assert.Equal(50000m, employees[0].CurrentSalary);
        Assert.Equal("Jane", employees[1].FirstName);
        Assert.Equal("Smith", employees[1].LastName);
    }

    [Fact]
    public void GetEmployees_FileNotFound_ReturnsEmptyListAndLogs()
    {
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(null);

        Assert.Empty(employees);
        _mockLogger.Verify(l => l.LogMessage("Error: File not found."), Times.Once);
    }

    [Fact]
    public void GetEmployees_EmptyFile_ReturnsEmptyList()
    {
        string csv = "FirstName,LastName,BirthDate,CurrentSalary\n";
        using MemoryStream stream = CreateCsvStream(csv);
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(stream);

        Assert.Empty(employees);
    }

    [Fact]
    public void GetEmployees_SkipsBlankLines()
    {
        string csv =
            "FirstName,LastName,BirthDate,CurrentSalary\nJohn,Doe,1990-01-15,50000\n\n\nJane,Smith,1985-06-20,60000\n";
        using MemoryStream stream = CreateCsvStream(csv);
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(stream);

        Assert.Equal(2, employees.Count);
    }

    [Fact]
    public void GetEmployees_InvalidRow_LogsAndSkips()
    {
        string csv =
            "FirstName,LastName,BirthDate,CurrentSalary\nJohn,Doe,1990-01-15,50000\nBadRow,Only\nJane,Smith,1985-06-20,60000";
        using MemoryStream stream = CreateCsvStream(csv);
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(stream);

        Assert.Equal(2, employees.Count);

        _mockLogger.Verify(l => l.LogMessage(It.Is<string>(s => s.Contains("Skipping invalid row"))), Times.Once);
    }

    [Fact]
    public void GetEmployees_DecimalSalary_ParsedCorrectly()
    {
        string csv = "FirstName,LastName,BirthDate,CurrentSalary\nJohn,Doe,2000-12-31,75000.50";
        using MemoryStream stream = CreateCsvStream(csv);
        CsvReader reader = new(_mockLogger.Object);

        List<Employee> employees = reader.GetEmployees(stream);

        Assert.Single(employees);
        Assert.Equal(75000.50m, employees[0].CurrentSalary);
    }
}

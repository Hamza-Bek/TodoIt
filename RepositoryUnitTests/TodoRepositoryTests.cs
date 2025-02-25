using Application.Interfaces;
using Application.Options;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace RepositoryUnitTests;

public class TodoRepositoryTests
{
    private readonly Mock<ITodoRepository> _mockTodoRepository;
    private readonly Guid _testUserId;
    private readonly Guid _testTodoId;

    public TodoRepositoryTests()
    {
        _mockTodoRepository = new Mock<ITodoRepository>();
        _testUserId = Guid.NewGuid();
        _testTodoId = Guid.NewGuid();
    }

    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnTodo_WhenTodoExists()
    {
        var expectedTodo = new Todo
        {
            Id = _testTodoId,
            OwnerId = _testUserId,
            Title = "Test Todo",
            DueDate = DateTime.UtcNow.Date,
            Priority = Priority.Medium
        };
        
        _mockTodoRepository.Setup(x => x.GetTodoByIdAsync(_testTodoId))
            .ReturnsAsync(expectedTodo);
        
        var result = await _mockTodoRepository.Object.GetTodoByIdAsync(_testTodoId);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testTodoId, result!.Id);
        Assert.AreEqual(_testUserId, result.OwnerId);
    }
    
    [Fact]
    public async Task GetTodoByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
    {
        _mockTodoRepository.Setup(x => x.GetTodoByIdAsync(_testTodoId))
            .ReturnsAsync(() => null);
        
        var result = await _mockTodoRepository.Object.GetTodoByIdAsync(_testTodoId);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task AddTodoAsync_ShouldReturnTodo_WhenTodoIsAdded()
    {
        var newTodo = new Todo
        {
            Id = _testTodoId,
            OwnerId = _testUserId,
            Title = "Test Todo",
            DueDate = DateTime.UtcNow.Date,
            Priority = Priority.Medium
        };
        
        _mockTodoRepository.Setup(x => x.AddTodoAsync(newTodo))
            .ReturnsAsync(newTodo);
        
        var result = await _mockTodoRepository.Object.AddTodoAsync(newTodo);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testTodoId, result!.Id);
        Assert.AreEqual(_testUserId, result.OwnerId);
    }
    
    [Fact]
    public async Task UpdateTodoAsync_ShouldReturnUpdatedTodo_WhenTodoIsUpdated()
    {
        var updatedTodo = new Todo
        {
            Id = _testTodoId,
            OwnerId = _testUserId,
            Title = "Updated Test Todo",
            DueDate = DateTime.UtcNow.Date,
            Priority = Priority.High
        };
        
        _mockTodoRepository.Setup(x => x.UpdateTodoAsync(_testTodoId, updatedTodo))
            .ReturnsAsync(updatedTodo);
        
        var result = await _mockTodoRepository.Object.UpdateTodoAsync(_testTodoId, updatedTodo);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testTodoId, result!.Id);
        Assert.AreEqual(_testUserId, result.OwnerId);
        Assert.AreEqual("Updated Test Todo", result.Title);
        Assert.AreEqual(Priority.High, result.Priority);
    }
    
    [Fact]
    public async Task UpdateTodoAsync_ShouldReturnNull_WhenTodoDoesNotExist()
    {
        _mockTodoRepository.Setup(x => x.UpdateTodoAsync(_testTodoId, It.IsAny<Todo>()))
            .ReturnsAsync(() => null);
        
        var result = await _mockTodoRepository.Object.UpdateTodoAsync(_testTodoId, new Todo());
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnTrue_WhenTodoIsDeleted()
    {
        _mockTodoRepository.Setup(x => x.DeleteTodoAsync(_testTodoId))
            .ReturnsAsync(true);
        
        var result = await _mockTodoRepository.Object.DeleteTodoAsync(_testTodoId);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteTodoAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
    {
        _mockTodoRepository.Setup(x => x.DeleteTodoAsync(_testTodoId))
            .ReturnsAsync(false);
        
        var result = await _mockTodoRepository.Object.DeleteTodoAsync(_testTodoId);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task RescheduleTodosAsync_ShouldRescheduleTodos_WhenTodosExist()
    {
        var todoIds = new List<Guid>
        {
            _testTodoId
        };
        
        _mockTodoRepository.Setup(x => x.RescheduleTodosAsync(todoIds, It.IsAny<DateTime>()))
            .Verifiable();
        
        await _mockTodoRepository.Object.RescheduleTodosAsync(todoIds, DateTime.UtcNow.Date);
        
        _mockTodoRepository.Verify(x => x.RescheduleTodosAsync(todoIds, It.IsAny<DateTime>()), Times.Once);
    }
}
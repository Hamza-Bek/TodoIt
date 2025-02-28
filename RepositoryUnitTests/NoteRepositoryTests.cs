using Application.Dtos.Note;
using Application.Interfaces;
using Domain.Models;
using Moq;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace RepositoryUnitTests;

public class NoteRepositoryTests
{
    private readonly Mock<INoteRepository> _mockNoteRepository;
    private readonly Guid _testNoteId;
    private readonly Guid _testFolderId;
    private readonly Guid _testUserId;
    
    public NoteRepositoryTests()
    {
        _mockNoteRepository = new Mock<INoteRepository>();
        _testNoteId = Guid.NewGuid();
        _testFolderId = Guid.NewGuid();
        _testUserId = Guid.NewGuid();
    }
    
    // Add tests here
    
    [Fact]
    public async Task GetNoteByIdAsync_ShouldReturnNote_WhenNoteExists()
    {
        var expectedNote = new Note
        {
            Id = _testNoteId,
            FolderId = _testFolderId,
            OwnerId = _testUserId,
            Title = "Test Note",
            Content = "Test Note Content"
        };
        
        _mockNoteRepository.Setup(x => x.GetNoteByIdAsync(_testNoteId))
            .ReturnsAsync(expectedNote);
        
        var result = await _mockNoteRepository.Object.GetNoteByIdAsync(_testNoteId);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testNoteId, result!.Id);
        Assert.AreEqual(_testFolderId, result.FolderId);
        Assert.AreEqual(_testUserId, result.OwnerId);
    }
    
    [Fact]
    public async Task GetNoteByIdAsync_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
    {
        _mockNoteRepository.Setup(x => x.GetNoteByIdAsync(_testNoteId))
            .ThrowsAsync(new ("Note not found"));

        Assert.ThrowsAsync<Exception>(() => _mockNoteRepository.Object.GetNoteByIdAsync(_testNoteId));
    }
    
    [Fact]
    public async Task AddNoteAsync_ShouldReturnNote_WhenNoteIsAdded()
    {
        var newNote = new Note
        {
            Id = _testNoteId,
            FolderId = _testFolderId,
            OwnerId = _testUserId,
            Title = "Test Note",
            Content = "Test Note Content"
        };
        
        _mockNoteRepository.Setup(x => x.AddNoteAsync(newNote))
            .ReturnsAsync(newNote);
        
        var result = await _mockNoteRepository.Object.AddNoteAsync(newNote);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testNoteId, result.Id);
        Assert.AreEqual(_testFolderId, result.FolderId);
        Assert.AreEqual(_testUserId, result.OwnerId);
    }
    
    [Fact]
    public async Task UpdateNoteAsync_ShouldReturnNote_WhenNoteIsUpdated()
    {
        var updatedNote = new Note
        {
            Id = _testNoteId,
            FolderId = _testFolderId,
            OwnerId = _testUserId,
            Title = "Updated Test Note",
            Content = "Updated Test Note Content"
        };
        
        _mockNoteRepository.Setup(x => x.UpdateNoteAsync(_testNoteId, updatedNote))
            .ReturnsAsync(updatedNote);
        
        var result = await _mockNoteRepository.Object.UpdateNoteAsync(_testNoteId, updatedNote);
        
        Assert.NotNull(result);
        Assert.AreEqual(_testNoteId, result.Id);
        Assert.AreEqual(_testFolderId, result.FolderId);
        Assert.AreEqual(_testUserId, result.OwnerId);
        Assert.AreEqual("Updated Test Note", result.Title);
        Assert.AreEqual("Updated Test Note Content", result.Content);
    }
    
    [Fact]
    public async Task UpdateNoteAsync_ShouldReturnNull_WhenNoteDoesNotExist()
    {
        _mockNoteRepository.Setup(x => x.UpdateNoteAsync(_testNoteId, It.IsAny<Note>()))
            .ReturnsAsync(() => null);
        
        var result = await _mockNoteRepository.Object.UpdateNoteAsync(_testNoteId, new Note());
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteNoteAsync_ShouldReturnTrue_WhenNoteIsDeleted()
    {
        _mockNoteRepository.Setup(x => x.DeleteNoteAsync(_testNoteId))
            .ReturnsAsync(true);
        
        var result = await _mockNoteRepository.Object.DeleteNoteAsync(_testNoteId);
        
        Assert.IsTrue(result);
    }
    
    [Fact]
    public async Task DeleteNoteAsync_ShouldReturnFalse_WhenNoteDoesNotExist()
    {
        _mockNoteRepository.Setup(x => x.DeleteNoteAsync(_testNoteId))
            .ReturnsAsync(false);
        
        var result = await _mockNoteRepository.Object.DeleteNoteAsync(_testNoteId);
        
        Assert.IsFalse(result);
    }
    
    [Fact]
    public async Task GetNotesByFilterCriteriaAsync_ShouldReturnNotes_WhenNotesExist()
    {
        // Arrange
        var expectedNotes = new List<Note>
        {
            new Note
            {
                Id = _testNoteId,
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note",
                Content = "Test Note Content"
            },
            new Note
            {
                Id = Guid.NewGuid(),
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note 2",
                Content = "Test Note Content 2"
            }
        };

        var filterCriteria = new NoteFilterCriteria
        {
            Title = "Test Note"
        };

        _mockNoteRepository.Setup(x => x.GetNotesAsync(It.IsAny<NoteFilterCriteria>()))
            .ReturnsAsync(expectedNotes);

        // Act
        var result = await _mockNoteRepository.Object.GetNotesAsync(filterCriteria);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(_testNoteId, result.First().Id);
        Assert.AreEqual(_testFolderId, result.First().FolderId);
        Assert.AreEqual(_testUserId, result.First().OwnerId);
        Assert.AreEqual("Test Note", result.First().Title);
        Assert.AreEqual("Test Note Content", result.First().Content);
    }
    
    [Fact]
    public async Task GetNotesByFilterCriteriaAsync_ShouldReturnEmptyList_WhenNotesDoNotExist()
    {
        // Arrange
        var filterCriteria = new NoteFilterCriteria
        {
            Title = "Test Note"
        };

        _mockNoteRepository.Setup(x => x.GetNotesAsync(It.IsAny<NoteFilterCriteria>()))
            .ReturnsAsync(() => new List<Note>());

        // Act
        var result = await _mockNoteRepository.Object.GetNotesAsync(filterCriteria);

        // Assert
        Assert.NotNull(result);
        Assert.IsEmpty(result);
    }
    
    [Fact]
    public async Task GetNotesByFilterCriteriaAsync_ShouldReturnAllNotes_WhenFilterCriteriaIsNull()
    {
        // Arrange
        var expectedNotes = new List<Note>
        {
            new Note
            {
                Id = _testNoteId,
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note",
                Content = "Test Note Content"
            },
            new Note
            {
                Id = Guid.NewGuid(),
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note 2",
                Content = "Test Note Content 2"
            }
        };

        _mockNoteRepository.Setup(x => x.GetNotesAsync(null))
            .ReturnsAsync(expectedNotes);

        // Act
        var result = await _mockNoteRepository.Object.GetNotesAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(_testNoteId, result.First().Id);
        Assert.AreEqual(_testFolderId, result.First().FolderId);
        Assert.AreEqual(_testUserId, result.First().OwnerId);
        Assert.AreEqual("Test Note", result.First().Title);
        Assert.AreEqual("Test Note Content", result.First().Content);
    }
    
    [Fact]
    public async Task GetNotesByFilterCriteriaAsync_ShouldReturnEmptyList_WhenNotesDoNotExistAndFilterCriteriaIsNull()
    {
        // Arrange
        _mockNoteRepository.Setup(x => x.GetNotesAsync(null))
            .ReturnsAsync(() => new List<Note>());

        // Act
        var result = await _mockNoteRepository.Object.GetNotesAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.IsEmpty(result);
    }
    
    [Fact]
    public async Task GetNotesByFilterCriteriaAsync_ShouldReturnAllNotes_WhenFilterCriteriaIsEmpty()
    {
        // Arrange
        var expectedNotes = new List<Note>
        {
            new Note
            {
                Id = _testNoteId,
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note",
                Content = "Test Note Content"
            },
            new Note
            {
                Id = Guid.NewGuid(),
                FolderId = _testFolderId,
                OwnerId = _testUserId,
                Title = "Test Note 2",
                Content = "Test Note Content 2"
            }
        };

        _mockNoteRepository.Setup(x => x.GetNotesAsync(It.IsAny<NoteFilterCriteria>()))
            .ReturnsAsync(expectedNotes);

        // Act
        var result = await _mockNoteRepository.Object.GetNotesAsync(new NoteFilterCriteria());

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(_testNoteId, result.First().Id);
        Assert.AreEqual(_testFolderId, result.First().FolderId);
        Assert.AreEqual(_testUserId, result.First().OwnerId);
        Assert.AreEqual("Test Note", result.First().Title);
        Assert.AreEqual("Test Note Content", result.First().Content);
    }

}
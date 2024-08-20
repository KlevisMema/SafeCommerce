/*
 * Defines the LogEntry class for tracking user activity in the system.
 * This file outlines the structure of a log entry, including its attributes and data annotations.
*/

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccess.Models;

/// <summary>
/// Represents a log entry for user activity monitoring.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets or sets the primary key of the log entry
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the log entry was created.
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// Gets or sets the user ID associated with the log entry.
    /// </summary>
    [Required]
    public string UserId { get; set; } = null!;
    /// <summary>
    /// Gets or sets the details of the log entry.
    /// </summary>
    [Required, StringLength(200)]
    public string Details { get; set; } = null!;
}
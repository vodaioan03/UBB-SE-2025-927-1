﻿using Duo.Api.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models
{
    /// <summary>
    /// Represents the completion status of a course by a user,
    /// including reward claim status and completion timestamp.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CourseCompletion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who completed the course.
        /// </summary>
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the completed course.
        /// </summary>
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the completion reward has been claimed.
        /// </summary>
        public bool CompletionRewardClaimed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timed reward has been claimed.
        /// </summary>
        public bool TimedRewardClaimed { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the course was completed.
        /// </summary>
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// Navigation property to the related course.
        /// </summary>
        public Course Course { get; set; } = null!;

        /// <summary>
        /// Navigation property to the related user.
        /// </summary>
        public User User { get; set; } = null!;
    }
}

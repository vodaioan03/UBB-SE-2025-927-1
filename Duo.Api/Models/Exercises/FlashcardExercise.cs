using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents a flashcard-style exercise.
    /// </summary>
    public class FlashcardExercise : Exercise
    {
        /// <summary>
        /// Gets the exercise type.
        /// </summary>
        public string Type { get; set; } = "Flashcard";

        /// <summary>
        /// Gets or sets the time allowed to complete the exercise in seconds.
        /// </summary>
        public int TimeInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the correct answer for the flashcard.
        /// </summary>
        /// 
        [Required]
        public string Answer { get; set; }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseApp.Models
{
    /// <summary>
    /// Represents a tag that can be assigned to courses or modules, with support for property change notifications.
    /// </summary>
    public partial class Tag
    {
        private int _tagId;
        private string _name = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public int TagId
        {
            get => _tagId;
            set
            {
                if (_tagId != value)
                {
                    _tagId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

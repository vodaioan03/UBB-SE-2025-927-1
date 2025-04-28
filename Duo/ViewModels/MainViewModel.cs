using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Services;
using Windows.System.Threading;

#pragma warning disable IDE0028, CS8618, CS8602, CS8601, IDE0060

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the main application logic, including course display, filtering, and user coin balance.
    /// </summary>
    public partial class MainViewModel : BaseViewModel, IMainViewModel
    {
        private const int CurrentUserId = 0;

        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;

        private string searchQuery = string.Empty;
        private bool filterByPremium;
        private bool filterByFree;
        private bool filterByEnrolled;
        private bool filterByNotEnrolled;

        /// <summary>
        /// Observable collection of courses to be displayed.
        /// </summary>
        public ObservableCollection<Course> DisplayedCourses { get; private set; }

        /// <summary>
        /// Observable collection of available tags.
        /// </summary>
        public ObservableCollection<Tag> AvailableTags { get; private set; }

        /// <summary>
        /// User's current coin balance.
        /// </summary>
        private int userCoinBalance;
        public int UserCoinBalance
        {
            get => userCoinBalance;
            private set
            {
                userCoinBalance = value;
                OnPropertyChanged(nameof(UserCoinBalance));
            }
        }

        public async Task RefreshUserCoinBalanceAsync()
        {
            UserCoinBalance = await coinsService.GetCoinBalanceAsync(CurrentUserId);
        }

        /// <summary>
        /// The search query used to filter courses.
        /// </summary>
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (value.Length <= 100 && searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for premium courses.
        /// </summary>
        public bool FilterByPremium
        {
            get => filterByPremium;
            set
            {
                if (filterByPremium != value)
                {
                    filterByPremium = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for free courses.
        /// </summary>
        public bool FilterByFree
        {
            get => filterByFree;
            set
            {
                if (filterByFree != value)
                {
                    filterByFree = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for enrolled courses.
        /// </summary>
        public bool FilterByEnrolled
        {
            get => filterByEnrolled;
            set
            {
                if (filterByEnrolled != value)
                {
                    filterByEnrolled = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for not enrolled courses.
        /// </summary>
        public bool FilterByNotEnrolled
        {
            get => filterByNotEnrolled;
            set
            {
                if (filterByNotEnrolled != value)
                {
                    filterByNotEnrolled = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Command to reset all filters.
        /// </summary>
        public ICommand ResetAllFiltersCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(CoinsServiceProxy serviceProxy, ICourseService? courseService = null, ICoinsService? coinsService = null)
        {
            this.courseService = courseService ?? new CourseService();
            this.coinsService = coinsService ?? new CoinsService(serviceProxy);

            DisplayedCourses = new ObservableCollection<Course>(this.courseService.GetCourses());
            AvailableTags = new ObservableCollection<Tag>(this.courseService.GetTags());

            foreach (var tag in AvailableTags)
            {
                tag.PropertyChanged += OnTagSelectionChanged;
            }

            ResetAllFiltersCommand = new RelayCommand(ResetAllFilters);
        }

        /// <summary>
        /// Attempts to grant a daily login reward to the user.
        /// </summary>
        public async Task<bool> TryDailyLoginReward()
        {
            bool loginRewardGranted = await coinsService.ApplyDailyLoginBonusAsync();
            OnPropertyChanged(nameof(UserCoinBalance));
            return loginRewardGranted;
        }

        private void OnTagSelectionChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(Tag.IsSelected))
            {
                ApplyAllFilters();
            }
        }

        /// <summary>
        /// Resets all the filters and clears the search query.
        /// </summary>
        private void ResetAllFilters(object? parameter)
        {
            SearchQuery = string.Empty;
            FilterByPremium = false;
            FilterByFree = false;
            FilterByEnrolled = false;
            FilterByNotEnrolled = false;

            foreach (var tag in AvailableTags)
            {
                tag.IsSelected = false;
            }

            ApplyAllFilters();
        }

        /// <summary>
        /// Applies all filters based on search query, selected tags, and filter flags.
        /// </summary>
        private void ApplyAllFilters()
        {
            var selectedTagIds = AvailableTags
                .Where(tag => tag.IsSelected)
                .Select(tag => tag.TagId)
                .ToList();

            var filteredCourses = courseService.GetFilteredCourses(
                searchQuery,
                filterByPremium,
                filterByFree,
                filterByEnrolled,
                filterByNotEnrolled,
                selectedTagIds);

            DisplayedCourses.Clear();
            foreach (var course in filteredCourses)
            {
                DisplayedCourses.Add(course);
            }
        }
    }
}

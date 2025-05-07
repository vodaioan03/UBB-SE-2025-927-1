using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Services;
using Duo.ViewModels.Helpers;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for handling course presentation, progress tracking, and user interactions
    /// </summary>
    public partial class CourseViewModel : BaseViewModel, ICourseViewModel
    {
        #region Constants

        /// <summary>Duration for which notifications are displayed (in seconds)</summary>
        internal const int NotificationDisplayDurationInSeconds = 3;

        /// <summary>Coin reward for completing all required modules</summary>
        private const int CourseCompletionRewardCoins = 50;

        /// <summary>Coin reward for completing the course within time limit</summary>
        private const int TimedCompletionRewardCoins = 300;

        /// <summary>Adjustment factor for time tracking to prevent double counting</summary>
        private const int TimeTrackingDatabaseAdjustmentDivisor = 2;

        /// <summary>Number of minutes in one hour</summary>
        private const int MinutesInAnHour = 60;
        #endregion

        #region Fields
        private IDispatcherTimerService? courseProgressTimer;
        private int totalSecondsSpentOnCourse;
        private int courseCompletionTimeLimitInSeconds;
        private string? formattedTimeRemaining;
        internal bool IsCourseTimerRunning;
        private int lastSavedTimeInSeconds = 0;

        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;
        private INotificationHelper? notificationHelper;

        private string notificationMessageText = string.Empty;
        private bool shouldShowNotification = false;
        #endregion

        #region Properties

        /// <summary>Gets the current course being viewed</summary>
        public Course CurrentCourse { get; }

        /// <summary>Gets the collection of modules with their progress status</summary>
        public ObservableCollection<ModuleProgressStatus> ModuleRoadmap { get; } = [];

        /// <summary>Gets the command for enrolling in the course</summary>
        public ICommand? EnrollCommand { get; private set; }

        /// <summary>Gets a value indicating whether the user is enrolled in this course</summary>
        public bool IsEnrolled { get; set; }

        /// <summary>Gets whether coin information should be visible</summary>
        public bool CoinVisibility => CurrentCourse.IsPremium && !IsEnrolled;

        /// <summary>Gets the current coin balance of the user</summary>
        private int coinBalance;
        public int CoinBalance
        {
            get => coinBalance;
            set
            {
                if (coinBalance != value)
                {
                    coinBalance = value;
                    OnPropertyChanged(nameof(CoinBalance));
                }
            }
        }

        public async Task<int> GetCoinBalanceAsync(int currentUserId)
        {
            CoinBalance = await coinsService.GetCoinBalanceAsync(currentUserId);
            return CoinBalance;
        }

        private ObservableCollection<Tag> tags = new ();

        public ObservableCollection<Tag> Tags
        {
            get => tags;
            private set
            {
                tags = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadTagsAsync()
        {
            try
            {
                var tagList = await courseService.GetCourseTagsAsync(CurrentCourse.CourseId);
                Tags = new ObservableCollection<Tag>(tagList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Gets or sets the formatted string representing time remaining in course
        /// Format: "X min Y sec"
        /// </summary>
        public string FormattedTimeRemaining
        {
            get => formattedTimeRemaining!;
            private set
            {
                formattedTimeRemaining = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the notification message to display</summary>
        public virtual string NotificationMessage
        {
            get => notificationMessageText;
            set
            {
                notificationMessageText = value;
                OnPropertyChanged(nameof(NotificationMessage));
            }
        }

        /// <summary>Gets or sets whether notification should be visible</summary>
        public virtual bool ShowNotification
        {
            get => shouldShowNotification;
            set
            {
                shouldShowNotification = value;
                OnPropertyChanged(nameof(ShowNotification));
            }
        }

        /// <summary>Gets the number of completed modules</summary>
        public int CompletedModules { get; private set; }

        /// <summary>Gets the number of required modules</summary>
        public int RequiredModules { get; private set; }

        /// <summary>Gets whether all required modules are completed</summary>
        public bool IsCourseCompleted => CompletedModules >= RequiredModules;

        /// <summary>Gets the total time limit for course completion (in seconds)</summary>
        public int TimeLimit { get; private set; }

        /// <summary>Gets the remaining time to complete the course (in seconds)</summary>
        public int TimeRemaining => Math.Max(0, TimeLimit - totalSecondsSpentOnCourse);

        /// <summary>Gets whether completion reward was claimed</summary>
        public bool CompletionRewardClaimed { get; private set; }

        /// <summary>Gets whether timed completion reward was claimed</summary>
        public bool TimedRewardClaimed { get; private set; }
        #endregion

        #region Nested Classes

        /// <summary>
        /// Represents a module along with its progress status
        /// </summary>
        public class ModuleProgressStatus
        {
            /// <summary>Gets or sets the module</summary>
            public Module? Module { get; set; }

            /// <summary>Gets or sets whether the module is unlocked</summary>
            public bool IsUnlocked { get; set; }

            /// <summary>Gets or sets whether the module is completed</summary>
            public bool IsCompleted { get; set; }
        }
        #endregion

        #region Constructor and Initialization

        /// <summary>
        /// Initializes a new instance of the CourseViewModel class
        /// </summary>
        public CourseViewModel()
        {
            CurrentCourse = new Course
            {
                Title = string.Empty,
                Description = string.Empty,
                ImageUrl = string.Empty,
                Difficulty = string.Empty
            };

            var httpClient = new System.Net.Http.HttpClient();
            var courseServiceProxy = new CourseServiceProxy(httpClient);

            courseService = new CourseService(courseServiceProxy);
            coinsService = new CoinsService(new CoinsServiceProxy(httpClient));

            notificationHelper = null;
        }

        /// <summary>
        /// Initializes a new instance of the CourseViewModel class
        /// </summary>
        /// <param name="course">The course to display and manage</param>
        /// <param name="courseService">The service for course-related operations (optional)</param>
        /// <param name="coinsService">The service for coin-related operations (optional)</param>
        /// <param name="timerService">The timer service for course progress tracking (optional)</param>
        /// <param name="notificationTimerService">The timer service for notifications (optional)</param>
        /// <exception cref="ArgumentNullException">Thrown when course is null</exception>
        public CourseViewModel(Course course, int currentUserId = 1, ICourseService? courseService = null,
            ICoinsService? coinsService = null, IDispatcherTimerService? timerService = null,
            IDispatcherTimerService? notificationTimerService = null, INotificationHelper? notificationHelper = null,
            CourseServiceProxy? serviceProxy = null)
        {
            CurrentCourse = course ?? throw new ArgumentNullException(nameof(course));

            var httpClient = new System.Net.Http.HttpClient();
            var defaultServiceProxy = serviceProxy ?? new CourseServiceProxy(httpClient);

            this.courseService = courseService ?? new CourseService(defaultServiceProxy);
            this.coinsService = coinsService ?? new CoinsService(new CoinsServiceProxy(httpClient));

            InitializeTimersAndNotificationHelper(timerService, notificationTimerService, notificationHelper);
        }
        public async Task InitializeAsync(int currentUserId)
        {
            await InitializeProperties(currentUserId);
            await LoadInitialData(currentUserId);
        }

        /// <summary>
        /// Initializes the timers and notification helper for the course progress and notifications.
        /// If any of the parameters are null, default implementations are used.
        /// </summary>
        /// <param name="timerService">Optional dispatcher timer service for course progress tracking.</param>
        /// <param name="notificationTimerService">Optional dispatcher timer service for notifications.</param>
        /// <param name="notificationHelper">Optional notification helper instance.</param>
        [ExcludeFromCodeCoverage]
        private void InitializeTimersAndNotificationHelper(IDispatcherTimerService? timerService,
            IDispatcherTimerService? notificationTimerService, INotificationHelper? notificationHelper)
        {
            courseProgressTimer = timerService ?? new DispatcherTimerService();
            var notificationTimer = notificationTimerService ?? new DispatcherTimerService();

            this.notificationHelper = notificationHelper ?? new NotificationHelper(this, notificationTimer);

            courseProgressTimer.Tick += OnCourseTimerTick;
        }

        /// <summary>
        /// Initializes key ViewModel properties such as enrollment status and enrollment command.
        /// </summary>
        private async Task InitializeProperties(int currentUserId)
        {
            try
            {
                IsEnrolled = await courseService.IsUserEnrolledAsync(currentUserId, CurrentCourse.CourseId);

                EnrollCommand = new RelayCommand(
                    async (parameter) => await EnrollUserInCourseAsync(parameter, currentUserId),
                    async (parameter) => await CanUserEnrollInCourseAsync(parameter, currentUserId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Loads the initial course-related data such as time spent, modules completed,
        /// time remaining, and initializes the course module structure.
        /// </summary>
        private async Task LoadInitialData(int currentUserId)
        {
            try
            {
                totalSecondsSpentOnCourse = await courseService.GetTimeSpentAsync(currentUserId, CurrentCourse.CourseId);
                lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
                courseCompletionTimeLimitInSeconds = CurrentCourse.TimeToComplete - totalSecondsSpentOnCourse;
                FormattedTimeRemaining = FormatTimeRemainingDisplay(courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse);

                CompletedModules = await courseService.GetCompletedModulesCountAsync(currentUserId, CurrentCourse.CourseId);
                RequiredModules = await courseService.GetRequiredModulesCountAsync(CurrentCourse.CourseId);
                TimeLimit = await courseService.GetCourseTimeLimitAsync(CurrentCourse.CourseId);

                await LoadAndOrganizeCourseModules(currentUserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Timer Methods

        /// <summary>
        /// Handles the Tick event of the course progress timer, updating the total time spent
        /// and refreshing the time display.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> object that contains no event data.</param>
        private void OnCourseTimerTick(object? sender, EventArgs e)
        {
            totalSecondsSpentOnCourse++;
            UpdateTimeDisplay();
            OnPropertyChanged(nameof(TimeRemaining));
        }

        /// <summary>
        /// Updates the formatted time display
        /// </summary>
        private void UpdateTimeDisplay()
        {
            int remainingSeconds = courseCompletionTimeLimitInSeconds - totalSecondsSpentOnCourse;
            FormattedTimeRemaining = FormatTimeRemainingDisplay(Math.Max(0, remainingSeconds));
        }
        #endregion

        #region Module Management

        /// <summary>
        /// Loads and organizes all modules for the current course with their progress status
        /// </summary>
        public async Task LoadAndOrganizeCourseModules(int currentUserId)
        {
            try
            {
                var modules = (await courseService.GetModulesAsync(CurrentCourse.CourseId))
                           .OrderBy(module => module.Position)
                           .ToList();

                ModuleRoadmap.Clear();

            for (int index = 0; index < modules.Count; index++)
            {
                var module = modules[index];
                bool isCompleted = await courseService.IsModuleCompletedAsync(currentUserId, module.ModuleId);
                bool isUnlocked = await GetModuleUnlockStatus(module, index, currentUserId);

                    ModuleRoadmap.Add(new ModuleProgressStatus
                    {
                        Module = module,
                        IsUnlocked = isUnlocked,
                        IsCompleted = isCompleted
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            OnPropertyChanged(nameof(ModuleRoadmap));
        }

        /// <summary>
        /// Determines if a module should be unlocked based on its position and progress
        /// </summary>
        /// <param name="module">The module to check</param>
        /// <param name="moduleIndex">The index of the module in the collection</param>
        /// <returns>True if the module should be unlocked, otherwise false</returns>
        private async Task<bool> GetModuleUnlockStatus(Module module, int moduleIndex, int currentUserId)
        {
            try
            {
                if (!module.IsBonus)
                {
                    return IsEnrolled &&
                           (moduleIndex == 0 ||
                            await courseService.IsModuleCompletedAsync(currentUserId, ModuleRoadmap[moduleIndex - 1].Module!.ModuleId));
                }
                return await courseService.IsModuleInProgressAsync(currentUserId, module.ModuleId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Determines if the user can enroll in the course
        /// </summary>
        private async Task<bool> CanUserEnrollInCourseAsync(object? parameter, int currentUserId)
        {
            int coinBalance = await GetCoinBalanceAsync(currentUserId);
            return !IsEnrolled && coinBalance >= CurrentCourse.Cost;
        }

        /// <summary>
        /// Enrolls the user in the current course
        /// </summary>
        private async Task EnrollUserInCourseAsync(object? parameter, int currentUserId)
        {
            try
            {
                bool coinDeductionSuccessful = await coinsService.TrySpendingCoinsAsync(currentUserId, CurrentCourse.Cost);

                if (!coinDeductionSuccessful)
                {
                    return;
                }

            bool enrollmentSuccessful = await courseService.EnrollInCourseAsync(currentUserId, CurrentCourse.CourseId);
            if (!enrollmentSuccessful)
            {
                return;
            }

                IsEnrolled = true;
                ResetCourseProgressTracking();
                OnPropertyChanged(nameof(IsEnrolled));
                OnPropertyChanged(nameof(CoinBalance));

                StartCourseProgressTimer();
                await LoadAndOrganizeCourseModules(currentUserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Resets all course progress tracking metrics
        /// </summary>
        private void ResetCourseProgressTracking()
        {
            totalSecondsSpentOnCourse = 0;
            FormattedTimeRemaining = FormatTimeRemainingDisplay(totalSecondsSpentOnCourse);
        }
        #endregion

        #region Timer Control Methods

        /// <summary>
        /// Starts the course progress timer if not already running
        /// </summary>
        public void StartCourseProgressTimer()
        {
            if (!IsCourseTimerRunning && IsEnrolled)
            {
                IsCourseTimerRunning = true;
                courseProgressTimer!.Start();
            }
        }

        /// <summary>
        /// Pauses the course progress timer and saves the current progress
        /// </summary>
        public async Task PauseCourseProgressTimer(int currentUserId)
        {
            if (IsCourseTimerRunning)
            {
                courseProgressTimer!.Stop();
                await SaveCourseProgressTime(currentUserId);
                IsCourseTimerRunning = false;
            }
        }

        /// <summary>
        /// Saves the current course progress time to the database
        /// </summary>
        private async Task SaveCourseProgressTime(int currentUserId)
        {
            try
            {
                int secondsToSave = (totalSecondsSpentOnCourse - lastSavedTimeInSeconds) /
                                TimeTrackingDatabaseAdjustmentDivisor;

                Console.WriteLine($"Attempting to save: Current={totalSecondsSpentOnCourse}, " +
                                  $"LastSaved={lastSavedTimeInSeconds}, ToSave={secondsToSave}");

                if (secondsToSave > 0)
                {
                    Console.WriteLine($"Saving {secondsToSave} seconds");
                    await courseService.UpdateTimeSpentAsync(currentUserId, CurrentCourse.CourseId, secondsToSave);
                    lastSavedTimeInSeconds = totalSecondsSpentOnCourse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Formats time in seconds to a display string (X min Y sec)
        /// </summary>
        /// <param name="totalSeconds">Total seconds to format</param>
        /// <returns>Formatted time string</returns>
        internal static string FormatTimeRemainingDisplay(int totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            int totalMinutes = timeSpan.Minutes + (timeSpan.Hours * MinutesInAnHour);
            return $"{totalMinutes} min {timeSpan.Seconds} sec";
        }

        /// <summary>
        /// Refreshes the course modules display
        /// </summary>
        public async Task RefreshCourseModulesDisplay(int currentUserId)
        {
            await LoadAndOrganizeCourseModules(currentUserId);
        }

        #endregion

        #region Reward Handling

        /// <summary>
        /// Marks a module as completed and checks for any earned rewards
        /// </summary>
        /// <param name="targetModuleId">ID of the module to mark as completed</param>
        public async Task MarkModuleAsCompletedAndCheckRewards(int targetModuleId, int currentUserId)
        {
            try
            {
                await courseService.CompleteModuleAsync(currentUserId, targetModuleId, CurrentCourse.CourseId);
                await UpdateCompletionStatus(currentUserId);

                if (IsCourseCompleted)
                {
                    await CheckForCompletionReward(currentUserId);
                    await CheckForTimedReward(currentUserId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Updates the module completion status properties
        /// </summary>
        private async Task UpdateCompletionStatus(int currentUserId)
        {
            try
            {
                CompletedModules = await courseService.GetCompletedModulesCountAsync(currentUserId, CurrentCourse.CourseId);
                OnPropertyChanged(nameof(CompletedModules));
                OnPropertyChanged(nameof(IsCourseCompleted));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Checks and claims the course completion reward if eligible
        /// </summary>
        private async Task CheckForCompletionReward(int currentUserId)
        {
            try
            {
                bool rewardClaimed = await courseService.ClaimCompletionRewardAsync(currentUserId, CurrentCourse.CourseId);
                if (rewardClaimed)
                {
                    CompletionRewardClaimed = true;
                    OnPropertyChanged(nameof(CompletionRewardClaimed));
                    OnPropertyChanged(nameof(CoinBalance));
                    ShowCourseCompletionRewardNotification();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Checks and claims the timed completion reward if eligible
        /// </summary>
        private async Task CheckForTimedReward(int currentUserId)
        {
            try
            {
                if (TimeRemaining > 0)
                {
                    bool rewardClaimed = await courseService.ClaimTimedRewardAsync(currentUserId, CurrentCourse.CourseId, totalSecondsSpentOnCourse);
                    if (rewardClaimed)
                    {
                        TimedRewardClaimed = true;
                        OnPropertyChanged(nameof(TimedRewardClaimed));
                        OnPropertyChanged(nameof(CoinBalance));
                        ShowTimedCompletionRewardNotification();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Notification Methods

        /// <summary>
        /// Shows notification for course completion reward
        /// </summary>
        private void ShowCourseCompletionRewardNotification()
        {
            string message = $"Congratulations! You have completed all required modules in this course. {CourseCompletionRewardCoins} coins have been added to your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
        }

        /// <summary>
        /// Shows notification for timed completion reward
        /// </summary>
        private void ShowTimedCompletionRewardNotification()
        {
            string message = $"Congratulations! You completed the course within the time limit. {TimedCompletionRewardCoins} coins have been added to your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
        }

        /// <summary>
        /// Shows notification for successful module purchase
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private async Task ShowModulePurchaseNotificationAsync(Module module, int currentUserId)
        {
            string message = $"Congratulations! You have purchased bonus module {module.Title}, {module.Cost} coins have been deducted from your balance.";
            notificationHelper!.ShowTemporaryNotification(message);
            await RefreshCourseModulesDisplay(currentUserId);
        }
        #endregion

        #region Module Purchase

        /// <summary>
        /// Attempts to purchase a bonus module
        /// </summary>
        /// <param name="module">The module to purchase</param>
        public async Task AttemptBonusModulePurchaseAsync(Module? module, int currentUserId)
        {
            ArgumentNullException.ThrowIfNull(module);

            try
            {
                if (await courseService.IsModuleCompletedAsync(currentUserId, module.ModuleId))
                {
                    return;
                }

                bool purchaseSuccessful = await courseService.BuyBonusModuleAsync(currentUserId, module.ModuleId, CurrentCourse.CourseId);

                if (purchaseSuccessful)
                {
                    await UpdatePurchasedModuleStatus(module, currentUserId);
                    await ShowModulePurchaseNotificationAsync(module, currentUserId);
                    OnPropertyChanged(nameof(ModuleRoadmap));
                    OnPropertyChanged(nameof(CoinBalance));
                }
                else
                {
                    ShowPurchaseFailedNotification();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Updates the status of a purchased module
        /// </summary>
        /// <param name="module">The module that was purchased</param>
        private async Task UpdatePurchasedModuleStatus(Module module, int currentUserId)
        {
            try
            {
                var moduleToUpdate = ModuleRoadmap.FirstOrDefault(m => m.Module!.ModuleId == module.ModuleId);
                if (moduleToUpdate != null)
                {
                    moduleToUpdate.IsUnlocked = true;
                    moduleToUpdate.IsCompleted = false;
                    await courseService.OpenModuleAsync(currentUserId, module.ModuleId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Shows notification for failed module purchase
        /// </summary>
        private void ShowPurchaseFailedNotification()
        {
            notificationHelper!.ShowTemporaryNotification("You do not have enough coins to buy this module.");
        }
        #endregion
    }
}

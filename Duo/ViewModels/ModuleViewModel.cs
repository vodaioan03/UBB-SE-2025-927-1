using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Services;
using Windows.System;

namespace Duo.ViewModels
{
    public partial class ModuleViewModel : BaseViewModel, IModuleViewModel
    {
        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;
        private readonly ICourseViewModel courseViewModel;
        public Module CurrentModule { get; set; }
        public bool IsCompleted { get; set; }
        public ICommand CompleteModuleCommand { get; set; }
        private int UserId { get; set; }

        public ICommand ModuleImageClickCommand { get; set; }

        public ModuleViewModel(Models.Module module, ICourseViewModel courseVM,
                    ICourseService? courseServiceOverride = null,
                    ICoinsService? coinsServiceOverride = null)
        {
            courseService = courseServiceOverride ?? new CourseService(new CourseServiceProxy(new System.Net.Http.HttpClient()));
            coinsService = coinsServiceOverride ?? new CoinsService(new CoinsServiceProxy(new System.Net.Http.HttpClient()));

            CurrentModule = module;
            courseViewModel = courseVM;

            CompleteModuleCommand = new RelayCommand(ExecuteCompleteModule, CanCompleteModule);
            ModuleImageClickCommand = new RelayCommand(HandleModuleImageClick);

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            IsCompleted = await courseService.IsModuleCompletedAsync(UserId, CurrentModule.ModuleId);
            await courseService.OpenModuleAsync(UserId, CurrentModule.ModuleId);

            courseViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(courseViewModel.FormattedTimeRemaining))
                {
                    OnPropertyChanged(nameof(TimeSpent));
                }
            };
        }

        public async Task HandleModuleImageClick(object? obj)
        {
            bool confirmStatus = await courseService.ClickModuleImageAsync(UserId, CurrentModule.ModuleId);
            if (confirmStatus)
            {
                OnPropertyChanged(nameof(CoinBalance));
            }
        }

        public string TimeSpent => courseViewModel.FormattedTimeRemaining;

        private int coinBalance;
        public int CoinBalance
        {
            get => coinBalance;
            private set
            {
                coinBalance = value;
                OnPropertyChanged(nameof(CoinBalance));
            }
        }

        // Async method to load and update the CoinBalance
        public async Task LoadCoinBalanceAsync()
        {
            CoinBalance = await coinsService.GetCoinBalanceAsync(0);
        }

        private bool CanCompleteModule(object? parameter)
        {
            return !IsCompleted;
        }

        private void ExecuteCompleteModule(object? parameter)
        {
            courseViewModel.MarkModuleAsCompletedAndCheckRewards(CurrentModule.ModuleId);
            IsCompleted = true;
            OnPropertyChanged(nameof(IsCompleted));
            courseViewModel.RefreshCourseModulesDisplay();
        }

        public async Task ExecuteModuleImageClick(object? obj)
        {
            bool confirmStatus = await courseService.ClickModuleImageAsync(UserId, CurrentModule.ModuleId);
            if (confirmStatus)
            {
                OnPropertyChanged(nameof(CoinBalance));
                courseViewModel.RefreshCourseModulesDisplay();
            }
        }
    }
}

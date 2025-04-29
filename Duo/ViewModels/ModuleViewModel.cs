using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using Duo.Models;
using Duo.Services;

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

        public ICommand ModuleImageClickCommand { get; set; }

        public ModuleViewModel(Models.Module module, ICourseViewModel courseVM,
            ICourseService? courseServiceOverride = null,
            ICoinsService? coinsServiceOverride = null)
        {
            courseService = courseServiceOverride ?? new CourseService(new CourseServiceProxy(new System.Net.Http.HttpClient()));
            coinsService = coinsServiceOverride ?? new CoinsService(new CoinsServiceProxy(new System.Net.Http.HttpClient()));

            CurrentModule = module;

            // Fix for CS0029: Await the asynchronous method to get the result
            IsCompleted = courseService.IsModuleCompletedAsync(0, module.ModuleId).GetAwaiter().GetResult();

            CompleteModuleCommand = new RelayCommand(ExecuteCompleteModule, CanCompleteModule);
            ModuleImageClickCommand = new RelayCommand(HandleModuleImageClick);
            courseViewModel = courseVM;

            courseService.OpenModuleAsync(0, module.ModuleId);

            courseViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(courseViewModel.FormattedTimeRemaining))
                {
                    OnPropertyChanged(nameof(TimeSpent));
                }
            };

            courseService.OpenModuleAsync(0, module.ModuleId);
        }

        public void HandleModuleImageClick(object? obj)
        {
            var confirmStatus = courseService.ClickModuleImageAsync(0, CurrentModule.ModuleId).GetAwaiter().GetResult();
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
        public void ExecuteModuleImageClick(object? obj)
        {
            if (courseService.ClickModuleImageAsync(0, CurrentModule.ModuleId).GetAwaiter().GetResult())
            {
                OnPropertyChanged(nameof(CoinBalance));
                courseViewModel.RefreshCourseModulesDisplay();
            }
        }
    }
}

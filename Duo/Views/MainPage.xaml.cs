using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Duo.Models;
using Duo.Services;
using Duo.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Duo.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class MainPage : Page
    {
        // keep this static so that the dialog is only shown once. The page is recreated every time it is navigated to.
        private static bool isDialogShown = false;

        public MainPage()
        {
            this.InitializeComponent();

            // Create an HttpClient instance (assuming you're not using Dependency Injection)
            HttpClient httpClient = new HttpClient();

            // Create an instance of ServiceProxy
            var serviceProxy = new CoinsServiceProxy(httpClient);

            var courseServiceProxy = new CourseServiceProxy(httpClient);

            // Create a CourseService instance (you can replace with your existing service)
            var courseService = new CourseService(courseServiceProxy);

            // Create a CoinsService instance, passing ServiceProxy
            var coinsService = new CoinsService(serviceProxy);

            // Set the DataContext with the updated MainViewModel constructor
            this.DataContext = new MainViewModel(
                serviceProxy, // Pass the ServiceProxy
                courseServiceProxy, // Pass the CourseServiceProxy
                courseService, // Pass the CourseService
                coinsService);

            // Set up the ItemClick event handler
            CoursesListView.ItemClick += CoursesListView_ItemClick;
        }

        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure the dialog is only shown once. Just in case.
            if (!isDialogShown)
            {
                isDialogShown = true;

#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
                bool dailyLoginRewardEligible = await (this.DataContext as MainViewModel)!.TryDailyLoginReward();
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly

                if (dailyLoginRewardEligible)
                {
                    ContentDialog welcomeDialog = new ContentDialog
                    {
                        Title = "Welcome!",
                        Content = "You have been granted the daily login reward! 100 coins Just for you <3",
                        CloseButtonText = "Cheers!",
                        XamlRoot = RootGrid.XamlRoot
                    };
                    await welcomeDialog.ShowAsync();
                }
            }
        }

        private void CoursesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Course selectedCourse)
            {
                var courseVM = new CourseViewModel(selectedCourse);
                this.Frame.Navigate(typeof(CoursePage), courseVM);
            }
        }
    }
}

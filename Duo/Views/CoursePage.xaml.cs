using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Models;
using Duo.ViewModels;
using Microsoft.UI.Xaml.Navigation;
using System;

#pragma warning disable CS8602
#pragma warning disable IDE0059

namespace Duo.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class CoursePage : Page
    {
        private CourseViewModel? viewModel;

        private int CurrentUserId { get; init; } = 1;

        public CoursePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is CourseViewModel vm)
            {
                viewModel = vm;
                this.DataContext = viewModel;
                ModulesListView.ItemClick += ModulesListView_ItemClick;
                vm.StartCourseProgressTimer();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                viewModel.PauseCourseProgressTimer(CurrentUserId);
                this.Frame.GoBack();
            }
        }

        private async void ModulesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is CourseViewModel.ModuleProgressStatus moduleDisplay && viewModel!.IsEnrolled)
            {
                if (moduleDisplay.IsUnlocked)
                {
                    this.Frame.Navigate(typeof(ModulePage), (moduleDisplay.Module, viewModel));
                    return;
                }
                try
                {
                    if (moduleDisplay.Module!.IsBonus)
                    {
                        await viewModel.AttemptBonusModulePurchaseAsync(moduleDisplay.Module, CurrentUserId);
                    }
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"An error occurred while attempting to unlock the module: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
            }
        }
    }
}

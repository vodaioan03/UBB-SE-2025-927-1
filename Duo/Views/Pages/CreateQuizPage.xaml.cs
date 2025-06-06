using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Duo.Views.Components.Modals;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateQuizPage : Page
    {
        public CreateQuizPage()
        {
            this.InitializeComponent();
            ViewModel.ShowListViewModal += (exercises) =>
            {
                this.DispatcherQueue.TryEnqueue(() =>
                {
                    ViewModel_openSelectExercises(exercises);
                });
            };
            ViewModel.RequestGoBack += ViewModel_RequestGoBack;
            ViewModel.ShowErrorMessageRequested += ViewModel_ShowErrorMessageRequested;
        }
        private async void ViewModel_ShowErrorMessageRequested(object sender, (string Title, string Message) e)
        {
            await ShowErrorMessage(e.Title, e.Message);
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            try
            {
                var dialog = new ContentDialog
                {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to show error dialog: {ex.Message}");
            }
        }

        public void ViewModel_RequestGoBack(object sender, EventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void ViewModel_openSelectExercises(List<Exercise> exercises)
        {
            var dialog = new ContentDialog
            {
                Title = "Select Exercise",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var listView = new ListView
            {
                ItemsSource = exercises,
                SelectionMode = ListViewSelectionMode.Single,
                MaxHeight = 300,
                ItemTemplate = (DataTemplate)Resources["ExerciseSelectionItemTemplate"]
            };

            dialog.Content = listView;
            dialog.PrimaryButtonText = "Add";
            dialog.IsPrimaryButtonEnabled = false;

            listView.SelectionChanged += (s, args) =>
            {
                dialog.IsPrimaryButtonEnabled = listView.SelectedItem != null;
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && listView.SelectedItem is Exercise selectedExercise)
            {
                ViewModel.AddExercise(selectedExercise);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Http;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duo.Data;
using Duo.ViewModels.ExerciseViewModels;
using Duo.Services;
using Duo.Repositories;
using Duo.ViewModels;
using Duo.ViewModels.Roadmap;
using Duo.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Duo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider;
        private Window? window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<DatabaseConnection>();

            services.AddSingleton<IUserRepository, UserRepository>();

            // User
            services.AddSingleton<IUserServiceProxy>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new UserServiceProxy(httpClient);
            });
            services.AddSingleton<IUserService>(sp =>
            {
                var proxy = sp.GetRequiredService<IUserServiceProxy>();
                return new UserService(proxy);
            });

            // Exercise
            services.AddSingleton<ExerciseServiceProxy>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new ExerciseServiceProxy(httpClient);
            });
            services.AddSingleton<IExerciseService>(sp =>
                {
                var proxy = sp.GetRequiredService<ExerciseServiceProxy>();
                return new ExerciseService(proxy);
            });

            services.AddSingleton<IExamRepository, ExamRepository>();
            services.AddSingleton<IQuizRepository, QuizRepository>();

            // Quiz
            services.AddSingleton<QuizServiceProxy>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new QuizServiceProxy(httpClient);
            });
            services.AddSingleton<IQuizService>(sp =>
            {
                var proxy = sp.GetRequiredService<QuizServiceProxy>();
                return new QuizService(proxy);
            });

            services.AddSingleton<ISectionRepository, SectionRepository>();

            // Section
            services.AddSingleton<SectionServiceProxy>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new SectionServiceProxy(httpClient);
            });
            services.AddSingleton<ISectionService>(sp =>
            {
                var proxy = sp.GetRequiredService<SectionServiceProxy>();
                return new SectionService(proxy);
            });

            services.AddSingleton<IRoadmapRepository, RoadmapRepository>();

            // Roadmap
            services.AddSingleton<RoadmapServiceProxy>(
                sp =>
                {
                    var httpClient = sp.GetRequiredService<HttpClient>();
                    return new RoadmapServiceProxy(httpClient);
                });
            services.AddSingleton<IRoadmapService>(
                sp =>
                {
                    var proxy = sp.GetRequiredService<RoadmapServiceProxy>();
                    return new RoadmapService(proxy);
                });

            services.AddSingleton<IExerciseViewFactory, ExerciseViewFactory>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IExerciseRepository, ExerciseRepository>();
            services.AddSingleton<IExamRepository, ExamRepository>();
            services.AddSingleton<IQuizRepository, QuizRepository>();
            services.AddSingleton<ISectionRepository, SectionRepository>();
            services.AddSingleton<IRoadmapRepository, RoadmapRepository>();

            services.AddTransient<FillInTheBlankExerciseViewModel>();
            services.AddTransient<MultipleChoiceExerciseViewModel>();
            services.AddTransient<AssociationExerciseViewModel>();
            services.AddTransient<ExerciseCreationViewModel>();
            services.AddTransient<QuizExamViewModel>();
            services.AddTransient<CreateQuizViewModel>();
            services.AddTransient<CreateSectionViewModel>();

            services.AddSingleton<RoadmapMainPageViewModel>();
            services.AddTransient<RoadmapSectionViewModel>();
            services.AddSingleton<RoadmapQuizPreviewViewModel>();

            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            window = new MainWindow();
            window.Activate();
        }
    }
}

﻿using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Boxes.Tests
{
    /// <summary>
    ///     Fournit un comportement spécifique à l'application afin de compléter la classe Application
    ///     par défaut.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        ///     Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code
        ///     créé à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        ///     Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points
        ///     d'entrée seront utilisés par exemple au moment du lancement de l'application pour l'ouverture
        ///     d'un fichier spécifique.
        /// </summary>
        /// <param name="e">
        ///     Détails concernant la requête et le processus de lancement.
        /// </param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'état de l'application précédemment suspendue
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            
            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.CreateDefaultUI();

            // Placez le frame dans la fenêtre active
            Window.Current.Activate();

            Microsoft.VisualStudio.TestPlatform.TestExecutor.UnitTestClient.Run(e.Arguments);
        }

        /// <summary>
        ///     Appelé lorsque la navigation vers une page donnée échoue
        /// </summary>
        /// <param name="sender">
        ///     Frame à l'origine de l'échec de navigation.
        /// </param>
        /// <param name="e">
        ///     Détails relatifs à l'échec de navigation.
        /// </param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        ///     Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est
        ///     enregistré sans savoir si l'application pourra se fermer ou reprendre sans endommager
        ///     le contenu de la mémoire.
        /// </summary>
        /// <param name="sender">
        ///     Source de la requête de suspension.
        /// </param>
        /// <param name="e">
        ///     Détails de la requête de suspension.
        /// </param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'état de l'application et arrêtez toute activité en arrière-plan
            deferral.Complete();
        }
    }
}

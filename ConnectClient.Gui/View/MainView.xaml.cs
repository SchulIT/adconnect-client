﻿using ModernWpf.Controls;
using System.Linq;
using System.Windows;

namespace ConnectClient.Gui.View
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            navigationView.SelectedItem = navigationView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem == null)
            {
                return;
            }

            var targetPage = selectedItem.Tag switch
            {
                "provision" => typeof(ProvisionPage),
                "remove" => typeof(RemovePage),
                "settings" => typeof(SettingsPage),
                "about" => typeof(AboutPage),
                _ => null
            };

            if(targetPage != null)
            {
                frame.Navigate(targetPage);
            }
        }
    }
}

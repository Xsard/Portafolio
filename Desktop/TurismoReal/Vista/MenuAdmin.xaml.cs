﻿using System;
using System.Data;
using System.Windows;
using Vista.Pages;

namespace Vista
{
    public partial class MenuAdmin : Window
    {
        private DataTable dt;
        public MenuAdmin(DataTable admin)
        {
            InitializeComponent();
            dt = admin;
            Default();
        }
        #region Barra de navegación
        private void Default()
        {
            MainMenu mainMenu = new(dt);
            PagesNavigation.Navigate(mainMenu);
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new(dt);
            PagesNavigation.Navigate(mainMenu);
        }
        private void btnDpto_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorDpto.xaml", UriKind.RelativeOrAbsolute));

        }
        private void btnServE_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorServExtras.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorUsuario.xaml", UriKind.RelativeOrAbsolute));
        }
        private void btnDisponibilidad_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Pages/MantenedorDisponibilidad.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion
    }
}

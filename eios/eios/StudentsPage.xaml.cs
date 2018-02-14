﻿using eios.Data;
using eios.Model;
using eios.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        StudentsListViewModel ViewModel { get; set; }
        OccupationsListViewModel OccupViewModel { get; set; }

        Occupation occupation;

        public StudentsPage(OccupationsListViewModel occupViewModel, Occupation occupation)
        {
            InitializeComponent();

            OccupViewModel = occupViewModel;
            ViewModel = new StudentsListViewModel(occupation);
            BindingContext = ViewModel;

            this.occupation = occupation;
            unavaibleButton.IsEnabled = occupation.IdLesson != 0;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is StudentSelect item)
                {
                    item.IsSelected = !item.IsSelected;
                    ViewModel.OnSite = ViewModel.StudentsList.FindAll(s => s.IsSelected.Equals(false)).Count;
                }
            };

        }

        async void OnUnaviableClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    await WebApi.Instance.SetNullAttendAsync(occupation);
                    await App.Database.SetSentFlag(occupation.IdOccupation, idGroup);
                }
                catch (HttpRequestException)
                {
                    await App.Database.DeleteAttendance(occupation.IdOccupation, idGroup);
                    await Navigation.PopAsync();
                    return;
                }
                Navigation.InsertPageBefore(new CompletedOccupationPage(OccupViewModel, this.occupation), this);
                await Navigation.PopAsync();
            }
            await Navigation.PopAsync();
        }

        async Task OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var idGroup = (int) App.Current.Properties["IdGroupCurrent"];
            await App.Database.SetAttendence(ViewModel.StudentsList, occupation.IdOccupation, idGroup);

            if (CrossConnectivity.Current.IsConnected)
            {
                var students = await App.Database.GetAbsentStudents(occupation.IdOccupation, occupation.IdGroup);
                try
                {
                    await WebApi.Instance.SetAttendAsync(students, occupation);
                    await App.Database.SetSentFlag(occupation.IdOccupation, idGroup);
                    await OccupViewModel.UpdateState();
                }
                catch (HttpRequestException)
                {
                    await App.Database.DeleteAttendance(occupation.IdOccupation, idGroup);

                    await Navigation.PopAsync();
                    return;
                }
            }

            Navigation.InsertPageBefore(new CompletedOccupationPage(OccupViewModel, this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}
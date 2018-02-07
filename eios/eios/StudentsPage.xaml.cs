﻿using eios.Data;
using eios.Model;
using eios.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        StudentsListViewModel viewModel;

        Occupation occupation;

        public StudentsPage(Occupation occupation)
        {
            InitializeComponent();

            viewModel = new StudentsListViewModel(occupation);
            BindingContext = viewModel;

            this.occupation = occupation;

            studentListView.ItemTapped += (sender, e) =>
            {
                studentListView.SelectedItem = null;

                if (e.Item is StudentSelect item)
                {
                    item.IsSelected = !item.IsSelected;
                    viewModel.OnSite = viewModel.StudentsList.FindAll(s => s.IsSelected.Equals(true)).Count;
                }
            };

        }

        async void OnUnaviableClicked(Object sender, AssemblyLoadEventArgs args)
        {
            await Navigation.PopAsync();
        }

        async Task OnMarkClicked(Object sender, AssemblyLoadEventArgs args)
        {
            var selectedList = viewModel.StudentsList.FindAll(s => s.IsSelected.Equals(true));
            var resultList = new List<StudentAbsent>();
            foreach (Student st in selectedList)
            {
                var cache = new StudentAbsent()
                {
                    Id = st.Id
                };
                resultList.Add(cache);
            }
            await WebApi.Instance.SetAttendAsync(this.occupation.IdOccupation, resultList);
            Navigation.InsertPageBefore(new CompletedOccupationPage(this.occupation), this);
            await Navigation.PopAsync();
        }
    }
}
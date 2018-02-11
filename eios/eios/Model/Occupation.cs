﻿using eios;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace eios.Model
{
    [Table("Occupations")]
    public class Occupation : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        [Column("id_group")]
        public int IdGroup { get; set; }

        [JsonProperty("id_occup"), Column("id_occup")]
        public int IdOccupation { get; set; }

        [JsonProperty("id_lesson"), Column("lesson_id")]
        public int IdLesson { get; set; }

        [JsonProperty("lesson_name"), Column("lesson_name")]
        public string Name { get; set; }

        [JsonProperty("id_aud"), Column("id_aud")]
        public int IdAud { get; set; }

        [JsonProperty("aud"), Column("aud")]
        public string Aud { get; set; }

        [Column("is_check")]
        public bool IsChecked { get; set; }
        
        [Column("is_block")]
        public bool IsBlocked { get; set; }
        
        [Column("is_sent")]
        public bool IsSent { get; set; }

        [Ignore]
        public string CircleColor
        {
            get
            {
                if (!IsChecked && !IsBlocked && IdLesson != 0 && IdOccupation < App.IdOccupNow) { return "#f7636c"; }
                else if (IsChecked) { return "#acd94e"; }
                return "#e0e0e0";
            }
        }

        [Ignore]
        public Type TargetType
        {
            get
            {
                if (!IsChecked && !IsBlocked && IdLesson != 0 && IdOccupation < App.IdOccupNow) { return typeof(StudentsPage); }
                else if (IsChecked) { return typeof(CompletedOccupationPage); }
                return null;
            }
        }

        [Ignore]
        public string Time
        {
            get
            {
                switch (IdOccupation)
                {
                    case 1: return "8:00";
                    case 2: return "9:50";
                    case 3: return "11:40";
                    case 4: return "13:45";
                    case 5: return "15:35";
                    case 6: return "17:25";
                    case 7: return "19:10";
                    case 8: return "20:55";
                    default: return "-";
                }
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

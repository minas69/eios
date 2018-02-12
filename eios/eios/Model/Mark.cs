﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eios.Model
{
    public class Mark
    {
        [JsonProperty("id_occup")]
        public int Id { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; set; }
    }
}

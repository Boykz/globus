﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntGlobus.ViewModels
{
    public class ResetNumViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string NewTelNum { get; set; }

    }
}

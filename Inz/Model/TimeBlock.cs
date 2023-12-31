﻿using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class TimeBlock
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public string StartHour { get; set; } = null!;
        [MaxLength(10)]
        public string EndHour { get; set; } = null!;
        public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
    }
}

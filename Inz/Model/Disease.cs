﻿using System.ComponentModel.DataAnnotations;

namespace Inz.Model
{
    public class Disease
    {
        public Disease() { }
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        public ICollection<CuredDisease> CuredDiseases { get; set; } = new List<CuredDisease>();
        public ICollection<DiseaseSuspicion> DiseaseSuspicions { get; set; } = new List<DiseaseSuspicion>();
    }
}

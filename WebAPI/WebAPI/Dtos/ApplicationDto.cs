﻿using WebAPI.Entities;

namespace WebAPI.Dtos
{
    public class ApplicationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}

﻿using System.Collections.Generic;

namespace NLemos.Api.Identity.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<string> Claims { get; set; }

        public User()
        {
            Claims = new List<string>();
        }
    }
}

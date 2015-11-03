﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Obligatorisk1.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public IDbSet<Component> Components { get; set; }
        public IDbSet<LoanInformation> LoanInformations { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<SpecificComponent> SpecificComponents { get; set; }

        public System.Data.Entity.DbSet<Obligatorisk1.Models.Category> Categories { get; set; }
    }
}
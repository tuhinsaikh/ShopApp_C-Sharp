﻿using ShopWeb.Data;
using ShopWeb.DataAccess.Repository.IRepository;
using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository

    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        void ICategoryRepository.Update(Category category)
        {
            _db.Update(category);
        }
    }
}

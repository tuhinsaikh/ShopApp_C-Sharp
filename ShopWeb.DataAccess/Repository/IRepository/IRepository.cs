﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> predicate, string? includeProperties = null);
        void Remove(T entity);
        void Add(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> predicate, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = dbSet.Where(predicate);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProps in includeProperties.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProps);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(string.IsNullOrEmpty(includeProperties)) return query.ToList();
            else
            {
                foreach(var includeProps in includeProperties.Split(new Char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProps);
                }
                return query.ToList();
            }

            //IEnumerable<T> query = dbSet;
            //return query.ToList();
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
    }
}

﻿using HotelManagement.Core.IRepositories;
using HotelManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelDbContext _hotelDbContext;
        protected DbSet<T> _dbSet;

        public GenericRepository (HotelDbContext hotelDbContext)
	    {
            _hotelDbContext = hotelDbContext;
            _dbSet = hotelDbContext.Set<T>();
	    }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public  async Task DeleteAsync<T>(T Value)
        {
            var entity = await _dbSet.FindAsync(Value);
            EntityEntry entityEntry = _dbSet.Remove(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();

        }


        public async Task<T> GetByIdAsync(T Value) => await _dbSet.FindAsync(Value);

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task UpdateAsync<T>(T Value, T entity)
        {
            var entityUpdate = await _dbSet.FindAsync(Value);
            EntityEntry entityEntry = _dbSet.Update(entityUpdate);
            entityEntry.State = EntityState.Modified;
        } 
    }
}

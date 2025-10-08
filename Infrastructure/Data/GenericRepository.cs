using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext contex) : IGenericRepository<T> where T : BaseEntity
{
    private readonly StoreContext _context = contex;

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public bool Exist(int id)
    {

        return _context.Set<T>().Any(x => x.Id == id);
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<bool> saveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<T?> GetEntitiesWitchSpec(ISpecification<T> specification)
    {
        return await ApplySpecication(specification).FirstOrDefaultAsync();
    }
    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification)
    {
        return await ApplySpecication(specification).ToListAsync();
    }
    public async Task<TResult?> GetEntitiesWitchSpec<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecication(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecication(specification).ToListAsync();
    }


    private IQueryable<T> ApplySpecication(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }
    private IQueryable<TResult> ApplySpecication<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(_context.Set<T>().AsQueryable(), specification);
    }

}
using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();

    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> saveAllAsync();
    bool Exist(int id);
    Task<T?> GetEntitiesWitchSpec(ISpecification<T> specification);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
    Task<TResult?> GetEntitiesWitchSpec<TResult>(ISpecification<T,TResult> specification);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification);
}

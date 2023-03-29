using System.Linq.Expressions;
using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Data.Repositories;

public interface IRepository<T> where T : DomainObject
{
    //T? GetById(int id);
    //IQueryable<T> GetAll();
    //void InsertOrUpdate(T? entity);
    //void Delete(T? entity);
    //void UpdateValues(T originalEntity, T modifiedEntity);

    T Add(T entity);
    T Update(T entity);
    T? Get(int id);
    IEnumerable<T> All();

    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    //void SaveChanges();
    void Remove(T entity);
    
}
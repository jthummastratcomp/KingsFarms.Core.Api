using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Data.Repositories;

public interface IRepository<T> where T : DomainObject
{
    T? GetById(int id);
    IQueryable<T> GetAll();
    void InsertOrUpdate(T? entity);
    void Delete(T? entity);
    void UpdateValues(T originalEntity, T modifiedEntity);
}
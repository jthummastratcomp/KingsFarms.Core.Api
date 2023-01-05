using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Repositories;

public class Repository<T> : IRepository<T> where T : DomainObject
{
    private readonly IUnitOfWork _unitOfWork;

    public Repository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public T? GetById(int id)
    {
        return _unitOfWork.Context.GetSet<T>().FirstOrDefault(x => x.Id == id);
    }

    public IQueryable<T> GetAll()
    {
        return _unitOfWork.Context.GetSet<T>();
    }

    public void InsertOrUpdate(T? entity)
    {
        if (entity != null) _unitOfWork.Context.GetEntry(entity).State = entity.Id == 0 ? EntityState.Added : EntityState.Modified;
    }

    public void Delete(T? entity)
    {
        if (entity != null) _unitOfWork.Context.GetSet<T>().Remove(entity);
    }

    public void UpdateValues(T originalEntity, T modifiedEntity)
    {
        _unitOfWork.Context.GetEntry(originalEntity).CurrentValues.SetValues(modifiedEntity);
    }
}
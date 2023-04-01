using System.Linq.Expressions;
using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Data.Repositories;

public class Repository<T> : IRepository<T> where T : DomainObject
{
    protected readonly KingsFarmsDbContext _context;
    //private readonly IUnitOfWork _unitOfWork;

    //public Repository(IUnitOfWork unitOfWork)
    //{
    //    _unitOfWork = unitOfWork;
    //}

    protected Repository(KingsFarmsDbContext context)
    {
        _context = context;
    }


    //public T? GetById(int id)
    //{
    //    //return _unitOfWork.Context.GetSet<T>().FirstOrDefault(x => x.Id == id);
    //    return _context.GetSet<T>().FirstOrDefault(x => x.Id == id);
    //}

    //public IQueryable<T> GetAll()
    //{
    //    return _unitOfWork.Context.GetSet<T>();
    //}

    //public void InsertOrUpdate(T? entity)
    //{
    //    if (entity != null) _unitOfWork.Context.GetEntry(entity).State = entity.Id == 0 ? EntityState.Added : EntityState.Modified;
    //}

    //public void Delete(T? entity)
    //{
    //    if (entity != null) _unitOfWork.Context.GetSet<T>().Remove(entity);
    //}

    //public void UpdateValues(T originalEntity, T modifiedEntity)
    //{
    //    _unitOfWork.Context.GetEntry(originalEntity).CurrentValues.SetValues(modifiedEntity);
    //}
    public virtual T Add(T entity)
    {
        return _context.Add(entity).Entity;
    }

    public virtual T Update(T entity)
    {
        return _context.Update(entity).Entity;
    }

    public virtual T? Get(int id)
    {
        return _context.Find<T>(id);
    }

    public virtual IEnumerable<T> All()
    {
        return _context.Set<T>().ToList();
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().AsQueryable().Where(predicate).ToList();
    }

    public virtual void Remove(T entity)
    {
        _context.Remove(entity);
    }

    //public virtual void SaveChanges()
    //{
    //    _context.SaveChanges();
    //}
}
using System.ComponentModel.DataAnnotations;
using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Repositories;

namespace KingsFarms.Core.Api.Data.Providers;

public abstract class BaseDataProvider<T> where T : DomainObject
{
    private readonly IRepository<T> _repository;
    private readonly IUnitOfWork _unitOfWork;


    protected BaseDataProvider(IRepository<T> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Remove(T? currentDomainObject)
    {
        _repository.Delete(currentDomainObject);
        _unitOfWork.Save();
    }

    public virtual T? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public int Save(T? modifiedDomainObject)
    {
        if (modifiedDomainObject == null) return 0;

        var domainObject = GetByIdPreSave(modifiedDomainObject);

        if (domainObject == null)
            _repository.InsertOrUpdate(modifiedDomainObject);
        else
            _repository.UpdateValues(domainObject, modifiedDomainObject);


        return Save();
    }

    protected virtual T? GetByIdPreSave(T? modifiedDomainObject)
    {
        return _repository.GetById(modifiedDomainObject.Id);
    }

    private int Save()
    {
        int id;
        try
        {
            id = _unitOfWork.Save();
        }
        catch (ValidationException ex)
        {
            throw ex;
        }

        return id;
    }
}
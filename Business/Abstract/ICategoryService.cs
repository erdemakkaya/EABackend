using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>>  GetAll();
       IDataResult<Category> GetById(int id);
        IResult Add(Category product);
        IResult Update(Category product);
        IResult Delete(Category product);
        void TransactionalOperation(Category product1, Category product2);
    }
}
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public IResult Add(Category Category)
        {
             _categoryDal.Add(Category);
            return new SuccessResult(Messages.ProductUpdated);
        }
        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessResultData<List<Category>>(_categoryDal.GetList());

        }
        public IDataResult<Category> GetById(int id)
        {
            return new SuccessResultData<Category>(_categoryDal.Get(p => p.CategoryId == id));
        }

        public IDataResult<List<Category>> GetByCategoryId(int categoryId)
        {
            return new SuccessResultData<List<Category>>(_categoryDal.GetList(p => p.CategoryId == categoryId).ToList());
        }
        public IResult Update(Category Category)
        {
            //ValidatorTool.FluentValidate(new ProductValidator(), Category);
             _categoryDal.Update(Category);
            return   new SuccessResult(Messages.ProductUpdated);
        }

    public IResult Delete(Category Category)
    {
        _categoryDal.Delete(Category);
       return new SuccessResult(Messages.ProductDeleted);
        }

    public void TransactionalOperation(Category category1, Category category2)
    {
        throw new NotImplementedException();
    }
}
}
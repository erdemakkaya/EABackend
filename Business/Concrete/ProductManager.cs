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
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public IResult Add(Product product)
        {
             _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessResultData<List<Product>>(_productDal.GetList());

        }
        public IDataResult<Product> GetById(int id)
        {
            return new SuccessResultData<Product>(_productDal.Get(p => p.ProductId == id));
        }

        public IDataResult<List<Product>> GetByCategoryId(int categoryId)
        {
            return new SuccessResultData<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }
        public IResult Update(Product product)
        {
            //ValidatorTool.FluentValidate(new ProductValidator(), product);
             _productDal.Update(product);
            return   new SuccessResult(Messages.ProductUpdated);
        }

    public IResult Delete(Product product)
    {
        _productDal.Delete(product);
       return new SuccessResult(Messages.ProductDeleted);
        }

    public void TransactionalOperation(Product product1, Product product2)
    {
        throw new NotImplementedException();
    }
}
}
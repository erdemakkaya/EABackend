using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EFEntityRepositoryBase<Product, EAContext>, IProductDal
    {
        //public List<ProductDetail> GetProductDetails()
        //{
        //    using (EAContext ctx = new EAContext())
        //    {
        //        var result = from p in ctx.Products
        //                     join c in ctx.Categories on int.Parse(p.CategoryId) equals c.CategoryId
        //                     select new ProductDetail
        //                     {
        //                         ProductId = p.ProductId,
        //                         ProductName = p.ProductName,
        //                         CategoryName = c.CategoryName
        //                     };
        //        return result.ToList();
        //    }
        //}
    }
}

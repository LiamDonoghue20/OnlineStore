

using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T> 
    {
        //Criteria = what criteria we are using to search  on (such as WHERE productID = Criteria)
        Expression<Func<T, bool>> Criteria {get; }
        //Includes = what we include when searching (such as including a product type and brand with a product)
        List<Expression<Func<T, object>>> Includes {get;}
        Expression<Func<T, object>> OrderBy {get; }
        Expression<Func<T, object>> OrderByDescending {get; }
        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled {get;}

    }
}
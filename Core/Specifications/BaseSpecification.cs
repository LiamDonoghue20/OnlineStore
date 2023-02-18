
using System.Linq.Expressions;


namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        //constructor for when we want to create a specification without any criteria
        public BaseSpecification()
        {

        }
        //and if we want one with a criteria
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
         
        }
        public Expression<Func<T, bool>> Criteria {get; }
        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();
        //allow the use of includes in our functions
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

    }
}
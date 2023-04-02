using Core.Entities;
using Core.Interfaces;
using Infrastructue.Data;
using System.Collections;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        //store all the repositories in a hashtable to access them here
        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //check to see if there is already a hashtable created for repositories, if not create one
            if(_repositories == null) _repositories = new Hashtable();
            //check the type of entity (ie products, brands, orders, basket)
            var type = typeof(TEntity).Name;
            //check if this type currently exists in the hashtable
            if(!_repositories.ContainsKey(type))
            {
                //if the type doesnt exist in the hashtable, create a generic repository
                var repositoryType = typeof(GenericRepository<>);
                //then create an instance of that type 
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(
                    TEntity)), _context
                );
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}
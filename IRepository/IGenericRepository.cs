using Trustesse_Assessment.Model;
using System.Linq.Expressions;
using X.PagedList;


namespace Trustesse_Assessment.IRepository
{
   public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task Insert(T item);
        Task InsertRamge(IEnumerable<T> item);
        Task Delete(int id);
        void DeleteRamge(IEnumerable<T> item);
        void Update(T item);

        //Note: Item could be "entity" or what ever you chose to name your model propertiies
    }
}


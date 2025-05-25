using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Trustesse_Assessment.Data;
using Trustesse_Assessment.IRepository;
using X.PagedList;

namespace Trustesse_Assessment.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Delete(int id)
        {
            var item = await _dbSet.FindAsync(id);
            //if (item == null)
            //{
            //    return 
            //}
            _dbSet.Remove(item);
        }

        public void DeleteRamge(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            //note that "expression" is for getting an item by its specification either by name , id or shortName etc
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var includePropertry in includes)
                {
                    query = query.Include(includePropertry);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }


        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = default, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (var includePropertry in includes)
                {
                    query = query.Include(includePropertry);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }


        public async Task Insert(T item)
        {
            await _dbSet.AddAsync(item);
        }

        public async Task InsertRamge(IEnumerable<T> item)
        {
            await _dbSet.AddRangeAsync(item);
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}

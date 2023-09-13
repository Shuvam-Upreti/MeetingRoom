using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;

namespace MeetingRoom.Repository.IRepository
{
    public interface IRepository <T> where T : class
    {
        T GetFirstorDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}

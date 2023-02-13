using AGAddressRavenDB.PersonDetail;
using Microsoft.Extensions.Caching.Memory;

namespace AGAddressRavenDB.Persistence
{
    public interface IRepository<T>
    {
        public IEnumerable<T> Get(string _Fname,string _Lname);
        public IEnumerable<T> GetAll(int pageSize, int pageNumber);
        public void InsertorUpdate(T element);

        public void Delete(string _Fname, string _Lname);
    }
}
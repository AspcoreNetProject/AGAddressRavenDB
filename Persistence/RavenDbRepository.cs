using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using AGAddressRavenDB.Persistence;
using AGAddressRavenDB.PersonDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Raven.Client.Documents.Linq;


namespace AGAddressRavenDB.Controllers
{
    public partial class PersonController
    {
        public class RavenDbRepository<T> : IRepository<T>
        {
            private readonly IRavenDBContext _context;
           
            public RavenDbRepository(IRavenDBContext conext)
            {
                _context= conext;
            }
            public IEnumerable<T> Get(string _FName, string _LName)
            {
               using var _session = _context.store.OpenSession();
                

                IRavenQueryable<Person> query = from person in _session.Query<Person>()
                                                where person.FirstName == _FName
                                                && person.LastName == _LName
                                                select person;

                List<Person> elements = query.ToList();

                return (IEnumerable<T>)elements;
              
            }

            public IEnumerable<T> GetAll(int pageSize, int pageNumber)
            {
                using var _session = _context.store.OpenSession();
                var elements = _session.Query<T>()
                    .Skip(pageSize*( pageNumber-1))
                    .Take(pageSize);

                return elements;
            }

            public void InsertorUpdate(T element)
            {
               
                
                using var _session = _context.store.OpenSession();

                List<Person> all = _session.Query<Person>().ToList();
                Person _Perdet = element as Person;
               
                 var newList= all.Where(x => x.FirstName == _Perdet.FirstName.ToLower() 
                                && x.LastName == _Perdet.LastName.ToLower()).ToList();
                if(newList.Count != 0 )
                {
                    //Updates the address if the Fname and Lname are same 
                    
                    //_persondet gets the record with firstname and last name entered and updates the address
                    var _persondet = _session.Load<Person>(_session.Advanced.GetDocumentId(newList.FirstOrDefault()));
                    
                    _persondet.Address = _Perdet.Address;
                    _session.SaveChanges();
                }
                else
                {
                    // Inserts the new record
                    _session.Store(element);
                     _session.SaveChanges();
                }
            
            }
            public void Delete(string _FName, string _LName)
            {
                using var _session = _context.store.OpenSession();
                List<Person> all = _session.Query<Person>().ToList();
                
                var newList = all.Where(x => x.FirstName == _FName.ToLower()
                               && x.LastName == _LName.ToLower()).ToList();
                if (newList.Count != 0)
                {
                    // Gets the Document Id for the Firstname and lastname
                    var docId = _session.Load<Person>(_session.Advanced.GetDocumentId(newList.FirstOrDefault()));
                    _session.Delete(docId);
                    _session.SaveChanges();
                }
                

                
            }
        }
        public class RepositoryException : Exception
        {
            public RepositoryException(string message, Exception exception) : base(message, exception)
            {

            }
        }

    }
}

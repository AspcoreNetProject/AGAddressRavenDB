using Microsoft.AspNetCore.Mvc;
using AGAddressRavenDB.PersonDetail;
using AGAddressRavenDB.Persistence;
using AutoMapper;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;

namespace AGAddressRavenDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PersonController: ControllerBase
    {
        private readonly IRepository<Person> _repository;
        private readonly ILogger<PersonController> _logger;
        private readonly IMapper _mapper;
        // inmemory caching
        private IMemoryCache _cache;
        private const string personListCacheKey = "personList";
        public PersonController(IRepository<Person> repository,
                                ILogger<PersonController> logger,
                                IMapper mapper,
                                IMemoryCache cache)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _cache= cache ?? throw new ArgumentNullException(nameof(cache)); ;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(string _Fname, string _Lname) 
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of Persons from cache.");
            if (_cache.TryGetValue(personListCacheKey, out IEnumerable<Person> persons))
            {
                _logger.Log(LogLevel.Information, "Person list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Person list not found in cache. Fetching from database.");
                persons = _repository.Get(_Fname, _Lname);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(personListCacheKey, persons, cacheEntryOptions);
            }
            //var person = _repository.Get(_Fname, _Lname);
            //var _personToGet = persons.ToList()
            //                   .Select(p => _mapper.Map<PersonToGet>(p))
            //                   .ToList();


            return Ok(persons);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertorUpdate(PersonToPut personToPut)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
          

            var _person=_mapper.Map<Person>(personToPut);
            try
            {
                //if (_cache.TryGetValue(personListCacheKey, out IEnumerable<Person> persons))
                //{
                //    _logger.Log(LogLevel.Information, "Person list found in cache.");
                //}
                //else
                //{
                //    _logger.Log(LogLevel.Information, "Person list not found in cache. Fetching from database.");
                //    persons = _repository.GetAll(1, 1);
                //    var cacheEntryOptions = new MemoryCacheEntryOptions()
                //            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                //            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                //            .SetPriority(CacheItemPriority.Normal)
                //            .SetSize(1024);
                //    _cache.Set(personListCacheKey, persons, cacheEntryOptions);
                //}

                _repository.InsertorUpdate(_person);
               
                 
                
               
            }
            catch(RepositoryException ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
            if (string.IsNullOrWhiteSpace(personToPut.FirstName) )
            {
                return CreatedAtAction(nameof(Get), new { personid = _person.FirstName });
            }

            return Ok();
        }

        [HttpDelete]
      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(string _Fname, string _Lname)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

             
            try
            {
                _repository.Delete(_Fname,_Lname);
                _cache.Remove(personListCacheKey);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                return StatusCode(500);
            }
        
            return Ok();
        }
    }
}

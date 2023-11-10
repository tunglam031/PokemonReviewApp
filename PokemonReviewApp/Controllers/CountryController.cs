using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCountries()
        {
            var categories = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(categories);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if(!_countryRepository.IsExisCountry(id))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(id));
            if(!ModelState.IsValid)
                return BadRequest();
            return Ok(country);
        }

        [HttpGet("Country/Owners")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int id)
        {
            if (!_countryRepository.IsExisCountry(id))
                return NotFound();
            var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersByCountry);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(owners);
        }
        [HttpGet("Owner/Country")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(country);
        }
    }
}

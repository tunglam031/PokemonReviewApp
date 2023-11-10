using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Collections.Generic;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IPokeCateRepository _pokeCateRepository;
        private readonly IPokeOwnerRepository _pokeOwnerRepository;
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IPokeCateRepository pokeCateRepository, IPokeOwnerRepository pokeOwnerRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _pokeCateRepository = pokeCateRepository;
            _pokeOwnerRepository = pokeOwnerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [Authorize]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
                return NotFound();
            var pokemon = _pokemonRepository.GetPokemon(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (_pokemonRepository.PokemonExists(id))
                return NotFound();
            var pokemonRating = _pokemonRepository.GetPokemonRating(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemonRating);
        }

        [HttpPost("AddPokemon")]

        public IActionResult CreatePokemon(int ownerId, int catId, PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);
            var pokemon = _pokemonRepository.GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
            if (!_pokemonRepository.CreatePokemon(pokemonMap, ownerId, catId))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(404, ModelState);
            }
            return Ok("Created Successfully");
        }
        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId,
            [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }
            var poke = _mapper.Map<Pokemon,PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            return Ok(poke);
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokeToDelete = _pokemonRepository.GetPokemon(pokeId);
            if (!_pokemonRepository.DeletePokemon(pokeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
            }
            
            //var listPokeCateToDelete = _pokeCateRepository.GetPokeCateByPokeId(pokeId);
            //var listPokeOwner = _pokeOwnerRepository.GetPokeOwnerByPokeId(pokeId);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (listPokeCateToDelete != null)
            //{
            //    foreach (var pokeCate in listPokeCateToDelete)
            //    {
            //        if(!_pokeCateRepository.DeletePokeCate(pokeCate))
            //            ModelState.AddModelError("", "Something went wrong deleting PokemonCategory");
            //    }
            //}

            //if (listPokeOwner.Count != 0)
            //{
            //    foreach (var pokeOwner in listPokeOwner)
            //    {
            //        if (_pokeOwnerRepository.DeletePokeOwner(pokeOwner))
            //            ModelState.AddModelError("", "Something went wrong deleting PokemonOwner");
            //    }
            //}

            return Ok();

        }
    }
}

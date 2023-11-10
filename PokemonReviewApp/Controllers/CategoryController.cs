﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Models.Filter;
using System.Reflection.Metadata.Ecma335;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories([FromQuery] CategoryFilter filter)
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories(filter));
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        [Authorize(Policy = "Admin")]
        public IActionResult GetCategory(int id)
        {
            if (!_categoryRepository.IsExisCategory(id))
                return NotFound();
                var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(category);
        }

        [HttpGet("{categoryId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            if (!_categoryRepository.IsExisCategory(categoryId))
                return NotFound();
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.IsExisCategory(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }

    }
}

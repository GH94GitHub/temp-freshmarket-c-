﻿using FreshMarket.Exceptions;
using FreshMarket.Models;
using FreshMarket.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreshMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishesController(DishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        [Route("")]
        public Dish[] GetAllDishes()
        {
            return _dishService.GetAllDishes();
        }

        [HttpGet]
        [Route("/{id:int}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            try
            {
                return await _dishService.GetDish(id);
            }
            catch (NotFoundException<Dish> ex)
            {
                Console.WriteLine(ex);
                return NotFound(ex.Message);
            }
        }
    }
}

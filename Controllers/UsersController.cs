﻿using Microsoft.AspNetCore.Mvc;
using FreshMarket.Dtos;
using FreshMarket.Exceptions;
using FreshMarket.Exceptions.Postgres;
using FreshMarket.Exceptions.UserExceptions;
using FreshMarket.Services;
using Npgsql;

namespace FreshMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                return await _userService.GetUser(id);
            }
            catch (UserIdNotExistsException ex)
            {
                Console.WriteLine(ex);
                return NotFound(ex.Message);
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserToCreate userToCreate)
        {
            try
            {
                return Ok(await _userService.CreateUser(userToCreate));
            }
            catch (ModelCannotHaveIdException ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            catch (UniqueViolationException uniqueViolation)
            {
                return BadRequest(uniqueViolation.Message);
            }
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> PutUser(int id, PartialUser partialUser)
        {
            if (id != partialUser.Id)
                return BadRequest();

            try
            {
                return await _userService.UpdateUser(partialUser);
            }
            catch (Exception ex) when(ex is PostgresException or UniqueViolationException or UserIdNotExistsException)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (UserIdNotExistsException ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        //PUT "api/users/5/subscription
        [Route("{id:int}/subscription")]
        public async Task<ActionResult<SubscriptionDto>> UpdateSubscription(int id, SubscriptionDto subscriptionDto)
        {
            try
            {
                await _userService.UpdateSubscription(id, subscriptionDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        //todo: update user allergies
        //todo: update user dish preferences
        //todo: add/update user payment
    }
}

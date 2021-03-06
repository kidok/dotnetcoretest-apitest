﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if(_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            
            return _context.TodoItems.ToList<TodoItem>();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody]TodoItem item)
        {
            if(item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]TodoItem item)
        {
            if(item == null || item.Id != id)
            {
                return BadRequest();
            }

            var result = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(result == null)
            {
                return NotFound();
            }

            result.IsComplete = item.IsComplete;
            result.Name = item.Name;

            _context.TodoItems.Update(result);
            _context.SaveChanges();

            return new NoContentResult();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.TodoItems.FirstOrDefault(t => t.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(result);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}

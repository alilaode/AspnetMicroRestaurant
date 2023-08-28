using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IMenuRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IMenuRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Menu>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
            var menus = await _repository.GetMenus();
            return Ok(menus);
        }

        [HttpGet("{id:length(24)}", Name = "GetMenu")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Menu), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Menu>> GetMenuById(string id)
        {
            var product = await _repository.GetMenu(id);

            if (product == null)
            {
                _logger.LogError($"Menu with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetMenuByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Menu>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenuByCategory(string category)
        {
            var products = await _repository.GetMenuByCategory(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetMenuByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Menu>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenuByName(string name)
        {
            var items = await _repository.GetMenuByName(name);
            if (items == null)
            {
                _logger.LogError($"Menus with name: {name} not found.");
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Menu), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Menu>> CreateMenu([FromBody] Menu menu)
        {
            await _repository.CreateMenu(menu);

            return CreatedAtRoute("GetMenu", new { id = menu.Id }, menu);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Menu), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMenu([FromBody] Menu menu)
        {
            return Ok(await _repository.UpdateMenu(menu));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteMenu")]
        [ProducesResponseType(typeof(Menu), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMenuById(string id)
        {
            return Ok(await _repository.DeleteMenu(id));
        }
    }
}

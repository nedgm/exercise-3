using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using FixMessagesApi.Mediators;
using FixMessagesApi.Mediators.Exceptions;
using FixMessagesApi.Mediators.Models;
using Microsoft.AspNetCore.Http;

namespace FixMessagesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixMessageController : ControllerBase
    {
        private readonly ILogger<FixMessageController> _logger;
        private readonly FixMessageMediator _fixMessageMediator;

        public FixMessageController(ILogger<FixMessageController> logger, FixMessageMediator fixMessageMediator)
        {
            _logger = logger;
            _fixMessageMediator = fixMessageMediator;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FixMessageViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMessageById(int id)
        {
            try
            {
                var result = _fixMessageMediator.GetFixMessageById(id);

                return Ok(result);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("desc")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FixMessageViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMessagesByDescription([FromQuery] string value, int? offset, int? count)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return BadRequest();
            }
            try
            {
                var result = _fixMessageMediator.GetFixMessagesByDescription(value, offset, count);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("sendtime")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FixMessageViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMessagesByDateTime([FromQuery] string from, string to, int? offset, int? count)
        {
            try
            {
                var result = _fixMessageMediator.GetFixMessagesBySendingTime(from, to, offset, count);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}

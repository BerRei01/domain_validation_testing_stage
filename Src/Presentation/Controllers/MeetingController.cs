using Application;
using Application.CreateMeeting.Implementation;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingController: Controller
    {

        private readonly IMediator _mediator;
        
        public MeetingController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            return Ok();
            
        }

        [HttpPost("SetMeeting")]
        public async Task<IActionResult> CreateSetMeeting([FromBody] CreateSetMeetingRequest request) // We reuse the Mediator request from the application here and save ourself the trouble of introducing aan extra Dto
        {
            ErrorOr<Success> requestResult = await _mediator.Send(request);
            if (requestResult.IsError) return BadRequest(requestResult.Errors);
            
            return Ok();
        }
        
        [HttpPost("ErrorOrMeeting")]
        public async Task<IActionResult> CreateErrorOrMeeting([FromBody] CreateErrorOrMeetingRequest request) // We reuse the Mediator request from the application here and save ourself the trouble of introducing aan extra Dto
        {
            ErrorOr<Success> requestResult = await _mediator.Send(request);
            if (requestResult.IsError) return BadRequest(requestResult.Errors);
            
            return Ok();
        }
        
        [HttpPost("AttributeMeeting")]
        public async Task<IActionResult> CreateAttributeMeeting([FromBody] CreateAttributeMeetingRequest request) // We reuse the Mediator request from the application here and save ourself the trouble of introducing aan extra Dto
        {
            ErrorOr<Success> requestResult = await _mediator.Send(request);
            if (requestResult.IsError) return BadRequest(requestResult.Errors);
            
            return Ok();
        }
        
        [HttpPost("ValueMeeting")]
        public async Task<IActionResult> CreateValueMeeting([FromBody] CreateValueMeetingRequest request) // We reuse the Mediator request from the application here and save ourself the trouble of introducing aan extra Dto
        {
            ErrorOr<Success> requestResult = await _mediator.Send(request);
            if (requestResult.IsError) return BadRequest(requestResult.Errors);
            
            return Ok();
        }
    }
}
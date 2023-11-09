using Microsoft.AspNetCore.Mvc;
using Robot;
using Data.Dto;

namespace WebServer.Controllers;

[ApiController]
[Route("api/sequence")]
public class SequenceController : ControllerBase
{
    private readonly SequenceManager _sequenceManager;

    public SequenceController(SequenceManager sequenceManager)
    {
        _sequenceManager = sequenceManager;
    }

    [HttpPost("run-sequence")]
    public ActionResult RunSequence(SequenceDto sequenceDto)
    {
        var isSuccess = _sequenceManager.Start(sequenceDto);
        
        if (isSuccess)
        {
            return Ok("success");
        }

        return BadRequest("busy");
    }

    [HttpPost("stop-sequence")]
    public void StopSequence()
    {
        _sequenceManager.Stop();
    }

    [HttpPost("pause-sequence")]
    public void PauseSequence()
    {
        _sequenceManager.Pause();
    }

    [HttpPost("resume-sequence")]
    public void ResumeSequence()
    {
        _sequenceManager.Resume();
    }
}


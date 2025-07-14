using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        
        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            //check model state is all dara annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var response = await userInterface.Register(appUserDTO);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            //check model state is all dara annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var response = await userInterface.Login(loginDTO);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if(id<=0) return BadRequest("Invalid user Id");

            //get user
            var user = await userInterface.GetUser(id);
            
            return user.Id > 0  ? Ok(user) : NotFound(user);
        }
    }
}

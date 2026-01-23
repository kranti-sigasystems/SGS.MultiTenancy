using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.UI.Controllers
{


    [Authorize]
    public class UserController : Controller
    {

        public readonly IUserService userService;


        /// <summary>
        /// Initializing the members if UserController
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Index table for the users.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// create form for the users.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View(new UserDto());
        }

        /// <summary>
        /// delete method for the users.
        /// </summary>
        /// <returns></returns>

        public IActionResult Delete()
        {
            return View();
        }

        /// <summary>
        /// update method for the users.
        /// </summary>
        /// <returns></returns>
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserDto model)
        {

            if (!ModelState.IsValid)
                return View(model);
            var normalizedEmail = model.Email.Trim().ToLowerInvariant();
            //// TenantId and AddressId is HardCoded change it in the run time
            //RegisterRequestDto registerRequestDto = new RegisterRequestDto()
            //{
            //    Name = model.UserName,
            //    Email = normalizedEmail,

            //    Password = model.Password,
            //    TenantId = Guid.Parse("67163edd-ec85-11f0-a68b-345a60c6c001")
            //};
            //RegisterResponseDto registerResponseDto = await _userService.Register(registerRequestDto);
            //LoginViewModel loginviewmodel = new LoginViewModel
            //{
            //    UserName = registerResponseDto.User.UserName,
            //    Password = ""
            //}


            model.TenantId = Guid.Parse("4e7537bf-55ff-4740-9aa6-22cbb857a355");



            return RedirectToAction("Index");
        }
    }
}

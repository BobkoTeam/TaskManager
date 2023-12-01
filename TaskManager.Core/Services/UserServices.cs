using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.User;
using TaskManager.Core.Entities.User;

namespace TaskManager.Core.Services
{
    public class UserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _maper;

        public UserServices(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _maper = mapper;
            _config = config;
        }


        public async Task<ServiceResponse> LoginUserAsync(LoginUserDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User or password incorrect."
                };
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User signed in successfully."
                };
            }

            if (result.IsNotAllowed)
            {

                return new ServiceResponse
                {
                    Success = true,
                    Message = "User signed in successfully."
                };
                /* return new ServiceResponse
                 {
                     Success = false,
                     Message = "Confirm your email please."
                 };*/
            }

            if (result.IsLockedOut)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User is locked out. Connect with site administrator."
                };
            }

            return new ServiceResponse
            {
                Success = false,
                Message = "User or password incorrect."
            };
        }




        public async Task<ServiceResponse> Register(RegisterDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return new ServiceResponse
                {
                    Message = "User exists",
                    Success = false,
                };
            }

            var mappedUser = _maper.Map<RegisterDto, AppUser>(model);
            IdentityResult result = await _userManager.CreateAsync(mappedUser, model.Password);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Message = "User successfully created.",
                    Success = true,
                };
            }
            List<IdentityError> errorList = result.Errors.ToList();

            string errors = "";
            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToString();
            }
            return new ServiceResponse
            {
                Message = "User creating error.",
                Success = false,
                Payload = errors
            };
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }



        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new ServiceResponse { Success = false, Message = "User not found." };
            }
            var mappedUser = _maper.Map<AppUser, EditUserDto>(user);
            return new ServiceResponse
            {
                Success = true,
                Message = "User loaded!",
                Payload = mappedUser
            };
        }

        public async Task<bool> UpdateUserInfoAsync(string id, string newFirstName, string newLastName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {

                return false;
            }
            user.FirstName = newFirstName;
            user.LastName = newLastName;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<ServiceResponse> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Message = "User not found.",
                    Success = false
                };
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new ServiceResponse
                {
                    Message = "Password do not match.",
                    Success = false
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Message = "Password successfully updated.",
                    Success = true,
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Message = "Password not updated.",
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description),
                };
            }
        }
    }
}

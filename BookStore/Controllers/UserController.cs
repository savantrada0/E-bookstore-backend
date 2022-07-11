using BookStore.Models.DataModels;
using BookStore.Models.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System;

namespace BookStore.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserRepository _repository = new();

        [HttpGet]
        [Route("roles")]
        public IActionResult GetRoles()
        {
            var roles = _repository.GetRoles();
            ListResponse<RoleModel> listResponse = new()
            {
                records = roles.records.Select(c => new RoleModel(c)),
                totalRecords = roles.totalRecords,
            };

            return Ok(listResponse);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetUsers(int pageIndex = 1, int pageSize = 10, string? keyword = "")
        {
            try
            {
                keyword = keyword?.ToLower()?.Trim();
                var users = _repository.GetUsers(pageIndex, pageSize, keyword);
                if (users == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), users);
            }

            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
           // ListResponse<User> response = _repository.GetUsers(pageIndex, pageSize, keyword);
          //  ListResponse<UserModel> users = new ListResponse<UserModel>()
          //  {
           //     records = response.records.Select(u => new UserModel(u)),
            //    totalRecords = response.totalRecords,
            //};
          //  return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _repository.GetUser(id);
                if (user == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), user);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
         //   User user = _repository.GetUser(id);
          //  if (user == null)
          //      return NotFound();

          //  UserModel userModel = new UserModel(user);
          //  return Ok(userModel);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateUser(UserModel model)
        {
            try
            {
                if (model != null)
                {
                    var user = _repository.GetUser(model.Id);
                    if (user == null)
                        return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found");

                    user.Firstname = model.Firstname;
                    user.Lastname = model.Lastname;
                    user.Email = model.Email;

                    var isSaved = _repository.UpdateUser(user);
                    if (isSaved)
                    {
                        return StatusCode(HttpStatusCode.OK.GetHashCode(), "User detail updated successfully");
                    }
                }

                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            //    if (model != null)
            //      return BadRequest();

            //User user = new User()
            //   {
            //     Id = model.Id,
            //     Firstname = model.Firstname,
            //     Lastname = model.Lastname,
            //     Email = model.Email,
            //    Password = model.Password,
            //     Roleid = model.RoleId,
            // };

            // user = _repository.UpdateUser(user);
            //  return Ok(user);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = _repository.DeleteUser(id);
            return Ok(isDeleted);
        }
    }
}

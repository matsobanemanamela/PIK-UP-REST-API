using System;
using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PIK_UP_REST_API.Models;
using System.Security.Claims;
using System.Text;
using System.IO;

namespace PIK_UP_REST_API.Controllers
{
    public class UserTablesController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();
       // private IMapper _mapper;

        // GET: api/UserTables
        [AllowAnonymous]
        public IQueryable<Object> GetUserTables()
        {
            var UserTables = db.UserTables.Select(x => new { x.UserID, x.Name, x.Surname, x.StudentNumber, x.Gender, x.Institution, x.Email, x.MobileNumber, x.Password, x.Image });
            return UserTables;
        }

        [HttpGet]
        [Route("api/GetUserClaims")]
        [Authorize]
        public UserTable GetUserClaims()
        {
          
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            UserTable model = new UserTable()
            {
                UserID = Convert.ToInt32(identityClaims.FindFirst("UserID").Value),
                Name = identityClaims.FindFirst("Name").Value,
                Surname = identityClaims.FindFirst("Surname").Value,
                StudentNumber = Convert.ToInt32(identityClaims.FindFirst("StudentNumber").Value),
                Email = identityClaims.FindFirst("Email").Value,
                Institution = identityClaims.FindFirst("Institution").Value,
                MobileNumber = identityClaims.FindFirst("MobileNumber").Value,
                Password = identityClaims.FindFirst("Password").Value,
                Gender = identityClaims.FindFirst("Gender").Value
              //  Image = Convert.(identityClaims.FindFirst("Image").Value)
            };
            return model;
        }
        // GET: api/UserTables/5
        [AllowAnonymous]
       [ResponseType(typeof(UserTable))]
         public IHttpActionResult GetUserTable(int id)
         {
             UserTable userTable = db.UserTables.Find(id);
             if (userTable == null)
             {
                 return NotFound();
             }

             return Ok(userTable);
         }
      

        // PUT: api/UserTables/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserTable(int id, UserTable userTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userTable.UserID)
            {
                return BadRequest();
            }

            db.Entry(userTable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserTables
        [AllowAnonymous]
        [ResponseType(typeof(UserTable))]
        public IHttpActionResult PostUserTable(UserTable userTable)
        {
           var user = db.UserTables.FirstOrDefault(x => x.Email == userTable.Email);

            if (user != null)
            {
                return BadRequest("Email that you have Entered already exist");
            }
            else
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
            }
            return CreatedAtRoute("DefaultApi", new { id = userTable.UserID }, userTable);
        }

        // DELETE: api/UserTables/5
        [AllowAnonymous]
        [ResponseType(typeof(UserTable))]
        public IHttpActionResult DeleteUserTable(int id)
        {
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return NotFound();
            }

            db.UserTables.Remove(userTable);
            db.SaveChanges();

            return Ok(userTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTableExists(int id)
        {
            return db.UserTables.Count(e => e.UserID == id) > 0;
        }
    }
}
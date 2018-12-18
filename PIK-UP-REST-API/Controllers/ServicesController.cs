using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PIK_UP_REST_API.Models;
using System.Web;

namespace PIK_UP_REST_API.Controllers
{
    public class ServicesController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/Services
        [AllowAnonymous]
        public IQueryable<Object> GetServices()
        {
            var service = db.Services.Select(x => new { x.ServiceID, x.UserID, x.Typeofservice, x.ServiceImage, x.Serviceprice, x.Comment });

            return service;
        }


        // GET: api/Services/5
        /* [ResponseType(typeof(Service))]
         public IHttpActionResult GetService(int id)
         {
             Service service = db.Services.Find(id);
             if (service == null)
             {
                 return NotFound();
             }

             return Ok(service);
         } */

        [HttpGet]
        [Route("api/GeteveryService")]
        [AllowAnonymous]
        public IQueryable<Object> GeteveryService()
        {

            var list = db.UserTables.Join(db.Services, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    ServiceID = ca.ServiceID,
                    UserID = trav.UserID,
                    Typeofservice = ca.Typeofservice,
                    ServiceImage = ca.ServiceImage,
                    Serviceprice = ca.Serviceprice,
                    Comment = ca.Comment,
                    Name = trav.Name,
                    Image = trav.Image,
                    Surname = trav.Surname
                }
              );

            //var details = list.Where(c => c.UserID.Equals(id));

            if (list == null)
            {
                return (null);
            }

            return list;

        }

        [HttpGet]
        [Route("api/GetService")]
        [AllowAnonymous]
        public IQueryable<Object> GetService(int id)
        {

            var list = db.UserTables.Join(db.Services, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    ServiceID = ca.ServiceID,
                    UserID = trav.UserID,
                    Typeofservice = ca.Typeofservice,
                    ServiceImage = ca.ServiceImage,
                    Serviceprice = ca.Serviceprice,
                    Comment = ca.Comment
                }
              );

            var details = list.Where(c => c.UserID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }


        // PUT: api/Services/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.ServiceID)
            {
                return BadRequest();
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/Services
        [AllowAnonymous]
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
         
            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.ServiceID }, service);
        }

        // DELETE: api/Services/5
        [AllowAnonymous]
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceID == id) > 0;
        }
    }
}
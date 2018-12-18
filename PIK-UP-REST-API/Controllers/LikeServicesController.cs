using System;
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

namespace PIK_UP_REST_API.Controllers
{
    public class LikeServicesController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeServices
        [AllowAnonymous]
        public IQueryable<Object> GetLikeServices()
        {
            var LikeServices = db.LikeServices.Select(x => new {x.likeId,x.ServiceID,x.UserID,x.DateTime,x.Condition });
            return LikeServices;
        }

        // GET: api/LikeServices/5
        /* [ResponseType(typeof(LikeService))]
         public IHttpActionResult GetLikeService(int id)
         {
             LikeService likeService = db.LikeServices.Find(id);
             if (likeService == null)
             {
                 return NotFound();
             }

             return Ok(likeService);
         } */

        [HttpGet]
        [Route("api/GetLikeServices")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeServices(int id)
        {

            var list = db.Services.Join(db.LikeServices, trav => trav.ServiceID, ca => ca.ServiceID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    ServiceID = ca.ServiceID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.ServiceID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        [HttpGet]
        [Route("api/GetLikeServicesCount")]
        [AllowAnonymous]
        public int GetLikeServicesCount(int id)
        {

            var list = db.Services.Join(db.LikeServices, trav => trav.ServiceID, ca => ca.ServiceID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    ServiceID = trav.ServiceID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.ServiceID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeServices/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeService(int id, LikeService likeService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeService.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeService).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeServiceExists(id))
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

        // POST: api/LikeServices
        [AllowAnonymous]
        [ResponseType(typeof(LikeService))]
        public IHttpActionResult PostLikeService(LikeService likeService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LikeServices.Add(likeService);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeService.likeId }, likeService);
        }

        // DELETE: api/LikeServices/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeService))]
        public IHttpActionResult DeleteLikeService(int id)
        {
            LikeService likeService = db.LikeServices.Find(id);
            if (likeService == null)
            {
                return NotFound();
            }

            db.LikeServices.Remove(likeService);
            db.SaveChanges();

            return Ok(likeService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeServiceExists(int id)
        {
            return db.LikeServices.Count(e => e.likeId == id) > 0;
        }
    }
}
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
    public class LikeTransportsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeTransports
        [AllowAnonymous]
        public IQueryable<LikeTransport> GetLikeTransports()
        {
            return db.LikeTransports;
        }

        // GET: api/LikeTransports/5
        /* [ResponseType(typeof(LikeTransport))]
         public IHttpActionResult GetLikeTransport(int id)
         {
             LikeTransport likeTransport = db.LikeTransports.Find(id);
             if (likeTransport == null)
             {
                 return NotFound();
             }

             return Ok(likeTransport);
         } */

        [HttpGet]
        [Route("api/GetLikeTransports")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeTransports(int id)
        {

            var list = db.Transports.Join(db.LikeTransports, trav => trav.TransportationID, ca => ca.TransportationID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    TransportationID = ca.TransportationID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.TransportationID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        [HttpGet]
        [Route("api/GetLikeTransportsCount")]
        [AllowAnonymous]
        public int GetLikeTransportsCount(int id)
        {

            var list = db.Transports.Join(db.LikeTransports, trav => trav.TransportationID, ca => ca.TransportationID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    TransportationID = trav.TransportationID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.TransportationID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeTransports/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeTransport(int id, LikeTransport likeTransport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeTransport.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeTransport).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeTransportExists(id))
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

        // POST: api/LikeTransports
        [AllowAnonymous]
        [ResponseType(typeof(LikeTransport))]
        public IHttpActionResult PostLikeTransport(LikeTransport likeTransport)
        {

            db.LikeTransports.Add(likeTransport);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeTransport.likeId }, likeTransport);
        }

        // DELETE: api/LikeTransports/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeTransport))]
        public IHttpActionResult DeleteLikeTransport(int id)
        {
            LikeTransport likeTransport = db.LikeTransports.Find(id);
            if (likeTransport == null)
            {
                return NotFound();
            }

            db.LikeTransports.Remove(likeTransport);
            db.SaveChanges();

            return Ok(likeTransport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeTransportExists(int id)
        {
            return db.LikeTransports.Count(e => e.likeId == id) > 0;
        }
    }
}
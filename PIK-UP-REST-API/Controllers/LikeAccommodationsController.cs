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
    public class LikeAccommodationsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeAccommodations
        [AllowAnonymous]
        public IQueryable<Object> GetLikeAccommodations()
        {
            var LikeAccommodations = db.LikeAccommodations.Select(x => new { x.likeId, x.AccommodationID, x.UserID, x.DateTime, x.Condition });
            return LikeAccommodations;
        }

        // GET: api/LikeAccommodations/5
        /*   [ResponseType(typeof(LikeAccommodation))]
           public IHttpActionResult GetLikeAccommodation(int id)
           {
               LikeAccommodation likeAccommodation = db.LikeAccommodations.Find(id);
               if (likeAccommodation == null)
               {
                   return NotFound();
               }

               return Ok(likeAccommodation);
           }   */

        [HttpGet]
        [Route("api/GetLikeAccommodations")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeAccommodations(int id)
        {

            var list = db.Accommodations.Join(db.LikeAccommodations, trav => trav.AccommodationID, ca => ca.AccommodationID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    AccommodationID = ca.AccommodationID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.AccommodationID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        [HttpGet]
        [Route("api/GetLikeAccommodationsCount")]
        [AllowAnonymous]
        public int GetLikeAccommodationsCount(int id)
        {

            var list = db.Accommodations.Join(db.LikeAccommodations, trav => trav.AccommodationID, ca => ca.AccommodationID,
                   (trav, ca) => new {
                       likeId = ca.likeId,
                       AccommodationID = trav.AccommodationID,
                       UserID = ca.UserID,
                       DateTime = ca.DateTime,
                       Condition = ca.Condition

                   }
                 );

            var details = list.Where(c => c.AccommodationID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeAccommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeAccommodation(int id, LikeAccommodation likeAccommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeAccommodation.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeAccommodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeAccommodationExists(id))
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

        // POST: api/LikeAccommodations
        [AllowAnonymous]
        [ResponseType(typeof(LikeAccommodation))]
        public IHttpActionResult PostLikeAccommodation(LikeAccommodation likeAccommodation)
        {

            db.LikeAccommodations.Add(likeAccommodation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeAccommodation.likeId }, likeAccommodation);
        }

        // DELETE: api/LikeAccommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeAccommodation))]
        public IHttpActionResult DeleteLikeAccommodation(int id)
        {
            LikeAccommodation likeAccommodation = db.LikeAccommodations.Find(id);
            if (likeAccommodation == null)
            {
                return NotFound();
            }

            db.LikeAccommodations.Remove(likeAccommodation);
            db.SaveChanges();

            return Ok(likeAccommodation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeAccommodationExists(int id)
        {
            return db.LikeAccommodations.Count(e => e.likeId == id) > 0;
        }
    }
}
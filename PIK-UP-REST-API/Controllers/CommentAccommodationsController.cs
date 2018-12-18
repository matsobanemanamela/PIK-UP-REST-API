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
    public class CommentAccommodationsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentAccommodations
        [AllowAnonymous]
        public IQueryable<CommentAccommodation> GetCommentAccommodations()
        {
            return db.CommentAccommodations;
        }

        // GET: api/CommentAccommodations/5
        /* [ResponseType(typeof(CommentAccommodation))]
         public IHttpActionResult GetCommentAccommodation(int id)
         {
             CommentAccommodation commentAccommodation = db.CommentAccommodations.Find(id);
             if (commentAccommodation == null)
             {
                 return NotFound();
             }

             return Ok(commentAccommodation);
         }  */


        [HttpGet]
        [Route("api/GetCommentAccommodations")]
        [AllowAnonymous]
        public IQueryable<Object> GetCommentAccommodations(int id)
        {

            var list = db.Accommodations.Join(db.CommentAccommodations, trav => trav.AccommodationID, ca => ca.AccommodationID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    AccommodationID = ca.AccommodationID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

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
        [Route("api/GetCommentAccommodationsCount")]
        [AllowAnonymous]
        public int  GetCommentAccommodationsCount(int id)
        {
           // int count = 0;
            //List<object> l = new List<object>(GetCommentAccommodationsCount(id));

            var list = db.Accommodations.Join(db.CommentAccommodations, trav => trav.AccommodationID, ca => ca.AccommodationID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    AccommodationID = trav.AccommodationID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

                }
              );
           
                var details = list.Where(c => c.AccommodationID.Equals(id)).Count();

          /*  if (details == null)
            {

                return (null);
            }*/

            return details;

        }
        // PUT: api/CommentAccommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentAccommodation(int id, CommentAccommodation commentAccommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentAccommodation.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentAccommodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentAccommodationExists(id))
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

        // POST: api/CommentAccommodations
        [AllowAnonymous]
        [ResponseType(typeof(CommentAccommodation))]
        public IHttpActionResult PostCommentAccommodation(CommentAccommodation commentAccommodation)
        {
          

            db.CommentAccommodations.Add(commentAccommodation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentAccommodation.CommentID }, commentAccommodation);
        }

        // DELETE: api/CommentAccommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(CommentAccommodation))]
        public IHttpActionResult DeleteCommentAccommodation(int id)
        {
            CommentAccommodation commentAccommodation = db.CommentAccommodations.Find(id);
            if (commentAccommodation == null)
            {
                return NotFound();
            }

            db.CommentAccommodations.Remove(commentAccommodation);
            db.SaveChanges();

            return Ok(commentAccommodation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentAccommodationExists(int id)
        {
            return db.CommentAccommodations.Count(e => e.CommentID == id) > 0;
        }
    }
}
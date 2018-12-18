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
    public class CommentTransportsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentTransports
        [AllowAnonymous]
        public IQueryable<Object> GetCommentTransports()
        {
            var CommentTransports = db.CommentTransports.Select(x => new {x.CommentID,x.TransportationID,x.UserID,x.DateandTime,x.Comments });
            return CommentTransports;
        }

        // GET: api/CommentTransports/5
        /*  [ResponseType(typeof(CommentTransport))]
          public IHttpActionResult GetCommentTransport(int id)
          {
              CommentTransport commentTransport = db.CommentTransports.Find(id);
              if (commentTransport == null)
              {
                  return NotFound();
              }

              return Ok(commentTransport);
          } */

        [HttpGet]
        [Route("api/GetCommentTransports")]
        [AllowAnonymous]
        public IQueryable<Object> GetCommentTransports(int id)
        {

            var list = db.Transports.Join(db.CommentTransports, trav => trav.TransportationID, ca => ca.TransportationID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    TransportationID = trav.TransportationID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

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
        [Route("api/GetCommentTransportsCount")]
        [AllowAnonymous]
        public int GetCommentTransportsCount(int id)
        {

            var list = db.Transports.Join(db.CommentTransports, trav => trav.TransportationID, ca => ca.TransportationID,
        (trav, ca) => new {
            CommentID = ca.CommentID,
            TransportationID = ca.TransportationID,
            UserID = ca.UserID,
            DateandTime = ca.DateandTime,
            Comments = ca.Comments

        }
      );

            var details = list.Where(c => c.TransportationID.Equals(id)).Count();

            return details;

        }

        // PUT: api/CommentTransports/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentTransport(int id, CommentTransport commentTransport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentTransport.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentTransport).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentTransportExists(id))
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

        // POST: api/CommentTransports
        [AllowAnonymous]
        [ResponseType(typeof(CommentTransport))]
        public IHttpActionResult PostCommentTransport(CommentTransport commentTransport)
        {

            db.CommentTransports.Add(commentTransport);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentTransport.CommentID }, commentTransport);
        }

        // DELETE: api/CommentTransports/5
        [AllowAnonymous]
        [ResponseType(typeof(CommentTransport))]
        public IHttpActionResult DeleteCommentTransport(int id)
        {
            CommentTransport commentTransport = db.CommentTransports.Find(id);
            if (commentTransport == null)
            {
                return NotFound();
            }

            db.CommentTransports.Remove(commentTransport);
            db.SaveChanges();

            return Ok(commentTransport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentTransportExists(int id)
        {
            return db.CommentTransports.Count(e => e.CommentID == id) > 0;
        }
    }
}
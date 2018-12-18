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
    public class CommentServicesController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentServices
        [AllowAnonymous]
        public IQueryable<Object> GetCommentServices()
        {
            var CommentServices = db.CommentServices.Select(x => new {x.CommentID,x.ServiceID,x.UserID,x.DateandTime,x.Comments });
            return CommentServices;
        }

        // GET: api/CommentServices/5
        /*  [ResponseType(typeof(CommentService))]
          public IHttpActionResult GetCommentService(int id)
          {
              CommentService commentService = db.CommentServices.Find(id);
              if (commentService == null)
              {
                  return NotFound();
              }

              return Ok(commentService);
          }  */

        [HttpGet]
        [Route("api/GetCommentServices")]
        [AllowAnonymous]
        public IQueryable<Object> GetAccommodation(int id)
        {

            var list = db.Services.Join(db.CommentServices, trav => trav.ServiceID, ca => ca.ServiceID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    ServiceID = ca.ServiceID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

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
        [Route("api/GetCommentServicesCount")]
        [AllowAnonymous]
        public int GetCommentServicesCount(int id)
        {

            var list = db.Services.Join(db.CommentServices, trav => trav.ServiceID, ca => ca.ServiceID,
                            (trav, ca) => new {
                                CommentID = ca.CommentID,
                                ServiceID = trav.ServiceID,
                                UserID = ca.UserID,
                                DateandTime = ca.DateandTime,
                                Comments = ca.Comments

                            }
                          );

            var details = list.Where(c => c.ServiceID.Equals(id)).Count();

            return details;

        }

        // PUT: api/CommentServices/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentService(int id, CommentService commentService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentService.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentService).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentServiceExists(id))
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

        // POST: api/CommentServices
        [AllowAnonymous]
        [ResponseType(typeof(CommentService))]
        public IHttpActionResult PostCommentService(CommentService commentService)
        {
          
            db.CommentServices.Add(commentService);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentService.CommentID }, commentService);
        }

        // DELETE: api/CommentServices/5
        [AllowAnonymous]
        [ResponseType(typeof(CommentService))]
        public IHttpActionResult DeleteCommentService(int id)
        {
            CommentService commentService = db.CommentServices.Find(id);
            if (commentService == null)
            {
                return NotFound();
            }

            db.CommentServices.Remove(commentService);
            db.SaveChanges();

            return Ok(commentService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentServiceExists(int id)
        {
            return db.CommentServices.Count(e => e.CommentID == id) > 0;
        }
    }
}
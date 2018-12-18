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
    public class LikeBooksController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeBooks
        [AllowAnonymous]
        public IQueryable<Object> GetLikeBooks()
        {
            var LikeBooks = db.LikeBooks.Select(x => new { x.likeId, x.BookID, x.UserID, x.DateTime, x.Condition });
            return LikeBooks;
        }

        // GET: api/LikeBooks/5
        /* [ResponseType(typeof(LikeBook))]
         public IHttpActionResult GetLikeBook(int id)
         {
             LikeBook likeBook = db.LikeBooks.Find(id);
             if (likeBook == null)
             {
                 return NotFound();
             }

             return Ok(likeBook);
         } */

        [HttpGet]
        [Route("api/GetLikeBooks")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeBooks(int id)
        {

            var list = db.Books.Join(db.LikeBooks, trav => trav.BookID, ca => ca.BookID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    BookID = ca.BookID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.BookID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        [HttpGet]
        [Route("api/GetLikeBooksCount")]
        [AllowAnonymous]
        public int GetLikeBooksCount(int id)
        {

            var list = db.Books.Join(db.LikeBooks, trav => trav.BookID, ca => ca.BookID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    BookID = trav.BookID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.BookID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeBooks/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeBook(int id, LikeBook likeBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeBook.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeBook).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeBookExists(id))
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

        // POST: api/LikeBooks
        [AllowAnonymous]
        [ResponseType(typeof(LikeBook))]
        public IHttpActionResult PostLikeBook(LikeBook likeBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LikeBooks.Add(likeBook);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeBook.likeId }, likeBook);
        }

        // DELETE: api/LikeBooks/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeBook))]
        public IHttpActionResult DeleteLikeBook(int id)
        {
            LikeBook likeBook = db.LikeBooks.Find(id);
            if (likeBook == null)
            {
                return NotFound();
            }

            db.LikeBooks.Remove(likeBook);
            db.SaveChanges();

            return Ok(likeBook);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeBookExists(int id)
        {
            return db.LikeBooks.Count(e => e.likeId == id) > 0;
        }
    }
}
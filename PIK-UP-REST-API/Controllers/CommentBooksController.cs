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
    public class CommentBooksController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentBooks
        [AllowAnonymous]
        public IQueryable<CommentBook> GetCommentBooks()
        {
            return db.CommentBooks;
        }

        // GET: api/CommentBooks/5
       /* [ResponseType(typeof(CommentBook))]
        public IHttpActionResult GetCommentBook(int id)
        {
            CommentBook commentBook = db.CommentBooks.Find(id);
            if (commentBook == null)
            {
                return NotFound();
            }

            return Ok(commentBook);
        }  */
              
        [HttpGet]
        [Route("api/GetCommentBooks")]
        [AllowAnonymous]
        public IQueryable<Object> GetCommentBooks(int id)
        {

            var list = db.Books.Join(db.CommentBooks, trav => trav.BookID, ca => ca.BookID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    BookID = ca.BookID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

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
        [Route("api/GetCommentBooksCount")]
        [AllowAnonymous]
        public int GetCommentBooksCount(int id)
        {
          

            var list = db.Books.Join(db.CommentBooks, trav => trav.BookID, ca => ca.BookID,
                 (trav, ca) => new {
                     CommentID = ca.CommentID,
                     BookID = trav.BookID,
                     UserID = ca.UserID,
                     DateandTime = ca.DateandTime,
                     Comments = ca.Comments

                 }
               );

            var details = list.Where(c => c.BookID.Equals(id)).Count();

            return details;

        }
        // PUT: api/CommentBooks/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentBook(int id, CommentBook commentBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentBook.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentBook).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentBookExists(id))
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

        // POST: api/CommentBooks
        [AllowAnonymous]
        [ResponseType(typeof(CommentBook))]
        public IHttpActionResult PostCommentBook(CommentBook commentBook)
        {
            
            db.CommentBooks.Add(commentBook);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentBook.CommentID }, commentBook);
        }

        // DELETE: api/CommentBooks/5
        [AllowAnonymous]
        [ResponseType(typeof(CommentBook))]
        public IHttpActionResult DeleteCommentBook(int id)
        {
            CommentBook commentBook = db.CommentBooks.Find(id);
            if (commentBook == null)
            {
                return NotFound();
            }

            db.CommentBooks.Remove(commentBook);
            db.SaveChanges();

            return Ok(commentBook);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentBookExists(int id)
        {
            return db.CommentBooks.Count(e => e.CommentID == id) > 0;
        }
    }
}
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
    public class BooksController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/Books
        [AllowAnonymous]
        public IQueryable<Object> GetBooks()
        {
            var books = db.Books.Select(x => new { x.BookID, x.UserID, x.BookName, x.Image, x.Edition,x.Course,x.neworused, x.Price,x.Comment });
            return books;
        }

        [HttpGet]
        [Route("api/GeteveryBooksDetails")]
        [AllowAnonymous]
        public IQueryable<Object> GeteveryBooksDetails()
        {

            var list = db.UserTables.Join(db.Books, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    BookID = ca.BookID,
                    UserID = trav.UserID,
                    BookName = ca.BookName,
                    Edition = ca.Edition,
                    Course = ca.Course,
                    neworused = ca.neworused,
                    Image = ca.Image,
                    Price = ca.Price,
                    Comment = ca.Comment,
                    Name = trav.Name,
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
        [Route("api/GetBooksDetails")]
        [AllowAnonymous]
        public IQueryable<Object> GetBooksDetails(int id)
        {

            var list = db.UserTables.Join(db.Books, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    BookID = ca.BookID,
                    UserID = trav.UserID,
                    BookName = ca.BookName,
                    Edition = ca.Edition,
                    Course = ca.Course,
                    neworused = ca.neworused,
                    Image = ca.Image,
                    Price = ca.Price,
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

        // PUT: api/Books/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookID)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        [AllowAnonymous]
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
           
            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookID }, book);
        }

        // DELETE: api/Books/5
        [AllowAnonymous]
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookID == id) > 0;
        }
    }
}
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
    public class CommentProductsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentProducts
        [AllowAnonymous]
        public IQueryable<Object> GetCommentProducts()
        {
            var CommentProducts = db.CommentProducts.Select(x => new {x.CommentID,x.ProductID,x.UserID,x.DateandTime,x.Comments });
            return CommentProducts;
        }

        // GET: api/CommentProducts/5
        /*  [ResponseType(typeof(CommentProduct))]
          public IHttpActionResult GetCommentProduct(int id)
          {
              CommentProduct commentProduct = db.CommentProducts.Find(id);
              if (commentProduct == null)
              {
                  return NotFound();
              }

              return Ok(commentProduct);
          } */

        [HttpGet]
        [Route("api/GetCommentProducts")]
        [AllowAnonymous]
        public IQueryable<Object> GetCommentProducts(int id)
        {

            var list = db.Products.Join(db.CommentProducts, trav => trav.ProductID, ca => ca.ProductID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    ProductID = trav.ProductID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

                }
              );

            var details = list.Where(c => c.ProductID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }


        [HttpGet]
        [Route("api/GetCommentProductsCount")]
        [AllowAnonymous]
        public int GetCommentProductsCount(int id)
        {


            var list = db.Products.Join(db.CommentProducts, trav => trav.ProductID, ca => ca.ProductID,
                    (trav, ca) => new {
                        CommentID = ca.CommentID,
                        ProductID = ca.ProductID,
                        UserID = ca.UserID,
                        DateandTime = ca.DateandTime,
                        Comments = ca.Comments

                    }
                  );

             var details = list.Where(c => c.ProductID.Equals(id)).Count();

            return details;

        }

        // PUT: api/CommentProducts/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentProduct(int id, CommentProduct commentProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentProduct.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentProductExists(id))
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

        // POST: api/CommentProducts
        [AllowAnonymous]
        [ResponseType(typeof(CommentProduct))]
        public IHttpActionResult PostCommentProduct(CommentProduct commentProduct)
        {

            db.CommentProducts.Add(commentProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentProduct.CommentID }, commentProduct);
        }

        // DELETE: api/CommentProducts/5
        [AllowAnonymous]
        [ResponseType(typeof(CommentProduct))]
        public IHttpActionResult DeleteCommentProduct(int id)
        {
            CommentProduct commentProduct = db.CommentProducts.Find(id);
            if (commentProduct == null)
            {
                return NotFound();
            }

            db.CommentProducts.Remove(commentProduct);
            db.SaveChanges();

            return Ok(commentProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentProductExists(int id)
        {
            return db.CommentProducts.Count(e => e.CommentID == id) > 0;
        }
    }
}
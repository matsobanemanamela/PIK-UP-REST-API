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
    public class LikeProductsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeProducts
        [AllowAnonymous]
        public IQueryable<Object> GetLikeProducts()
        {
            var LikeProducts = db.LikeProducts.Select(x => new {x.likeId,x.ProductID,x.UserID,x.DateTime,x.Condition});
            return LikeProducts;
        }

        // GET: api/LikeProducts/5
        /* [ResponseType(typeof(LikeProduct))]
         public IHttpActionResult GetLikeProduct(int id)
         {
             LikeProduct likeProduct = db.LikeProducts.Find(id);
             if (likeProduct == null)
             {
                 return NotFound();
             }

             return Ok(likeProduct);
         } */

        [HttpGet]
        [Route("api/GetLikeProducts")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeProducts(int id)
        {

            var list = db.Products.Join(db.LikeProducts, trav => trav.ProductID, ca => ca.ProductID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    ProductID = ca.ProductID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

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
        [Route("api/GetLikeProductsCount")]
        [AllowAnonymous]
        public int GetLikeProductsCount(int id)
        {

            var list = db.Products.Join(db.LikeProducts, trav => trav.ProductID, ca => ca.ProductID,
                           (trav, ca) => new {
                               likeId = ca.likeId,
                               ProductID = trav.ProductID,
                               UserID = ca.UserID,
                               DateTime = ca.DateTime,
                               Condition = ca.Condition

                           }
                         );

            var details = list.Where(c => c.ProductID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeProducts/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeProduct(int id, LikeProduct likeProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeProduct.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeProductExists(id))
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

        // POST: api/LikeProducts
        [AllowAnonymous]
        [ResponseType(typeof(LikeProduct))]
        public IHttpActionResult PostLikeProduct(LikeProduct likeProduct)
        {
            
            db.LikeProducts.Add(likeProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeProduct.likeId }, likeProduct);
        }

        // DELETE: api/LikeProducts/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeProduct))]
        public IHttpActionResult DeleteLikeProduct(int id)
        {
            LikeProduct likeProduct = db.LikeProducts.Find(id);
            if (likeProduct == null)
            {
                return NotFound();
            }

            db.LikeProducts.Remove(likeProduct);
            db.SaveChanges();

            return Ok(likeProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeProductExists(int id)
        {
            return db.LikeProducts.Count(e => e.likeId == id) > 0;
        }
    }
}
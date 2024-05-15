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
using WebApi.Models;

namespace WebApi.Controllers
{
    public class SellProductController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/SellProduct
        public IQueryable<SellProduct> GetSellProducts()
        {
            return db.SellProducts;
        }

        // GET: api/SellProduct/5
        [ResponseType(typeof(SellProduct))]
        public IHttpActionResult GetSellProduct(int id)
        {
            SellProduct sellProduct = db.SellProducts.Find(id);
            if (sellProduct == null)
            {
                return NotFound();
            }

            return Ok(sellProduct);
        }

        // PUT: api/SellProduct/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSellProduct(int id, SellProduct sellProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sellProduct.ProductID)
            {
                return BadRequest();
            }

            db.Entry(sellProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellProductExists(id))
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

        // POST: api/SellProduct
        [ResponseType(typeof(SellProduct))]
        public IHttpActionResult PostSellProduct(SellProduct sellProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SellProducts.Add(sellProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sellProduct.ProductID }, sellProduct);
        }

        // DELETE: api/SellProduct/5
        [ResponseType(typeof(SellProduct))]
        public IHttpActionResult DeleteSellProduct(int id)
        {
            SellProduct sellProduct = db.SellProducts.Find(id);
            if (sellProduct == null)
            {
                return NotFound();
            }

            db.SellProducts.Remove(sellProduct);
            db.SaveChanges();

            return Ok(sellProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SellProductExists(int id)
        {
            return db.SellProducts.Count(e => e.ProductID == id) > 0;
        }
    }
}
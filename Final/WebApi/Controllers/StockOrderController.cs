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
    public class StockOrderController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/StockOrder
        public IQueryable<StockOrder> GetStockOrders()
        {
            return db.StockOrders;
        }

        // GET: api/StockOrder/5
        [ResponseType(typeof(StockOrder))]
        public IHttpActionResult GetStockOrder(int id)
        {
            StockOrder stockOrder = db.StockOrders.Find(id);
            if (stockOrder == null)
            {
                return NotFound();
            }

            return Ok(stockOrder);
        }

        // PUT: api/StockOrder/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStockOrder(int id, StockOrder stockOrder)
        {
           

            if (id != stockOrder.OrderID)
            {
                return BadRequest();
            }

            db.Entry(stockOrder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockOrderExists(id))
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

        // POST: api/StockOrder
        [ResponseType(typeof(StockOrder))]
        public IHttpActionResult PostStockOrder(StockOrder stockOrder)
        {
           

            db.StockOrders.Add(stockOrder);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stockOrder.OrderID }, stockOrder);
        }

        // DELETE: api/StockOrder/5
        [ResponseType(typeof(StockOrder))]
        public IHttpActionResult DeleteStockOrder(int id)
        {
            StockOrder stockOrder = db.StockOrders.Find(id);
            if (stockOrder == null)
            {
                return NotFound();
            }

            db.StockOrders.Remove(stockOrder);
            db.SaveChanges();

            return Ok(stockOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StockOrderExists(int id)
        {
            return db.StockOrders.Count(e => e.OrderID == id) > 0;
        }
    }
}
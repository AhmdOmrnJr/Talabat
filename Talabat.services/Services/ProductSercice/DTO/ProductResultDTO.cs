using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.services.Services.ProductSercice.DTO
{
    public class ProductResultDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductBrandName { get; set; }
    }
}

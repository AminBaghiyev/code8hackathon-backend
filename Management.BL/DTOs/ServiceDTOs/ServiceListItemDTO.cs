using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.DTOs.ServiceDTOs
{
    public record ServiceListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using System.Collections.Generic;

namespace SanjeshP.RDC.Models
{
    public class SieveModelDto<TEntites>
    {
        public int TotalRecord { get; set; }
        public ICollection<TEntites> SelectDto { get; set; }
    }
}

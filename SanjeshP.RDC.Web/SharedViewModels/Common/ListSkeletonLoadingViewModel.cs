using System.Collections.Generic;

namespace SanjeshP.RDC.Web.SharedViewModels.Common
{
    public class ListSkeletonLoadingViewModel<TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; }
        public int NumberOfRows { get; set; }
    }
}

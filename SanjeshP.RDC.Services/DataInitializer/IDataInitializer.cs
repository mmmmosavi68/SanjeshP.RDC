using SanjeshP.RDC.Common;

namespace Services.RDC.DataInitializer
{
    public interface IDataInitializer : IScopedDependency
    {
        void InitializeData();
    }
}

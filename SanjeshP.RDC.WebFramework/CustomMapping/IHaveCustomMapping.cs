using AutoMapper;
using SanjeshP.RDC.Entities.User;

namespace WebFramework.CustomMapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}

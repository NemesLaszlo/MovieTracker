using AutoMapper;
using NetTopologySuite.Geometries;

namespace MovieTracker_API.MapperProfiles
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {

        }
    }
}

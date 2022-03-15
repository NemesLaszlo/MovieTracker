namespace MovieTracker_API.DTOs
{
    public class HomeDTO
    {
        public List<MovieDTO> InTheaters { get; set; }
        public List<MovieDTO> UpcomingReleases { get; set; }
    }
}

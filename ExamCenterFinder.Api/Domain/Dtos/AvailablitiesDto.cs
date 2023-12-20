namespace ExamCenterFinder.Api.Domain.Dtos
{
    public class ExamCenterDto
    {
        public int AvailabilityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime StartTime { get; set; }
        public int Seats { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceMiles { get; set; }
    }
    public class AvailablitiesDto
    {
        public IList<ExamCenterDto> Availability { get; set; }
    }
}

namespace ExamCenterFinder.Api.Domain
{
    public class ZipCodeCenterPoint
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

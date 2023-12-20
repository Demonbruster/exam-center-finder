namespace ExamCenterFinder.Api.Domain
{
    public class ExamCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCodeCenterPointId { get; set; }
        public ZipCodeCenterPoint ZipCodeCenterPoint { get; set; }

        public List<ExamSlot> ExamSlots { get; set; }
    }
}

namespace ExamCenterFinder.Api.Domain
{
    public class ExamSlot
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int TotalSeats { get; set; }
        public int ReservedSeats { get; set; }

        public int ExamCenterId { get; set; }
        public ExamCenter ExamCenter { get; set; }
    }
}

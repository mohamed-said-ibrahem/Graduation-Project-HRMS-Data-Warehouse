namespace HR_System.Models
{
    public class Position
    {
        private int positionId;
        private string positionName;
        private string positionDescription;

        public int PositionId { get => positionId; set => positionId = value; }
        public string PositionName { get => positionName; set => positionName = value; }
        public string PositionDescription { get => positionDescription; set => positionDescription = value; }
    }
}
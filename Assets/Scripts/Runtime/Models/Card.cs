namespace Runtime.Models
{
    public class Card
    {
        public Model Model { get; }
        public SegmentData SegmentData { get; private set; }

        public Card(Model model)
        {
            Model = model;
            SegmentData = SegmentData.Default(model);
        }

        public static readonly Card StartingCard = 
            new(Model.ConnectorBox) { SegmentData = SegmentData.None };
    }
}
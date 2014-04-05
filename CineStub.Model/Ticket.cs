namespace CineStub.Model
{
    public class Ticket : Entity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
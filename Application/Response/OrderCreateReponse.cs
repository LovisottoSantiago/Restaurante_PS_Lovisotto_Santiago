namespace Application.Response
{
    public class OrderCreateReponse
    {
        public long OrderNumber {  get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

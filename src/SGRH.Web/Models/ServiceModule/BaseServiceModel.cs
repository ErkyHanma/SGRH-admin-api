namespace SGRH.Web.Models.ServiceModule
{
    public abstract class BaseServiceModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
    }
}

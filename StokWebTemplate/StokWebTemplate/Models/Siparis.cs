namespace StokWeb.Models
{
    public class Siparis
    {
        public int SiparisId { get; set; }
        public string CariSirket { get; set; }
        public string StokAdi { get; set; }
        public string BirimKod { get; set; }
        public decimal Miktar { get; set; }
        public DateTime SiparisTarihi { get; set; }
        public string SiparisTur { get; set; }
    }
}
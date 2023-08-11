namespace Hastane.Models
{
    public class Ziyaretler
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ziyaret_id { get; set; }
        public int hasta_id { get; set; }
        public string ziyaret_tarihi { get; set; }
        public string doktor_adi { get; set; }
        public string sikayet { get; set; }
        public string tedavi_sekli { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StudentServisWebScraper.Api.Data
{
    public class JobOffer
    {
        public JobOffer()
        {
            this.DateAdded = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int Code { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateLastChanged { get; set; }

        public DateTime? DateRemoved { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public decimal? HourlyPay { get; set; }

        [IgnoreDataMember]
        public string UniqueText => $"{Category}#{Text}";

        public override string ToString()
        {
            return UniqueText;
        }

        public override int GetHashCode()
        {
            return UniqueText.GetHashCode();
        }
    }
}

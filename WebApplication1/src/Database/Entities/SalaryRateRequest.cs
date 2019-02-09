using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Database.Entities
{
    public class SalaryRateRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestChainId { get; set; }
        [Required] 
        public int SuggestedRate { get; set; }
        [Required] 
        public SalaryRateRequestStatus SalaryRateRequestStatus { get; set; }
        [Required] 
        public DateTime CreatedAt { get; set; }
        public int? ReviewerId { get; set; }
        public int SenderId { get; set; }
        public string ReviewerComment { get; set; }
        public string InternalComment { get; set; }
        [Required] 
        public string Reason { get; set; }


        [ForeignKey("SenderId")]
        [InverseProperty("SalaryRateRequests")]
        public User Sender { get; set; }
        [ForeignKey("ReviewerId")] 
        public User Reviewer { get; set; }
    }
}
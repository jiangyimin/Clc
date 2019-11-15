using System.ComponentModel.DataAnnotations;

namespace Clc.Issues.Dto
{
    public class IssueInputDto
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string ProcessStyle { get; set; }

        public int? LeaderId { get; set; }

    }
}
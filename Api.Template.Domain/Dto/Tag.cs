using System.ComponentModel.DataAnnotations;

namespace Api.Template.Domain.Dto
{
    public class Tag
    {
        public string? Urn { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? TagType { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? TagCategory { get; set; }

        public List<Property>? Properties { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? CreatedBy { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public DateTime? CreatedOn { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? UpdatedBy { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public DateTime UpdatedOn { get; set; }
    }
}

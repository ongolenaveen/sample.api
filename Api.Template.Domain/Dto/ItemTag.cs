using System.ComponentModel.DataAnnotations;

namespace Api.Template.Domain.Dto
{
    public class ItemTag
    {
        [Required(ErrorMessage = "{0} is required.")]
        public int? ItemId { get; set; }

        public Tag? Tags { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Tags == null)
                validationResults.Add(new ValidationResult("You must provide tags."));

            return validationResults;
        }
    }
}

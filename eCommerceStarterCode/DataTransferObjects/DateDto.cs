using System.ComponentModel.DataAnnotations;

namespace eCommerceStarterCode.DataTransferObjects
{
    public class DateDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}

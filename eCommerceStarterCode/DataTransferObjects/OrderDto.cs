using System.ComponentModel.DataAnnotations;

namespace eCommerceStarterCode.DataTransferObjects
{
    public class OrderDto : DateDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; }
    }
}

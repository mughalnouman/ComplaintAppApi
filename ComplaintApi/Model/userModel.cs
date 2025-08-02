using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ComplaintApi.Model
{
    [Table("userTable")]
    /// <summary>
    /// Represents a user entry in the userTable.
    /// </summary>
    public class userModel
    {
        /// <summary>
        /// Primary Key (Auto-incremented)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }

        /// <summary>
        /// User's email address (Required, 5-100 characters)
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [MinLength(5, ErrorMessage = "Email must be at least 5 characters")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        //string.empty is lye use kr rhy ha jb object create hoga is Class ka to string value nullable nhi jaye is se app crash hogi and error aye ga
        //    is lye jb yeah use kry gy to object m string value = "" empty aye gi null ki bjaye 
        public string email { get; set; } = string.Empty;

        /// <summary>
        /// User's mobile number (Required, exactly 11 digits)
        /// </summary>
        [Required(ErrorMessage = "Mobile number is required")]
        [MinLength(11, ErrorMessage = "Mobile number must be 11 digits")]
        [MaxLength(11, ErrorMessage = "Mobile number must be 11 digits")]
        //regular expression add kia ha is Se yeah hoga mobile number 11 se km or zyada nhi hoga
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile number must contain exactly 11 digits")]
        public string mobile { get; set; } = string.Empty;

        /// <summary>
        /// City name where the user lives (Required)
        /// </summary>
        [Required(ErrorMessage = "City is required")]
        public string city { get; set; } = string.Empty;

        /// <summary>
        /// State name (Optional field, can be left blank)
        /// </summary>
        public string? state { get; set; }

        /// <summary>
        /// Complete address of the user (Required, max 1000 characters)
        /// </summary>
        [Required(ErrorMessage = "Address is required")]
        [MaxLength(1000, ErrorMessage = "Address cannot exceed 1000 characters")]
        public string address { get; set; } = string.Empty;
    }
}

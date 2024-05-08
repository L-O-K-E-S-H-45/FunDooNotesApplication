using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "{0} should be between 3 and 50 characters")]
        //[RegularExpression(@"^[A-Z][a-z]{2,}", 
        //    ErrorMessage = "{0} must start with Uppercase & rest can have lower case alpbabates")]
        public string FirstName { get; set; }

        //[RegularExpression(@"^[A-Z][a-z]{2,20}", 
        //    ErrorMessage = "{0} must start with Uppercase & rest can have lower case alpbabates")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        //[StringLength(50, MinimumLength = 13, ErrorMessage = "{0} should be between 13 and 50 characters")]
        //[RegularExpression(@"^[a-z][a-z0-9]{2,}(\@gmail.com)", ErrorMessage = "Email must be in lowe case & must end with @gmail.com")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, " +
        //    "one lowercase letter, one digit, and one special character")]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

    }
}

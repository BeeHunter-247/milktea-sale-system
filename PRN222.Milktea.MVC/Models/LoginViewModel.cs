using System.ComponentModel.DataAnnotations;

namespace PRN222.Milktea.MVC.Models
{
	public class LoginViewModel
	{
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}

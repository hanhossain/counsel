namespace Counsel.Core.Models
{
	public class Player
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string FullName => $"{FirstName} {LastName}";

		public string Position { get; set; }

		public string Team { get; set; }
	}
}

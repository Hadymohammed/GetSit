using GetSit.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
	public class ReviewSpaceVM
	{
		[Required]
		public int SpaceId { get; set; }
		[Required]
		public Space Space { get; set; }
		[Required]	
		public SpaceEmployee ?spaceEmployee { get; set; }
		[Required]
		public HallRequest? hallRequest { get; set; }
		[Required]
		public SpaceHall Hall { get; set; }
		[AllowNull]
		public int? RequestId { get; set; }
	}
}

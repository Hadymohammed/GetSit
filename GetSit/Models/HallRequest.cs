using GetSit.Data.Base;
using GetSit.Data.enums;
using GetSit.Models;

namespace GetSit.Models
{
    public class HallRequest:IEntityBase
    {
        public int Id { get; set; }
        public String? comment { get; set; }
        public DateTime Date { get; set; }
        
        public ReqestStatus Status { get; set; }

        public int HallId { get; set; }
        public SpaceHall Hall { get; set; }
    }
}

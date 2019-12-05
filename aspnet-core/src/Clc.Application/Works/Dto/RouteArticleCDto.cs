using Clc.Fields;

namespace Clc.Works.Dto
{
    /// <summary>
    /// RouteArticleCDto
    /// </summary>
    public class RouteArticleCDto
    {
        public int ArticleId { get; set;}

        public string Rfid { get; set; }
        public string DisplayText { get; set; }

        public int RecordId { get; set; }
        public bool IsReturn { get; set; }
        
        public RouteArticleCDto()
        {}
        public RouteArticleCDto(Article a, int recordId = 0, bool isReturn=false)
        {
            ArticleId = a.Id;
            DisplayText = a.Cn + " " + a.Name;

            Rfid = a.Rfid;
            RecordId = recordId;
            IsReturn = isReturn;
        }
    }
}


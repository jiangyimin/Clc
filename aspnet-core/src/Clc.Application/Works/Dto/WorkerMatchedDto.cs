using System;
using Clc.Fields;
using Clc.Types;

namespace Clc.Works.Dto
{
    public class WorkerMatchedDto
    {
        public int RouteWorkerId { get; set; }

        // about Workers
        public int Id { get; set; }
        public string Cn { get; set; }
        public string Name { get; set; }

        public string Photo { get; set; }
        public string Rfid { get; set; }
        public string ArticleTypeList { get; set; }
        public string Duties { get; set; }

        public WorkerMatchedDto(int id, Worker w, WorkRole role)
        {
            RouteWorkerId = id;
            Id = w.Id;
            Cn = w.Cn;
            Name = w.Name;
            Rfid = w.Rfid;
            Photo = w.Photo != null ? Convert.ToBase64String(w.Photo) : null;
            ArticleTypeList = role.ArticleTypeList;
            Duties = role.Duties;
        }
    }
}
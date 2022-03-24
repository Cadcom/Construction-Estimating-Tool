using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Shared.Entities
{
    public class Account:BaseEntity
    {
        [EmailAddress]
        [Required]
        public string LoginEmail { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }

        //We'll use byte variable Like binary parameter values. For examples "000" => [read,write,delete]
        //sample "101" means for roles [read(true),write(false),delete(true)]
        public byte Roles { get; set; }

        public long BaseID { get; set; }
        public virtual List<Estimate> Estimates { get; set; }


        //if baseID=ID (main ID) it's own base account otherwise it's subuser account
        [NotMapped]
        public bool isBaseAccount { get {
                return ID == BaseID;    
            } 
        }

        [NotMapped]
        public string Token { get; set; }
    }
}

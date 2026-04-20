using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Import.Common.Models
{
    [Table("EMPLOYEE")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        [UsedImplicitly]
        public long Id { get; set; }

        [Column("FIRST_NAME", TypeName = "nvarchar(30)")]
        public string? FirstName { get; set; }

        [Column("LAST_NAME", TypeName = "nvarchar(30)")]
        public string? LastName { get; set; }

        [Column("BIRTH_DATE")]
        public DateTime BirthDate { get; set; }

        [Column("CURRENT_SALARY", TypeName = "numeric(10,2)")]
        public decimal CurrentSalary { get; set; }
    }
}

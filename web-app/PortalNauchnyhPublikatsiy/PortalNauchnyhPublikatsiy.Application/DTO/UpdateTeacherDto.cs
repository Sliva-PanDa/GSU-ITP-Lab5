using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PortalNauchnyhPublikatsiy.Application.DTO
{
    public class UpdateTeacherDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Должность обязательна")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ученая степень обязательна")]
        public string Degree { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо выбрать кафедру")]
        public int DepartmentId { get; set; }
    }
}

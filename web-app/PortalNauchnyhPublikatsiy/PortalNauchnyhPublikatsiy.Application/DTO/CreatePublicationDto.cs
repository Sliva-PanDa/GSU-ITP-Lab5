using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PortalNauchnyhPublikatsiy.Application.DTO
{
    public class CreatePublicationDto
    {
        [Required(ErrorMessage = "Название публикации обязательно для заполнения.")]
        [StringLength(500, ErrorMessage = "Название не может превышать 500 символов.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать тип публикации.")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать год публикации.")]
        [Range(1900, 2100, ErrorMessage = "Год должен быть в диапазоне от 1900 до 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать журнал или конференцию.")]
        public int JournalConferenceId { get; set; }

        [StringLength(255, ErrorMessage = "DOI не может превышать 255 символов.")]
        public string? DOI { get; set; }

        [StringLength(500, ErrorMessage = "Путь к файлу не может превышать 500 символов.")]
        public string? FilePath { get; set; }
    }
}

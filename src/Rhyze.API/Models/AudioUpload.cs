using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Rhyze.API.Models
{
    public class AudioUpload
    {
        [FromForm(Name = "tracks")]
        [Required(ErrorMessage = "Please supply one or more audio files in a multipart/form-data request.")]
        public IFormFileCollection Tracks { get; set; }
    }
}
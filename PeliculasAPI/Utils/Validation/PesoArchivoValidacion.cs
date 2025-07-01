using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Utils.Validation
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int _pesoMaximoEnMegaBytes;

        public PesoArchivoValidacion(int pesoMaximoEnMegaBytes)
        {
            _pesoMaximoEnMegaBytes = pesoMaximoEnMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > _pesoMaximoEnMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe superar {_pesoMaximoEnMegaBytes} MegaBytes");
            }

            return ValidationResult.Success;
        }
    }
}

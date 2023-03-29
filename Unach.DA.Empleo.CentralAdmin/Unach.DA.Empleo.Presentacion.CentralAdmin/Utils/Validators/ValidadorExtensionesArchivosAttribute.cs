using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Utils.Validators
{
    public class ValidadorExtensionesArchivosAttribute : ValidationAttribute, IClientModelValidator
    {
        private List<string> AllowedExtensions { get; set; }

        public ValidadorExtensionesArchivosAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!(file == null))
                {
                    if (!AllowedExtensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This pdf extension is not allowed!";
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            //context.Attributes.Add("data-val-archivo", error);

            MergeAttribute(context.Attributes, "data-val-requiredif", error);
            MergeAttribute(context.Attributes, "data-val-requiredif-dependentproperty", context.ModelMetadata.PropertyName);

            var desiredValue = AllowedExtensions.FirstOrDefault().ToString().ToLower();
            MergeAttribute(context.Attributes, "data-val-requiredif-desiredvalue", desiredValue);
            //MergeAttribute(context.Attributes, "data-val", "true");
            //MergeAttribute(context.Attributes, "data-val-archivo", GetErrorMessage());
        }



        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
        



    }
}

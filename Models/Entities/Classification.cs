using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModels
{
    public class Classification
    {
        public int Id {get; set;}

        public string ClassificationName {get; set;}
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineStub.Model
{
    public class Slot : Entity
    {
        public DateTime DateTime { get; set; }

        public bool IsOpen { get; set; }

        public bool IsRoot { get; set; } 
        
        
        public Movie Movie { get; set; }
    }
}
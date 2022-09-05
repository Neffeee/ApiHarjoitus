using System;
using System.Collections.Generic;

namespace WebAPIHarjoitusKoodi.Models
{
    public partial class Documentation
    {
        public int DocumentationId { get; set; }
        public string AvailableRoute { get; set; } = null!;
        public string? Method { get; set; }
        public string? Descriptions { get; set; }
        public string? Keycode { get; set; }
    }
}

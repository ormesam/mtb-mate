﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class TraceMessage
    {
        public int TraceMessageId { get; set; }
        public DateTime DateUtc { get; set; }
        public string Message { get; set; }
    }
}

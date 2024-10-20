﻿using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AdminPortal.Models
{
    public class Post
    {
        public int PostID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [JsonIgnore] public virtual User User { get; set; }
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionTimeUtc { get; set; }

        public virtual List<Like> Likes { get; set; }
        public string MediaType { get; set; }
        public string MediaLink { get; set; }

        [ForeignKey("Board")]
        public int BoardID { get; set; }
        [JsonIgnore] public virtual Board Board { get; set; }

        public string ViewDate() => TransactionTimeUtc.ToString("dd/MM/yyyy");
        public string ViewTime() => TransactionTimeUtc.ToString("hh:mm tt");
    }
}


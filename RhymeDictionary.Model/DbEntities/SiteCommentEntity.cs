using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhymeDictionary.Model.DbEntities
{
    [Table("SiteComments")]
    public class SiteCommentEntity
    {
        public SiteCommentEntity(Guid id, string email, string text, DateTime dateSavedUtc)
        {
            this.Id = id;
            this.Email = email;
            this.Text = text;
            this.DateSavedUtc = dateSavedUtc;
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Text { get; private set; }

        public DateTime DateSavedUtc { get; private set; }
    }
}

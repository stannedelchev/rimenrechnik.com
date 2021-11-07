using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhymeDictionary.Migration.DapperFastCrudEntities
{
    /*
     *
     *
     *
     *
     * These classes are copied from the ones in RhymeDictionary.Model.DbEntities, which have both entity- and business- logic in them.
     * This business logic features private setters and computed properties, which for some reason confuses Dapper.FastCrud
     * when making Insert()s and it uses null values for properties with a private setter.
     * So instead of using the .Model.DbEntities classes for both retrieving Azure/SQL Server tables and inserting to PostgreSQL,
     * we have a separate copy for each purpose.
     * Ideally this can be improved by providing some transformation like AutoMapper or copy constructors from .DbEntities to .DapperFastCrud.
     * Given this is a one time job and the fields are almost exactly the same, there's no need for such improvement at the moment.
     *
     *
     *
     *
     *
     */


    [Table("WordSuggestions")]
    public class WordSuggestionEntity
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public string IpAddress { get; set; }

        public DateTime DateAddedUtc { get; set; }
    }

    [Table("WordSearchVisits")]
    public class WordSearchVisitEntity
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public DateTime DateSearchedUtc { get; set; }

        public string IpAddress { get; set; }
    }

    [Table("Words")]
    public class WordEntity
    {
        public Guid Id { get; set; }
        public string Normal { get; set; }
        public string Forward { get; set; }
        public string Reverse { get; set; }
        public string ReverseEnding { get; set; }
    }

    [Table("WordAdditions")]
    public class WordAdditionEntity
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public Guid? WordId { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime DateAddedUtc { get; set; }

        public string IpAddress { get; set; }
    }

    [Table("SiteComments")]
    public class SiteCommentEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public DateTime DateSavedUtc { get; set; }
    }
}
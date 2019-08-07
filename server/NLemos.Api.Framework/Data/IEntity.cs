using System;

namespace NLemos.Api.Framework.Data
{
    /// <summary>
    /// This is the least required to be written into database
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The key of the entity.
        /// If not set, the <see cref="SqlDbContext"/> will set it to <see cref="Guid"/>.<see cref="Guid.NewGuid"/>
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The DateTime when the entity was first written to database.
        /// </summary>
        /// <remarks>
        ///     See https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=netframework-4.8 
        ///     and https://stackoverflow.com/a/14268167 for why to use DateTimeOffset
        /// </remarks>
        DateTimeOffset Added { get; set; }
        DateTimeOffset Modified { get; set; }
    }
}

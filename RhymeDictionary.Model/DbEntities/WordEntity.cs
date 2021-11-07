using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace RhymeDictionary.Model.DbEntities
{
    [DebuggerDisplay("{Forward}: {Normal}")]
    [Table("Words")]
    public class WordEntity
    {
        private string forward;

        // Ex: B830F170-CD3B-4F4C-919B-000363A40C00
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the normal written representation of the word.
        /// </summary>
        /// <remarks>работнически</remarks>
        public string Normal { get; set; }

        /// <summary>
        /// Gets or sets the forward-readable version of the word, with stress apostrophe added before its vowel.
        /// </summary>
        /// <remarks>раб'отнически</remarks>
        public string Forward
        {
            get
            {
                return this.forward;
            }

            set
            {
                this.forward = value ?? throw new ArgumentException();
                this.SetEndings(value);
            }
        }

        /// <summary>
        /// Gets or sets the reversed representation of the word, with stress apostrophe after the vowel.
        /// </summary>
        /// <remarks>иксечинто'бар</remarks>
        public string Reverse { get; set; }

        /// <summary>
        /// Gets the ending of the word - everything after the stress apostrophe, including the stress.
        /// </summary>
        /// <remarks>'отнически</remarks>
        public string Ending { get; private set; }

        /// <summary>
        /// Gets the reversed representation of the ending of the word, stress apostrophe at the end of the string.
        /// </summary>
        /// <remarks>иксечинто'</remarks>
        public string ReverseEnding { get; private set; }

        /// <summary>
        /// Gets the number of syllables in the word.
        /// </summary>
        /// <remarks>5</remarks>
        public int Syllables => new SyllableCounter().Count(this.Normal);

        /// <summary>
        /// Creates a <see cref="WordEntity"/> from the forward stressed representation of the word, given as an apostrophe.
        /// </summary>
        /// <param name="forward">Forward representation of the word, ex: раб'отнически.</param>
        /// <returns>The created <see cref="WordEntity"/>.</returns>
        public static WordEntity FromForward(string forward)
        {
            var result = new WordEntity
            {
                Id = Guid.NewGuid(),
                Normal = forward.Replace("'", string.Empty),
                Reverse = new string(forward.Reverse().ToArray()),
                Forward = forward
            };

            return result;
        }

        private void SetEndings(string forwardValue)
        {
            var stressMarkIndex = forwardValue.LastIndexOf("'", StringComparison.Ordinal);
            if (stressMarkIndex != -1)
            {
                this.Ending = forwardValue.Substring(stressMarkIndex);
                this.ReverseEnding = new string(this.Ending.Reverse().ToArray());
            }
            else
            {
                this.Ending = null;
                this.ReverseEnding = null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Crawler
{
    public class Config
    {
        /// <summary>
        ///     Gets or sets the configuration for this service.
        /// </summary>
        /// <value>
        ///     The Configuration from the application.
        /// </value>
        public SelfConfig Self { get; set; }


        /// <summary>
        ///     Configuration needed for this service.
        /// </summary>
        public class SelfConfig
        {
            /// <summary>
            ///     Gets or sets the name of this service.
            /// </summary>
            /// <value>
            ///     The name of the this service.
            /// </value>
            [Required]
            public string MetaDataFileName { get; set; }

            /// <summary>
            ///     Gets or sets the Github token for authentication.
            /// </summary>
            /// <value>
            ///     The Github Token that is used for authentication.
            /// </value>
            [Required]
            public string GithubToken { get; set; }

            /// <summary>
            ///     Gets or sets the boolean that indicates whether or not your meta data is based on yaml or json
            /// true if yaml, false otherwise
            /// </summary>
            /// <value>
            ///     The boolean whether or not to use yaml for the meta data file.
            /// </value>
            [Required]
            public bool YamlMode { get; set; }

            /// <summary>
            ///     Gets or sets the boolean that indicates whether or you save to database or to a flat repos.json
            /// true if database, false if files
            /// </summary>
            /// <value>
            ///     The boolean whether or not to use database to save output.
            /// </value>
            [Required]
            public bool DatabaseMode { get; set; }

            /// <summary>
            ///     Gets or sets the Github organization to query.
            /// </summary>
            /// <value>
            ///     The Github Organization to query for.
            /// </value>
            [Required]
            public string GithubOrganization { get; set; }
        }

}
}

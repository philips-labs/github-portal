using System;
using System.Collections.Generic;
using System.Text;
using Octokit;

namespace Crawler
{
    public class CrawlerRepositoryResult
    {
        public CrawlerRepositoryResult(Repository repository, dynamic metaData, string contributorFileUrl, CommitActivity commitActivity)
        {
            Repository = repository;
            CommitActivity = commitActivity;
            ContributorFileUrl = contributorFileUrl;
            MetaData = metaData;
        }
        public Repository Repository { get; set; }
        public dynamic MetaData { get; set; }
        public string ContributorFileUrl { get; set; }
        public int RepositoryScore { get; set; }
        public CommitActivity CommitActivity { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Permissions
    {
        [JsonPropertyName("Admin")]
        public bool Admin { get; set; }

        [JsonPropertyName("Push")]
        public bool Push { get; set; }

        [JsonPropertyName("Pull")]
        public bool Pull { get; set; }
    }

    public class License
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }

        [JsonPropertyName("NodeId")]
        public string NodeId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("SpdxId")]
        public string SpdxId { get; set; }

        [JsonPropertyName("Url")]
        public object Url { get; set; }

        [JsonPropertyName("Featured")]
        public bool Featured { get; set; }
    }

    public class Repository
    {
        [JsonPropertyName("Url")]
        public string Url { get; set; }

        [JsonPropertyName("HtmlUrl")]
        public string HtmlUrl { get; set; }

        [JsonPropertyName("CloneUrl")]
        public string CloneUrl { get; set; }

        [JsonPropertyName("GitUrl")]
        public string GitUrl { get; set; }

        [JsonPropertyName("SshUrl")]
        public string SshUrl { get; set; }

        [JsonPropertyName("SvnUrl")]
        public string SvnUrl { get; set; }

        [JsonPropertyName("MirrorUrl")]
        public object MirrorUrl { get; set; }

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("NodeId")]
        public string NodeId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("FullName")]
        public string FullName { get; set; }

        [JsonPropertyName("IsTemplate")]
        public bool IsTemplate { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Homepage")]
        public string Homepage { get; set; }

        [JsonPropertyName("Language")]
        public string Language { get; set; }

        [JsonPropertyName("Private")]
        public bool Private { get; set; }

        [JsonPropertyName("Fork")]
        public bool Fork { get; set; }

        [JsonPropertyName("ForksCount")]
        public int ForksCount { get; set; }

        [JsonPropertyName("StargazersCount")]
        public int StargazersCount { get; set; }

        [JsonPropertyName("WatchersCount")]
        public int WatchersCount { get; set; }

        [JsonPropertyName("DefaultBranch")]
        public string DefaultBranch { get; set; }

        [JsonPropertyName("OpenIssuesCount")]
        public int OpenIssuesCount { get; set; }

        [JsonPropertyName("PushedAt")]
        public DateTime PushedAt { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("Permissions")]
        public Permissions Permissions { get; set; }

        [JsonPropertyName("Parent")]
        public object Parent { get; set; }

        [JsonPropertyName("Source")]
        public object Source { get; set; }

        [JsonPropertyName("License")]
        public License License { get; set; }

        [JsonPropertyName("HasIssues")]
        public bool HasIssues { get; set; }

        [JsonPropertyName("HasWiki")]
        public bool HasWiki { get; set; }

        [JsonPropertyName("HasDownloads")]
        public bool HasDownloads { get; set; }

        [JsonPropertyName("AllowRebaseMerge")]
        public object AllowRebaseMerge { get; set; }

        [JsonPropertyName("AllowSquashMerge")]
        public object AllowSquashMerge { get; set; }

        [JsonPropertyName("AllowMergeCommit")]
        public object AllowMergeCommit { get; set; }

        [JsonPropertyName("HasPages")]
        public bool HasPages { get; set; }

        [JsonPropertyName("SubscribersCount")]
        public int SubscribersCount { get; set; }

        [JsonPropertyName("Size")]
        public int Size { get; set; }

        [JsonPropertyName("Archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("DeleteBranchOnMerge")]
        public object DeleteBranchOnMerge { get; set; }

        [JsonPropertyName("Visibility")]
        public object Visibility { get; set; }

        [JsonPropertyName("Topics")]
        public List<string> Topics { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("business-category")]
        public string BusinessCategory { get; set; }

        [JsonPropertyName("business-category-details")]
        public string BusinessCategoryDetails { get; set; }
    }

    public class Activity
    {
        [JsonPropertyName("Days")]
        public List<int> Days { get; set; }

        [JsonPropertyName("Total")]
        public int Total { get; set; }

        [JsonPropertyName("Week")]
        public int Week { get; set; }

        [JsonPropertyName("WeekTimestamp")]
        public DateTime WeekTimestamp { get; set; }
    }

    public class CommitActivity
    {
        [JsonPropertyName("Activity")]
        public List<Activity> Activity { get; set; }
    }


    public class CrawlerResult
    {
        [JsonPropertyName("Repository")]
        public Repository Repository { get; set; }

        [JsonPropertyName("MetaData")]
        public MetaData MetaData { get; set; }

        [JsonPropertyName("ContributorFileUrl")]
        public string ContributorFileUrl { get; set; }

        [JsonPropertyName("RepositoryScore")]
        public int RepositoryScore { get; set; }

        [JsonPropertyName("CommitActivity")]
        public CommitActivity CommitActivity { get; set; }
    }


}

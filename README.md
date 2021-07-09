## GitHub Portal
Lists all InnerSource projects of Philips in an interactive and easy to use way. Can be used as a template for implementing the [InnerSource Portal pattern](https://patterns.innersourcecommons.org/p/innersource-portal) by the [InnerSource Commons](http://innersourcecommons.org/) community.

## Inspiration
This project is heavily inspired by the [SAP Project Portal for InnerSource](https://github.com/SAP/project-portal-for-innersource).

## State
Work in progress - Functional, but not ready for production.

Functional Crawler.
Functional Portal.

## Technology Stack
* [Blazor Web Assembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)  
* .NET Core 3.1
* [TailwindCSS](https://tailwindcss.com/)

## Crawler
The crawler is based on the InnerSource Crawler on Philips-forks. This one is built with C# and runs as a console application. It's configurable to an extend. Configuration options are discussed in detail below.

### Configuration
Use environment variables to customize the values in the config file (appsettings.json).
Available environment variables are:

| Environment Variable  | Description | Default value |
| ------------ | --------------- | --------------- |
| App__Self__MetaDataFileName | Name of the meta data file | philips-repo.yaml
| App__Self__GithubToken | Authentication token used to authenticate API requests | token
| App__Self__YamlMode | Is the meta data file a YAML/YML file or is it a JSON? If Yaml, true, if JSON, false | true
| App__Self__DatabaseMode | Do you want to output the default repos.json or output to a database? | false
| App__Self__GithubOrganization | Name of the Github Organization to query | philips-internal

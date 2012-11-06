namespace Honey
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using Extensions;

    public class BluePrintParser
    {
        private readonly List<HttpMethod> httpMethods = new List<HttpMethod>()
        {
            HttpMethod.Get,
            HttpMethod.Post,
            HttpMethod.Put,
            HttpMethod.Delete
        };

        public IEnumerable<IGrouping<string, BluePrintResource>> Parse(string blueprint)
        {
            return blueprint
                .Split(Environment.NewLine.ToCharArray())
                .Where(line => httpMethods.Select(httpMethod => httpMethod.Method.ToUpper()).Contains(line.Split('/').First().Trim()))
                .Select(line => line.Split('/'))
                .Select
                (
                    line => new BluePrintResource
                    {
                        Method = line.First().Trim().ToTitleCase(),
                        Name = (line[1].Contains("?") ? line[1].Split('?').First().Replace("{", String.Empty) : line[1]).ToTitleCase(),
                        IdParameter = line.Count() <= 2 ? null : line[2].Replace("{", String.Empty).Replace("}", String.Empty),
                        Parameters = new BluePrintParameters().Parse(line.Join(String.Empty))

                    }
                )
                .GroupBy(resource => resource.Name);
        }

        public IEnumerable<IGrouping<string, BluePrintResource>> ParseFile(string filePath)
        {
            return Parse(File.ReadAllText(filePath));
        }
    }
}

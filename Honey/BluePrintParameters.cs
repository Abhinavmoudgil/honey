namespace Honey
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class BluePrintParameters
    {
        public Dictionary<string, string> Value { get; set; }

        public override string ToString()
        {
            return string.Join("&", Value.Select(pair => "{0}={1}".FormatWith(pair.Key)));
        }

        public BluePrintParameters Parse(string template)
        {
            var parameters = template.Split('?');

            if (parameters.Length != 2)
            {
                return null;
            }

            Value = parameters[1].Substring(0, parameters[1].Length - 1).Split(',').ToDictionary(name => name, type => "object");

            return this;
        }
    }
}
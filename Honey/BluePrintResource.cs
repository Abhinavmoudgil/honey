namespace Honey
{
    using Extensions;

    public class BluePrintResource
    {
        public string Method { get; set; }

        public string Name { get; set; }

        public string IdParameter { get; set; }

        public bool HasIdParameter
        {
            get
            {
                return IdParameter.HasValue();
            }
        }

        public BluePrintParameters Parameters { get; set; }

        public bool HasParameters
        {
            get
            {
                return Parameters != null;
            }
        }
    }
}
namespace Domain
{
    public class Country
    {
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Name { get; set; }
        public string Capital { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public List<State> States { get; set; }
    }
}
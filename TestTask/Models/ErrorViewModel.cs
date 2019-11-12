using System;
using System.Collections.Generic;

namespace TestTask.Models
{
    public class ListValuts
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public string Nominal { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Dynamic
    {
        public string ID { get; set; }
        public string Date { get; set; }
        public string Nominal { get; set; }
        public string ValueDynamic { get; set; }
    }
    public class IndexViewModel
    {
        public IEnumerable<ListValuts> ListValuts { get; set; }
        public IEnumerable<Dynamic> Dynamic { get; set; }

    }
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

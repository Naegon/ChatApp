using System;
namespace Tests
{
    public class Test
    {
        private string _title;

        public string Title { get => _title; set => _title = value; }

        public Test(string title)
        {
            _title = title;
        }

        public override string ToString()
        {
            return _title;
        }
    }
}

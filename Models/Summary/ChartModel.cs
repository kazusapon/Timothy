using System;
using System.Collections.Generic;
using System.Linq;
using Timothy.Models;

namespace Timothy.Models.Summary
{
    public class PieModel
    {
        public List<string> Labels {get; set;}

        public List<int> Datasets {get; set;}
    }

    public class ChartModel
    {
        public List<string> Labels {get; set;}

        //public List<int> Dataset {get; set;}
        public List<DatasetModel> Datasets {get; set;}
    }

    public class DatasetModel
    {
        public string Label {get; set;}

        public List<int> Data {get; set;}
    }
    
    public class SystemsCountModel
    {
        public Entities.System System {get; set;}

        public int? YearOrMonth {get; set;}

        public int InquiryCount {get; set;}
    }

    public class GuestTypePieChartModel
    {
        public Entities.GuestType GuestType {get; set;}

        public int InquiryCount {get; set;}
    }
}
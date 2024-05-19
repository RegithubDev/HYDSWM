using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace COMMON.COLLECTION
{
    public class NTripInfo
    {
        public int TripId { get; set; }
        public string VehicleUId { get; set; }
        public string TId { get; set; }
        public int BufferMin { get; set; }
    }
    public class NRouteStopInfo
    {
        public string StopId { get; set; }
        //public TimeSpan Arvltime { get; set; }
        //public TimeSpan DeptTime { get; set; }
        public TimeSpan PickupTime { get; set; }
        public int TripId { get; set; }
        public string VehicleUId { get; set; }
        public int BufferMin { get; set; }
        public int PointCatId { get; set; }
    }
    public class RouteNTripInfo
    {
        private DateTime? _LastRTDate;
        private DateTime? _TripRStartDate;
        private string _name;
        public int RowNumber { get; set; }
        public int TotalCount { get; set; }
        public string RouteCode { get; set; }
        public int RouteId { get; set; }
        public int TotalDistance { get; set; }
        public DateTime? TripRStartDate
        {
            get
            {
                return _TripRStartDate;
            }
            set
            {
                _TripRStartDate = value;
            }
        }
        public DateTime? LastRTDate
        {
            get
            {
                return _LastRTDate;
            }
            set
            {
                _LastRTDate = value;
            }
        }
        public string TotalDuration
        {
            get
            {
                string Result = "0(Min)";
                TimeSpan? span = this._LastRTDate.HasValue ? this._LastRTDate.Value.Subtract(this._TripRStartDate.Value) : (TimeSpan?)null;
                int minutes = Convert.ToInt32(span.HasValue ? span.Value.TotalMinutes : 0);
                //  Result = minutes.ToString()+ "(Min)";
                Result = span.HasValue ? span.Value.ToString(@"hh\:mm") : "00:00";
                return Result;
            }
        }
        public int TotalTrips { get; set; }
        public int TotalPoints { get; set; }
        public int TotalCollectedPoint { get; set; }
        public int TotalNewCollectedPoint { get; set; }
        // public string RouteTripInfo { get; set; }
        public string RouteTripInfo
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public List<RTripInfo> SRouteTripInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<RTripInfo>>(_name);
            }
        }
    }
    public class RTripInfo
    {
        private DateTime? _LastTDate;
        private DateTime? _FirstTDate;
        private DateTime? _TripStartDate;
        private int _TotalPoint;
        private int _TotalCollectedPoint;
        private int _TotalNewCollectedPoint;
        private int _TotalCPoint;
        private int _TripStatus;
        private int _TotalHSPoint;
        private int _TotalCollectedHSPoint;
        public int TripId { get; set; }
        public string TripName { get; set; }
        public int TotalDistance { get; set; }
        public string TId { get; set; }
        public string TRefId { get; set; }
        public string ShiftName { get; set; }
        public string TotalDuration
        {
            get
            {
                string Result = "0(Min)";
                TimeSpan? span = this._LastTDate.HasValue ? this._LastTDate.Value.Subtract(this._TripStartDate.Value) : (TimeSpan?)null;
                int minutes = Convert.ToInt32(span.HasValue ? span.Value.TotalMinutes : 0);
                //  Result = minutes.ToString()+ "(Min)";
                Result = span.HasValue ? span.Value.ToString(@"hh\:mm") : "00:00";
                return Result;
            }
        }

        public string LastCollectedPoint { get; set; }
        public string LastScannedOn { get; set; }
        public string SLatLng { get; set; }
        public string ELatLng { get; set; }
        public int TotalPoint
        {
            get
            {
                return _TotalPoint;
            }
            set
            {
                _TotalPoint = value;
            }
        }
        public int TotalCollectedPoint
        {
            get
            {
                return _TotalCollectedPoint;
            }
            set
            {
                _TotalCollectedPoint = value;
            }
        }
        public int TotalNewCollectedPoint
        {
            get
            {
                return _TotalNewCollectedPoint;
            }
            set
            {
                _TotalNewCollectedPoint = value;
            }
        }

        public DateTime? LastTDate
        {
            get
            {
                return _LastTDate;
            }
            set
            {
                _LastTDate = value;
            }
        }
        public DateTime? TripStartDate
        {
            get
            {
                return _TripStartDate;
            }
            set
            {
                _TripStartDate = value;
            }
        }
        public DateTime? FirstTDate
        {
            get
            {
                return _FirstTDate;
            }
            set
            {
                _FirstTDate = value;
            }
        }
        public string Status
        {
            get
            {
                string Result = string.Empty;
                int TotalCPoint = this._TotalNewCollectedPoint + this._TotalCollectedPoint;
                this._TotalCPoint = TotalCPoint;
                // for new version
                if (this.TripStatus == 2)
                    Result = "COMPLETED";
                else
                {
                    if (TotalCPoint >= this.TotalPoint)
                        Result = "COMPLETED";
                    else if (TotalCPoint > 0 && TotalCPoint < this.TotalPoint)
                        Result = "IN PROGRESS";
                    else if (TotalCPoint == 0 && _TripStartDate.HasValue)
                        Result = "STARTED";
                    else if (TotalCPoint == 0 && !_TripStartDate.HasValue)
                        Result = "NOT STARTED";
                }

                //for older version
                //if (TotalCPoint >= this.TotalPoint)
                //    Result = "COMPLETED";
                //else if (TotalCPoint > 0 && TotalCPoint < this.TotalPoint)
                //    Result = "IN PROGRESS";
                //else if (TotalCPoint == 0 && _TripStartDate.HasValue)
                //    Result = "STARTED";
                //else if (TotalCPoint == 0 && !_TripStartDate.HasValue)
                //    Result = "NOT STARTED";

                return Result;
            }
        }
        public int TripStatus
        {
            get
            {
                return _TripStatus;
            }
            set
            {
                _TripStatus = value;
            }
        }
        public int TotalHSPoint
        {
            get
            {
                return _TotalHSPoint;
            }
            set
            {
                _TotalHSPoint = value;
            }
        }
        public int TotalCollectedHSPoint
        {
            get
            {
                return _TotalCollectedHSPoint;
            }
            set
            {
                _TotalCollectedHSPoint = value;
            }
        }
        public int IsCompleted
        {
            get
            {
                int Result = 0;

                if (this._TripStatus == 1)
                {
                    //for older version
                    // if ((this._TotalCPoint) >= (this._TotalPoint - this._TotalHSPoint))
                    if ((this._TotalCPoint) >= 1)//(this._TotalPoint - this._TotalHSPoint))
                        Result = 3;
                    else
                        Result = _TripStatus;
                }
                else
                    Result = this._TripStatus;
                return Result;
            }
        }
    }
    public class NPointTimelineInfo
    {

        private string _name;
        public int RouteId { get; set; }
        public DateTime TDate { get; set; }
        public string STDate { get; set; }
        public string RouteCode { get; set; }
        public string RouteName { get; set; }
        public string TripName { get; set; }
        public int TotalPoints { get; set; }
        public string ShiftName { get; set; }
        public int TotalCollectedPoints { get; set; }
        public string RouteStopInfo
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public List<PointChildTimeline> SRouteTripInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<PointChildTimeline>>(_name);
            }
        }

    }

    public class PointChildTimeline
    {

        public string RowNumber { get; set; }
        public int StopId { get; set; }
        public string GeoPointName { get; set; }
        public string SPickupTime { get; set; }
        public string PointName { get; set; }
        public string VehicleNo { get; set; }
        public int RouteId { get; set; }
        public string TDate { get; set; }
        public string STDate { get; set; }
        public string SAfterDate { get; set; }
        public string Status { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string SchPickupTime { get; set; }
        public int ColorId { get; set; }
        public string TripName { get; set; }
        public int PointCatId { get; set; }
        public string BeforeImage { get; set; }
        public string AfterImage { get; set; }
        public string Remarks { get; set; }
        public int PClIdSS { get; set; }
    }
    public class NEmrPointTimelineInfo
    {

        private string _name;
        public int RouteId { get; set; }
        public DateTime TDate { get; set; }
        public string STDate { get; set; }

        public string RouteName { get; set; }

        public int TotalPoints { get; set; }
        public int TotalCollectedPoints { get; set; }
        public string RouteStopInfo
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public List<EmrPointChildTimeline> SRouteTripInfo
        {
            get
            {
                return JsonConvert.DeserializeObject<List<EmrPointChildTimeline>>(_name);
            }
        }

    }
    public class EmrPointChildTimeline
    {

        public string RowNumber { get; set; }
        public int StopId { get; set; }
        public string GeoPointName { get; set; }
        public string SPickupTime { get; set; }
        public string PointName { get; set; }
        public string VehicleNo { get; set; }

        public string TDate { get; set; }
        public string STDate { get; set; }
        public string SAfterDate { get; set; }
        public string Status { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string SchPickupTime { get; set; }
        public int ColorId { get; set; }

        public int PointCatId { get; set; }

    }
}

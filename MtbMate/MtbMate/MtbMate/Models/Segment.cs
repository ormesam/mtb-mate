﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate.Models {
    public class Segment {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<SegmentLocation> Points { get; set; }
        public DateTime Created { get; set; }
        public SegmentLocation Start => Points.FirstOrDefault();
        public SegmentLocation End => Points.LastOrDefault();
        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Created.ToString("dd/MM/yy HH:mm") : Name;

        public LatLng GetClosestStartPoint(IList<LatLng> locations) {
            return GetClosestPoint(locations, Start.Point);
        }

        public LatLng GetClosestEndPoint(IList<LatLng> locations) {
            return GetClosestPoint(locations, End.Point);
        }

        private LatLng GetClosestPoint(IList<LatLng> locations, LatLng point) {
            LatLng closestLocation = null;
            double lastDistance = double.MaxValue;

            foreach (var location in locations) {
                double distance = location.CalculateDistance(point);

                if (distance < lastDistance) {
                    lastDistance = distance;
                    closestLocation = location;
                }
            }

            return closestLocation;
        }

        public ShareFile GetLocationFile() {
            StringBuilder sb = new StringBuilder();

            foreach (var point in Points) {
                sb.AppendLine($"S,{point.Point.Latitude},{point.Point.Longitude},E");
            }

            sb.AppendLine();

            string fileName = "Segment Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }
    }
}

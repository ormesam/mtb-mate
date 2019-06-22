﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MtbMate.Utilities;

namespace MtbMate.Models
{
    public class RideModel
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IList<LocationModel> Locations { get; set; }
        public IList<JumpModel> Jumps { get; set; }
        public IList<AccelerometerReadingModel> AccelerometerReadings { get; set; }

        public RideModel()
        {
            Locations = new List<LocationModel>();
            Jumps = new List<JumpModel>();
            AccelerometerReadings = new List<AccelerometerReadingModel>();
        }

        public async Task StartRide()
        {
            Start = DateTime.UtcNow;

            AccelerometerUtility.Instance.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            await GeoUtility.Instance.Start();
            AccelerometerUtility.Instance.Start();
        }

        public async Task StopRide()
        {
            End = DateTime.UtcNow;

            await GeoUtility.Instance.Stop();
            AccelerometerUtility.Instance.Stop();

            AccelerometerUtility.Instance.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            CheckForJumpsAndDrops();
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e)
        {
            AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e)
        {
            Locations.Add(e.Location);
        }

        private void CheckForJumpsAndDrops()
        {
            bool previousReadingBelowLowerLimit = false;
            double landingUpperLimit = 3;
            double takeoffUpperLimit = 2;
            double lowerLimit = -1;
            // temp for now
            TimeSpan jumpAllowance = TimeSpan.FromSeconds(1.5);

            var dropReadings = new List<AccelerometerReadingModel>();

            foreach (var reading in AccelerometerReadings)
            {
                // The z index is lower than the lower limit when at the top of the jump.
                if (reading.Z > lowerLimit)
                {
                    previousReadingBelowLowerLimit = false;
                    continue;
                }

                if (previousReadingBelowLowerLimit)
                {
                    continue;
                }

                previousReadingBelowLowerLimit = true;

                dropReadings.Add(reading);
            }

            foreach (var drop in dropReadings)
            {
                var readingsBeforeDrop = AccelerometerReadings
                    .Where(i => i.TimeStamp >= drop.TimeStamp - jumpAllowance)
                    .Where(i => i.TimeStamp <= drop.TimeStamp);

                var readingsAfterDrop = AccelerometerReadings
                    .Where(i => i.TimeStamp <= drop.TimeStamp + jumpAllowance)
                    .Where(i => i.TimeStamp >= drop.TimeStamp);

                var takeOffReading = readingsBeforeDrop
                    .Where(i => i.Z > takeoffUpperLimit)
                    .Where(i => i.Z == readingsBeforeDrop.Max(j => j.Z))
                    .FirstOrDefault();

                var landingReading = readingsAfterDrop
                    .Where(i => i.Z > landingUpperLimit)
                    .Where(i => i.Z == readingsAfterDrop.Max(j => j.Z))
                    .FirstOrDefault();

                if (takeOffReading == null || landingReading == null)
                {
                    continue;
                }

                JumpModel jump = new JumpModel
                {
                    TakeOffGForce = takeOffReading.Z,
                    TakeOffTimeStamp = takeOffReading.TimeStamp,
                    LandingGForce = landingReading.Z,
                    LandingTimeStamp = landingReading.TimeStamp,
                };

                Jumps.Add(jump);
            }
        }

        public string GetReadings()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"TimeStamp,X,Y,Z");

            foreach (var reading in AccelerometerReadings)
            {
                sb.AppendLine($"{reading.TimeStamp.ToString("hh:mm:ss.fff")},{reading.X},{reading.Y},{reading.Z}");
            }

            return sb.ToString();
        }
    }
}

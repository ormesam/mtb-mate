﻿using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.JumpDetection;

namespace Tracked.Screens.Review {
    internal class AccelerometerReadingsScreenViewModel : ViewModelBase {
        private readonly RideDto ride;

        public PlotModel AccelerometerChartModel { get; }

        public AccelerometerReadingsScreenViewModel(MainContext context, RideDto ride)
            : base(context) {
            this.ride = ride;

            AccelerometerChartModel = CreateAccelerometerChartModel();
        }

        public override string Title => "Ride";

        private PlotModel CreateAccelerometerChartModel() {
            var plot = new PlotModel {
                Title = "Accelerometer",
                Axes = {
                    new DateTimeAxis {
                        Position = AxisPosition.Bottom,
                    },
                    new LinearAxis {
                        Position = AxisPosition.Left,
                        MinimumPadding = 0,
                        IsZoomEnabled = false,
                    },
                },
                //Series = {
                //    new LineSeries()
                //    {
                //          ItemsSource = ride.AccelerometerReadings
                //            .OrderBy(i => i.Timestamp)
                //            .Select(i => new {
                //                x = i.Timestamp,
                //                y = i.X,
                //            })
                //            .ToList(),
                //          DataFieldX = "x",
                //          DataFieldY = "y",
                //    },
                //}
            };

            //foreach (var jump in ride.Jumps) {
            //    plot.Series.Add(new LineSeries {
            //        ItemsSource = jump.Readings
            //            .OrderBy(i => i.Timestamp)
            //            .Select(i => new {
            //                x = i.Timestamp,
            //                y = i.X,
            //            })
            //            .ToList(),
            //        DataFieldX = "x",
            //        DataFieldY = "y",
            //        Color = OxyPlot.OxyColor.FromRgb(255, 0, 0),
            //    });
            //}

            LineAnnotation baseLine = new LineAnnotation() {
                StrokeThickness = 1,
                Color = OxyColors.Black,
                Type = LineAnnotationType.Horizontal,
                Y = 0,
            };

            LineAnnotation maxToleranceLine = new LineAnnotation() {
                StrokeThickness = 1,
                Color = OxyColors.Red,
                Type = LineAnnotationType.Horizontal,
                Y = JumpDetectionUtility.Tolerance,
            };

            LineAnnotation minToleranceLine = new LineAnnotation() {
                StrokeThickness = 1,
                Color = OxyColors.Red,
                Type = LineAnnotationType.Horizontal,
                Y = -JumpDetectionUtility.Tolerance,
            };

            plot.Annotations.Add(baseLine);
            plot.Annotations.Add(maxToleranceLine);
            plot.Annotations.Add(minToleranceLine);

            return plot;
        }
    }
}
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump1 : JumpTestBase {
        public override string FileName => "Jumps\\Jump1.csv";

        [TestMethod]
        public void Analyse() {
            Assert.IsTrue(JumpDetectionUtility.Jumps.Count == 2);

            Assert.AreEqual(0.683, JumpDetectionUtility.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(0.683, JumpDetectionUtility.Jumps[1].Airtime, 0.0001);
        }
    }
}

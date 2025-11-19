using Xunit;
using PROG6212_POE.Models;
using System;

namespace CMCSTests
{
    public class ClaimTests
    {
        // TEST 1: Does the auto-calculation work for normal input?
        [Fact]
        public void CalculateTotalAmount_ValidInputs_ReturnsCorrectTotal()
        {
            // Arrange
            var claim = new Claim
            {
                HoursWorked = 10,
                HourlyRate = 500
            };

            // Act
            claim.CalculateTotalAmount();

            // Assert
            Assert.Equal(5000, claim.TotalAmount);
        }

        // TEST 2: Does the system prevent negative values? (Error Handling)
        [Fact]
        public void CalculateTotalAmount_NegativeHours_ThrowsException()
        {
            // Arrange
            var claim = new Claim
            {
                HoursWorked = -5,
                HourlyRate = 500
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => claim.CalculateTotalAmount());
            Assert.Equal("Hours and Rate cannot be negative.", exception.Message);
        }

        // TEST 3: Business Rule - Does it block impossible hours?
        [Fact]
        public void CalculateTotalAmount_HoursExceed24_ThrowsException()
        {
            // Arrange
            var claim = new Claim
            {
                HoursWorked = 25, // Impossible in one day
                HourlyRate = 500
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => claim.CalculateTotalAmount());
        }

        // TEST 4: Does the status update workflow work correctly?
        [Fact]
        public void ClaimStatus_DefaultIsSubmitted()
        {
            // Arrange
            var claim = new Claim();

            // Act (Initialization)

            // Assert
            Assert.Equal("Submitted", claim.Status); // Verifies default state
        }

        // TEST 5: Rounding Logic (Financial Reliability)
        [Fact]
        public void CalculateTotalAmount_DecimalInputs_RoundsCorrectly()
        {
            // Arrange
            var claim = new Claim
            {
                HoursWorked = 10.5m,
                HourlyRate = 150.50m
            };

            // Act
            claim.CalculateTotalAmount();

            // Assert
            // 10.5 * 150.50 = 1580.25
            Assert.Equal(1580.25m, claim.TotalAmount);
        }
    }
}
using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Shared.Tests;

internal class InputTests
{
    [Test]
    public void GetNumbersWorksForAllNumbers()
    {
        var numbers = Input.GetDigits("0123456789");

        for (int n = 0; n < 10; n++) numbers[n].Should().Be(n);
    }

    [Test]
    public void GetNumbersCanSplit()
    {
        var numbers = Input.GetNumbers("0,1,2,3,4,5,6,7,8,9", [',']);

        for (int n = 0; n < 10; n++) numbers[n].Should().Be(n);
    }

    [Test]
    public void SplitGetNumbersCanBeLarger()
    {
        var numbers = Input.GetNumbers("10,11,12,13,14,15,16,17,18,19", [',']);

        for (int n = 0; n < 10; n++) numbers[n].Should().Be(n + 10);
    }

    [Test]
    public void SpitGetNumbersDontCareAboutSpaces()
    {
        var numbers = Input.GetNumbers("0, 1, 2, 3, 4, 5, 6, 7, 8, 9", [',']);

        for (int n = 0; n < 10; n++) numbers[n].Should().Be(n);
    }

    [Test]
    public void SplitGetNumbersWorksWithNegativeNumbers()
    {
        var numbers = Input.GetNumbers("0,-1,-2,-3,-4,-5,-6,-7,-8,-9", [',']);

        for (int n = 0; n < 10; n++) numbers[n].Should().Be(-n);
    }
}
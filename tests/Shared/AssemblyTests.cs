// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit;

public static class AssemblyTests
{
    [Fact]
    public static void Library_Is_Strong_Named()
    {
        // Arrange
        var assembly = typeof(XUnitLoggerOptions).Assembly;

        // Act
        var name = assembly.GetName();
        var actual = name.GetPublicKey();

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldNotBeEmpty();
        Convert.ToHexStringLower(actual).ShouldBe("00240000048000009400000006020000002400005253413100040000010001004b0b2efbada897147aa03d2076278890aefe2f8023562336d206ec8a719b06e89461c31b43abec615918d509158629f93385930c030494509e418bf396d69ce7dbe0b5b2db1a81543ab42777cb98210677fed69dbeb3237492a7ad69e87a1911ed20eb2d7c300238dc6f6403e3d04a1351c5cb369de4e022b18fbec70f7d21ed");

        actual = name.GetPublicKeyToken();
        actual.ShouldNotBeNull();
        actual.ShouldNotBeEmpty();
        Convert.ToHexStringLower(actual).ShouldBe("9a192a7522c9e1a0");
    }
}

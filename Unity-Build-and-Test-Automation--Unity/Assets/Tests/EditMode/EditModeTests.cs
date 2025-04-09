using NUnit.Framework;
using UnityEngine;

public class EditModeTests
{

    [Test]
    public void TestOne()
    {
        Debug.Log("TestOne");
        Assert.IsTrue(true);
    }
    
    [Test]
    public void TestTwo()
    {
        Debug.Log("TestTwo");
        Assert.IsTrue(true, "Test two failed");
    }
    
}

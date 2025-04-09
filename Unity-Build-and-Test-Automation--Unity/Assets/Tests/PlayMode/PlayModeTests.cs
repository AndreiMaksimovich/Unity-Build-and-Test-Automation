using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [OneTimeSetUp]
    public void SetUp()
    {
        
    }
    
    [Test]
    public void SimpleTest()
    {
    }

    [UnityTest]
    public IEnumerator RunTimeTest()
    {
        
        var task = Addressables.LoadSceneAsync("Scenes/Demo.01", LoadSceneMode.Single);
        yield return task;
        
        for (var i = 0; i < 10; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1);
        }
        
        yield return null;
    }
}

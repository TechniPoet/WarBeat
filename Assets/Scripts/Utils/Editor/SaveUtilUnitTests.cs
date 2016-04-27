using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class SaveUtilUnitTests
{
	const string TEST_KEY = "TEST_PREF_KEY";


    [Test]
    public void EditorTest()
    {
        //Arrange
        var gameObject = new GameObject();

        //Act
        //Try to rename the GameObject
        var newGameObjectName = "My game object";
        gameObject.name = newGameObjectName;

        //Assert
        //The object has a new name
        Assert.AreEqual(newGameObjectName, gameObject.name);
    }


	[Test]
	public void GenericSaveList()
	{
		List<ConditionalItem> tempList = new List<ConditionalItem>();
		ConditionalItem condItem = new ConditionalItem();
		for (int i = 0; i < 10; i++)
		{
			condItem = new ConditionalItem();
			condItem.cond1Val = i;
			condItem.cond2Val = i;
			tempList.Add(condItem);
		}

		SaveUtil.SaveList<ConditionalItem>(tempList, TEST_KEY);
		List<ConditionalItem> loadedList = SaveUtil.LoadList<ConditionalItem>(TEST_KEY);

		Assert.AreEqual(tempList.Count, loadedList.Count);

		for (int i = 0; i < tempList.Count; i++)
		{
			Assert.AreEqual(tempList[i].GetSentence(), loadedList[i].GetSentence());
		}
		PlayerPrefs.DeleteKey(TEST_KEY);
	}
}
